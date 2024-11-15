using Microsoft.EntityFrameworkCore;
using DBCRUDEMPLEADO.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Compilar cambios en tiempo real
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//Servicio cadena de conecion con DbContext
builder.Services.AddDbContext<DbcrudempleadoContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"))
); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
