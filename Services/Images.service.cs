using Amazon.S3;
using Amazon.S3.Model;

namespace movie_library.Services;

public class ImagesService
{
    private readonly IAmazonS3 _s3Client;
    
    private const string BucketName = "movies-library-images";

    public ImagesService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<string> UploadAsync(string base64Image)
    {
        // Decode the Base64 string to a byte array
        var imageBytes = Convert.FromBase64String(base64Image);

        // Generate a unique file name for the image
        var fileName = Guid.NewGuid() + ".webp";

        // Create a new S3 object for the image
        var s3Object = new PutObjectRequest
        {
            BucketName = BucketName,
            Key = fileName,
            InputStream = new MemoryStream(imageBytes)
        };
        
        // Upload the image to S3
        await _s3Client.PutObjectAsync(s3Object);
            
        // Return the URL of the image
        return $"https://{BucketName}.s3.amazonaws.com/{fileName}";
    }
}