using DotNetEnv;
using movie_library;
using movie_library.Config;

var builder = WebApplication.CreateBuilder(args);

// Load environmental variables from .env
Env.Load();

// Create config builder that can access env variables
var configBuilder = builder.Configuration.AddEnvironmentVariables();
var config = configBuilder.Build();

// Create AwsConfig instance
var awsConfig = new AWSConfig();

// Manually create an instance of the Startup class
var startup = new Startup(config, awsConfig);

// Manually call ConfigureServices()
startup.ConfigureServices(builder.Services);

var app = builder.Build();

// Call Configure(), passing in the dependencies
startup.Configure(app, app.Environment);

app.Run();