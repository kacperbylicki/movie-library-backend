namespace movie_library.Config;

public class AWSConfig
{
    public string Region { get; set; }
    public string AccessKeyId { get; set; }
    public string SecretAccessKey { get; set; }
    public string UserPoolId { get; set; }
    public string AppClientId { get; set; }
    public string UserPoolClientId { get; set; }
    public string AppClientSecret { get; set; }
    
    public AWSConfig()
    {
        Region = Environment.GetEnvironmentVariable("AWS_REGION") ?? "eu-central-1";
        AccessKeyId = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        SecretAccessKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
        UserPoolId = Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_ID");
        AppClientId = Environment.GetEnvironmentVariable("AWS_COGNITO_APP_CLIENT_ID");
        UserPoolClientId = Environment.GetEnvironmentVariable("AWS_COGNITO_USER_POOL_CLIENT_ID");
        AppClientSecret = Environment.GetEnvironmentVariable("AWS_COGNITO_APP_CLIENT_SECRET");
    }
}