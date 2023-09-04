using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authorization;
using MassTransit;
using MCR.App.Abstractions.EventBus;
using MCR.App.Configuration;
using MCR.App.Consumers;
using MCR.App.Settings;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.AddOptions<OutboxSettings>()
    .BindConfiguration("Outbox")
    .ValidateDataAnnotations()
    .ValidateOnStart();
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<OutboxSettings>>().Value);

builder.Services.AddOptions<MessageBrokerSettings>()
    .BindConfiguration("MessageBroker");
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

builder.Services.AddTransient<IEventBus, EventBus>();
builder.Services.AddMediatR(new MediatRServiceConfiguration
{

});

builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();

    busConfigurator.AddConsumer<ProductCreatedEventConsumer>();

    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();

        configurator.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.Username);
            h.Password(settings.Password);
        });
    });
});

builder.Services
    .AddCaching(builder.Configuration)
    .AddInfrastructure(builder.Configuration)
    .AddApplication();

builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();
builder.Services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();

builder.Services.AddKeycloakAuthentication(builder.Configuration);
builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("Database"));

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    rateLimiterOptions.AddTokenBucketLimiter("token", options =>
    {
        options.TokenLimit = 1000;
        options.ReplenishmentPeriod = TimeSpan.FromHours(1);
        options.TokensPerPeriod = 700;
        options.AutoReplenishment = true;
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("RequireWorkspaces", builder =>
        {
            builder.RequireProtectedResource("workspaces", "workspaces:read")
                .RequireRealmRoles("User")
                .RequireResourceRoles("Admin");
        });
    })
    .AddKeycloakAuthorization(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

app.Run();
