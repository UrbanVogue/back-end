using IdentityServer;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using IdentityServer.Data;
using IdentityServer.Configurations;
using EmailService;
using IdentityServer.Seeder;
using IdentityServer4.AspNetIdentity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLogging();

builder.Services.AddControllersWithViews();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IMailSender, MailSender>();

builder.Services.AddSingleton<IIdentityServerConfiguration, IdentityServerConfiguration>();
builder.Services.AddSingleton<ISeeder, Seeder>();

var mobileConfig = builder.Configuration.GetSection(nameof(MobileClientConfiguration)).Get<MobileClientConfiguration>();
builder.Services.AddSingleton(mobileConfig);

var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AspNetCoreIdentityDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AspNetCoreIdentityDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<DataProtectionTokenProviderOptions>(opt =>
   opt.TokenLifespan = TimeSpan.FromHours(2));

builder.Services.AddIdentityServer()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(connectionString,
                sql => sql.MigrationsAssembly(migrationsAssembly));

        options.EnableTokenCleanup = true;
    })
    .AddDeveloperSigningCredential()
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator<IdentityUser>>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "IdentityServer.Cookie";
    config.Cookie.SameSite = SameSiteMode.None;
    config.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyOrigin();
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowAnyHeader();
});

app.UseIdentityServer();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
        await seeder.EnsureSeedDataAsync(scope.ServiceProvider);
}

app.Run();
