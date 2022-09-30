using Netrika.Services.MedicalOrganizations;
using Netrika.Services.Utils;
using NetrikaTest.Services.MedicalOrganizations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.RegisterMedicalOrganizationsServices();
builder.Services.RegisterUtilityValidators();
builder.Services.Configure<MedicalOrganizationsParams>(options => builder.Configuration.Bind("MedicalOrganizationsParams", options));
builder.Services.AddSwaggerGen();

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


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
