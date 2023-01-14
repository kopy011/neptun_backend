using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using neptun_backend.Context;
using neptun_backend.Entities;
using neptun_backend.Middleware;
using neptun_backend.Services;
using neptun_backend.UnitOfWork;
using neptun_backend.Utils;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Add services
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<NeptunBackendDbContext>()
                .AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(cfg =>
    {
        cfg.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://mik.uni-pannon.hu",
            ValidateAudience = true,
            ValidAudience = "https://mik.uni-pannon.hu",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Secret_Key_12345")),
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ActivePersonOnly",
       policy => policy.RequireClaim(UserClaims.ISDELETED, "False"));
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork<NeptunBackendDbContext>>();
builder.Services.AddScoped<ICourseUnitOfWork, CourseUnitOfWork<NeptunBackendDbContext>>();

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

//connection string
builder.Services.AddDbContext<NeptunBackendDbContext>(
    options => options.UseSqlServer("Server=.;Database=NeptunBackend;Trusted_Connection=True;MultipleActiveResultSets=True"));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<RequestResultMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
