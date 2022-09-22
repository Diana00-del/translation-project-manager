using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TranslationProjectManager.Data;
using TranslationProjectManager.Services;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TranslationProjectManagerContextConnection");

builder.Services.AddDbContext<TranslationProjectManagerContext>(options => options.UseLazyLoadingProxies().UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<TranslationProjectManagerContext>();

// Add services to the container.
builder.Services.AddMvc(options => {
    options.EnableEndpointRouting = false;
});
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileStorageConfigurationService, FileStorageConfigurationService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseMvcWithDefaultRoute();

app.Run();
