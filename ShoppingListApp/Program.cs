var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ShoppingList}/{action=List}/{id?}");

app.MapControllerRoute(
    name: "adminPanel",
    pattern: "{controller=Admin}/{action=Panel}");

app.Run();
