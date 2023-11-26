using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.S3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using movie_library.Config;
using movie_library.Services;

namespace movie_library;

public class Startup
{
    private IConfiguration Configuration { get; }
    private AWSConfig AwsConfig { get; }
    
    public Startup(IConfiguration configuration, AWSConfig awsConfig)
    {
        Configuration = configuration;
        AwsConfig = awsConfig;
    }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.Configure<DatabaseConfig>(Configuration.GetSection("DB"));

        var amazonCognitoIdentityProvider = new AmazonCognitoIdentityProviderClient();
        var cognitoUserPool = new CognitoUserPool(AwsConfig.UserPoolId, AwsConfig.UserPoolClientId, amazonCognitoIdentityProvider, AwsConfig.AppClientSecret);

        services.AddSingleton<IAmazonCognitoIdentityProvider>(x => amazonCognitoIdentityProvider);
        services.AddSingleton<CognitoUserPool>(x => cognitoUserPool);

        services.AddCognitoIdentity();
        services.AddAWSService<IAmazonS3>();
        
        services.AddSingleton<AccountsService>();
        services.AddSingleton<MoviesService>();
        services.AddSingleton<ImagesService>();
        services.AddSingleton<CommentsService>();
        services.AddSingleton<RatingsService>();
        
        services.AddAuthentication(options =>
        {
            options .DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = $"https://cognito-idp.{AwsConfig.Region}.amazonaws.com/{AwsConfig.UserPoolId}";
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer =
                    $"https://cognito-idp.{AwsConfig.Region}.amazonaws.com/{AwsConfig.UserPoolId}",
                ValidateLifetime = true,
                LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                ValidateAudience = false
            };
        });
        
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "cognito:groups" && c.Value == "admin")));
            
            options.AddPolicy("AdminOrModeratorOnly", policy =>
                policy.RequireAssertion(context =>
                    context.User.HasClaim(c => c.Type == "cognito:groups" && c.Value == "admin" || c.Value == "moderator")));
        });

        services.AddControllers()
            .AddJsonOptions(
                options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
        
        services.AddCors(options =>
        {
            options.AddPolicy(name: "MovieLibraryPolicy",
                policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors("MovieLibraryPolicy");
        
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}