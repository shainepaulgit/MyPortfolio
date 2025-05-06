using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyPortfolio.Configurations;
using MyPortfolio.Data;
using MyPortfolio.Models.Entities;
using MyPortfolio.Models.Repositories.Contracts;
using MyPortfolio.Models.Repositories.Implementations;
using MyPortfolio.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
var gitHubSettings = builder.Configuration.GetSection("GitHubSettings");
var emailSettings = builder.Configuration.GetSection("EmailSettings");  
builder.Services.Configure<GitHubSettings>(gitHubSettings);
builder.Services.Configure<EmailSettings>(emailSettings);
builder.Services.AddHttpClient();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString)
           .UseLazyLoadingProxies();

});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<AppIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddAutoMapper(typeof(ModelMapper));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<FileHandling>();
builder.Services.AddScoped<EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


//seeding
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new List<string>() {"Admin"};
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
    var users = userManager.Users.ToList();
    var isZeroUsers = users.Count() == 0;   
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    if (isZeroUsers)
    {
        var initialPassword = "@ShainePaul09";
        var initialUser = new AppIdentityUser
        {
            UserName = "shaine_paul09",
            Email = "shainepaulm@gmail.com",
            PhoneNumber = "09679369759",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,

            FirstName = "Shaine Paul",
            MiddleName = "M",
            LastName = "Calzo",
            DateOfBirth = new DateTime(2003, 5, 29),
            Website = "shainepaul.co.ph",
            Degree = "Bachelor of Science in Information Technology",
            Address = "Bagong Silang Victoria Oriental Mindoro",
            ProfilePictureFileName = "DefaultProfilePicture.png",
            ResumeFileName = "MyResumeDefault.pdf",
          

        };
        await userManager.CreateAsync(initialUser,initialPassword); 
        await userManager.AddToRoleAsync(initialUser, "Admin");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
