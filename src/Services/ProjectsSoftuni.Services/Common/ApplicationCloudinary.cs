namespace ProjectsSoftuni.Services.Common
{
    using System.IO;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public static class ApplicationCloudinary
    {
        public static async Task<string> UploadFile(Cloudinary cloudinary, IFormFile file, string name)
        {
            byte[] destinationFile;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                destinationFile = memoryStream.ToArray();
            }

            using (var ms = new MemoryStream(destinationFile))
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(name, ms),
                    PublicId = name,
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult.SecureUri.AbsoluteUri;
            }
        }
    }
}
