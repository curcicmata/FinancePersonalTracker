using FinancePersonalTracker.Components;
using FinancePersonalTracker.Components.Account;
using FinancePersonalTracker.Data;
using FinancePersonalTracker.Interface;
using FinancePersonalTracker.Models;
using FinancePersonalTracker.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

//builder.Services.Configure<SendGridOptions>(builder.Configuration.GetSection("SendGrid"));
//builder.Services.AddTransient<IEmailSender<ApplicationUser>, SendGridEmailSender>();
//builder.Services.Configure<SendGridOptions>(builder.Configuration);

builder.Services.Configure<SmtpOptions>(builder.Configuration.GetSection("SmtpOptions"));
builder.Services.AddScoped<IEmailSender<ApplicationUser>, SmtpEmailSender>();
//builder.Services.AddScoped<IEmailSender<ApplicationUser>, SendGridEmailSender>();



builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IFamilyGroupService, FamilyGroupService>();

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();



using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var retries = 10;

    while (retries > 0)
    {
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            await context.Database.MigrateAsync();
            Console.WriteLine("Database migrated successfully.");
            break;
        }
        catch (Npgsql.NpgsqlException ex)
        {
            retries--;
            logger.LogWarning("Postgres not ready yet. Retrying in 5s...");
            await Task.Delay(5000);
        }
    }
}



app.Run();
