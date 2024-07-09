using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace API;

public class PhotoService : IPhotoService
{
    private readonly Cloudinary _cloudinary;

    public PhotoService(IOptions<CLoudinarySettings> options)
    {
        var acc =new Account(options.Value.CloudName, options.Value.ApiKey, options.Value.ApiSecret);
        _cloudinary = new Cloudinary(acc);
    }
    public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        if(file.Length > 0){
            using var stream = file.OpenReadStream();   
            var uploadPrams = new ImageUploadParams{
                File = new FileDescription (file.FileName ,stream),
                Transformation = new Transformation()
                        .Height(500).Width(500).Crop("fill").Gravity("face"),
                Folder = "da-net8"
            };
            uploadResult = await _cloudinary.UploadAsync(uploadPrams);
        }
        return uploadResult; 
    }

    public async Task<DeletionResult> DeletehotoAsync(string publicId)
    {
        var deletePrams = new DeletionParams(publicId);

        return await _cloudinary.DestroyAsync(deletePrams);


    }
}
