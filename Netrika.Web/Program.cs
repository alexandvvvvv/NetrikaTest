using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Netrika.Services.MedicalOrganizations;
using Netrika.Services.Utils;
using NetrikaTest.Services.MedicalOrganizations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOptions();

builder.Services.AddControllersWithViews();
builder.Services.RegisterMedicalOrganizationsServices();
builder.Services.RegisterUtilityValidators();
builder.Services.Configure<MedicalOrganizationsParams>(options => builder.Configuration.Bind("MedicalOrganizationsParams", options));
builder.Services.AddSwaggerGen();


//load general configuration from appsettings.json
builder.Services.Configure<IpRateLimitOptions>(options => builder.Configuration.Bind("IpRateLimiting", options));

//load ip rules from appsettings.json
builder.Services.Configure<IpRateLimitPolicies>(options => builder.Configuration.Bind("IpRateLimitPolicies", options));
builder.Services.AddMemoryCache();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.UseIpRateLimiting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
