using AutoMapper;
using Microsoft.EntityFrameworkCore;
using rentapp.BL.Dtos.Customer;
using rentapp.BL.Entities;
using rentapp.Data.Repositories.Interfaces;
using rentapp.Service.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rentapp.Service.Services
{
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDto> Delete(int id)
        {
            var customer = await _customerRepository.GetById(id);
            if (customer == null)
            {
                throw new KeyNotFoundException("No se ha podido borrar el cliente");
            }

            await _customerRepository.Delete(customer);

            CustomerDto dto = _mapper.Map<Customer, CustomerDto>(customer);

            return dto;
        }

        public Task<CustomerItemDto> GetById(int id)
        {
            return _customerRepository.GetItemDtoById(id);
        }

        public async Task<List<CustomerItemDto>> GetCustomers()
        {
            return await _customerRepository.GetCustomers();
        }

        public async Task SaveCustomer(CustomerDto dto)
        {
            Customer obj = null;

            if (dto.CustomerId == 0)
            {
                obj = _mapper.Map<CustomerDto, Customer>(dto);
                obj.DateCreated = DateTime.Now;
                obj.CreatedUserId = user.UserId;
                _mapper.Map(dto.CustomerAddresses, obj.CustomerAddresses);
                obj.CustomerAddresses = obj.CustomerAddresses.Select(p => { p.DateCreated = DateTime.Now; p.CreatedUserId = user.UserId; return p; }).ToList();
            }
            else
            {
                obj = await _customerRepository.GetById(dto.CustomerId, p => p.Include(x => x.CustomerAddresses.Where(p => p.IsActive)));
                if (obj == null)
                {
                    throw new KeyNotFoundException("El cliente no éxiste");
                }
                _mapper.Map(dto, obj);
                obj.UpdatedUserId = user.UserId;
                obj.DateUpdated = DateTime.Now;

                obj.CustomerAddresses = _mapper.Map(dto.CustomerAddresses, obj.CustomerAddresses);
                obj.CustomerAddresses = obj.CustomerAddresses.Select(p => { p.DateUpdated = DateTime.Now; p.UpdatedUserId = user.UserId; return p; }).ToList();
            }

            await _customerRepository.Save(obj);

            dto = _mapper.Map<Customer, CustomerDto>(obj);

        }
    }
}
