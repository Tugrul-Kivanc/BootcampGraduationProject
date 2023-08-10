using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession( options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
builder.Services.AddFluentValidation( options =>
    options.RegisterValidatorsFromAssemblyContaining<Program>());

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();

app.MapControllers();
app.MapGet("", context =>
{
    return Task.Run(() => context.Response.Redirect("/Login"));
});

app.Run();
