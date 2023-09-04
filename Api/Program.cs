using Api.Core;
using Api.Presentation.Constants;
using Api.Presentation.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppConfiguration>()
                    ?? throw new ArgumentNullException(ErrorConstants.AppConfigurationMessage);


builder.Services.AddSingleton(configuration);
var app = await builder.ConfigureServices(configuration)
                        .ConfigurePipelineAsync(configuration);

app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

NewRelic.Api.Agent.NewRelic.StartAgent();

app.Run();
