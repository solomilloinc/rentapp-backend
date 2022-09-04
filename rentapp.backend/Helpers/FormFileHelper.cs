using rentapp.BL.Dtos;
using System.Drawing;

namespace NG.Web.Helpers
{
    public static class FormFileHelper
    {
        public static FileBytesDto ReadFormFileContent(IFormFile formFile)
        {
            if (formFile == null)
            {
                return null;
            }

            FileBytesDto dto = new FileBytesDto();
            dto.FileName = formFile.FileName;

            using (var stream = formFile.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);

                    dto.Bytes = ms.ToArray();
                }
            }

            return dto;
        }

        public static async Task<FileBytesDto> ReadFormFileContentAsync(IFormFile formFile)
        {
            if (formFile == null)
            {
                return null;
            }

            FileBytesDto dto = new FileBytesDto();
            dto.FileName = formFile.FileName;

            using (var stream = formFile.OpenReadStream())
            {
                using (var ms = new MemoryStream())
                {
                    await stream.CopyToAsync(ms);

                    dto.Bytes = ms.ToArray();
                }
            }

            return dto;
        }

        public static Tuple<Stream, Image> GetImage(IFormFile formFile)
        {
            if (formFile == null)
            {
                return null;
            }

            Stream stream = null;
            try
            {
                stream = formFile.OpenReadStream();

                return new Tuple<Stream, Image>(stream, Image.FromStream(stream));
            }
            catch (Exception)
            {
                if (stream != null)
                {
                    stream.Dispose();
                }

                return null;
            }
        }

    }
}
