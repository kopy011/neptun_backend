using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using neptun_backend.Context;
using neptun_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Add services
builder.Services.AddScoped<IInstructorService, InstructorService>();
builder.Services.AddScoped<ISemesterService, SemesterService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllers();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
