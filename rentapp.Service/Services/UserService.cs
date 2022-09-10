using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using rentapp.BL.Core.Helpers;
using rentapp.BL.Dtos.Auth;
using rentapp.BL.Entities;
using rentapp.Data.Repositories.Interfaces;
using rentapp.Service.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace rentapp.Service.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IHttpContextService _httpContextService;
        private readonly IJwtUtils _jwtUtils;
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings, IUserRepository userRepository, IJwtUtils jwtUtils, IHttpContextService httpContextService)
        {
            _appSettings = appSettings.Value;
            _userRepository = userRepository;
            _jwtUtils = jwtUtils;
            _httpContextService = httpContextService;
        }

        public AuthenticateResponseDto Authenticate(AuthenticateRequestDto model)
        {
            var user = _userRepository.GetByUserName(model.Username);
           
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                throw new KeyNotFoundException("Username or password is incorrect");
            }

            // authentication successful so generate jwt and refresh tokens
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            var refreshToken = _jwtUtils.GenerateRefreshToken(_httpContextService.GetIPAddress());
            user.RefreshTokens.Add(refreshToken);

            // remove old refresh tokens from user
            RemoveOldRefreshTokens(user);

            // save changes to db
            _userRepository.SaveUser(user);

            return new AuthenticateResponseDto(user, jwtToken, refreshToken.Token);
        }

        public AuthenticateResponseDto RefreshToken(string token)
        {
            var user = GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                // revoke all descendant tokens in case this token has been compromised
                RevokeDescendantRefreshTokens(refreshToken, user, _httpContextService.GetIPAddress(), $"Attempted reuse of revoked ancestor token: {token}");
                _userRepository.SaveUser(user);
            }

            if (!refreshToken.IsActive)
                throw new KeyNotFoundException("Invalid token");

            // replace old refresh token with a new one (rotate token)
            var newRefreshToken = RotateRefreshToken(refreshToken, _httpContextService.GetIPAddress());
            user.RefreshTokens.Add(newRefreshToken);

            // remove old refresh tokens from user
            RemoveOldRefreshTokens(user);

            // save changes to db
            _userRepository.SaveUser(user);

            // generate new jwt
            var jwtToken = _jwtUtils.GenerateJwtToken(user);

            return new AuthenticateResponseDto(user, jwtToken, newRefreshToken.Token);
        }

        public void RevokeToken(string token)
        {
            var user = GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            if (!refreshToken.IsActive)
                throw new KeyNotFoundException("Invalid token");

            // revoke token and save
            RevokeRefreshToken(refreshToken, _httpContextService.GetIPAddress(), "Revoked without replacement");
            _userRepository.SaveUser(user);
        }

        public IEnumerable<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User GetById(int id)
        {
            return _userRepository.GetById(id);
        }

        // helper methods

        private User GetUserByRefreshToken(string token)
        {
            var user = _userRepository.GetUserByToken(token);

            if (user == null)
                throw new KeyNotFoundException("Invalid token");

            return user;
        }

        private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = _jwtUtils.GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void RemoveOldRefreshTokens(User user)
        {
            // remove old inactive refresh tokens from user based on TTL in app settings
            user.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
        }

        private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            // recursively traverse the refresh token chain and ensure all descendants are revoked
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
                if (childToken.IsActive)
                    RevokeRefreshToken(childToken, ipAddress, reason);
                else
                    RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
            }
        }

        private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
        {
            token.Revoked = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = reason;
            token.ReplacedByToken = replacedByToken;
        }
    }
}
