using Amazon.S3;
using Amazon.S3.Model;

namespace movie_library.Services;

public class VideosService
{
    private readonly IAmazonS3 _s3Client;
    private const string BucketName = "movies-library-videos";

    public VideosService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<List<string>> GetUploadedVideosKeys()
    {
        var listObjectsRequest = new ListObjectsRequest
        {
            BucketName = BucketName
        };

        var response = await _s3Client.ListObjectsAsync(listObjectsRequest);

        return response.S3Objects.Select(obj => obj.Key).ToList();
    }
}