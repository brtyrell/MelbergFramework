using MelbergFramework.Core.HealthCheck;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MelbergFramework.Application;

public class MelbergHost
{
    public delegate void 
        RegisterServices(IServiceCollection services);
    public RegisterServices ServiceActions = (IServiceCollection _) => {};

    public delegate void 
        ConfigureApplication(WebApplication app);
    public ConfigureApplication AppActions = (WebApplication _) => {};


    private MelbergHost() { }

    public static MelbergHost CreateHost()
    {
        return new MelbergHost()
            .AddHealthRegistration();
    }
    
    public MelbergHost AddServices(RegisterServices serviceAction)
    {
        ServiceActions += serviceAction;

        return this;
    }

    public MelbergHost AddHealthRegistration()
    {
        ServiceActions += (IServiceCollection _) => { 
            HealthCheckModule.RegisterHealthCheckChecker(_);
            };

        return this;
    }

    public MelbergHost ConfigureApp(ConfigureApplication appAction) 
    {
        AppActions += appAction;

        return this;
    }

    public MelbergHost DevelopmentPasswordReplacement(
            string key,
            string replacementKey)
    {
        AppActions += (WebApplication _) => 
        {
            if(_.Environment.IsDevelopment())
            {
                _.Configuration[key] = _.Configuration[replacementKey];
            }
        };

        return this;
    }

    public MelbergHost AddControllers()
    {
        ServiceActions += (IServiceCollection _) => { _.AddControllers(); };
        return this;
    }

    public WebApplication Build()
    {
        var builder = WebApplication.CreateBuilder();

        ServiceActions(builder.Services);
        builder.Services
            .AddOptions<ApplicationConfiguration>()
            .BindConfiguration(ApplicationConfiguration.Section)
            .ValidateDataAnnotations();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        AppActions(app);

        app.MapGet("/health", async () => 
                await app
                    .Services
                    .GetRequiredService<IHealthCheckChecker>()
                    .IsOk());

        app.UseHttpsRedirection();

        app.UseCors(_ => _
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin => true)
            .AllowCredentials()
            );

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

}
