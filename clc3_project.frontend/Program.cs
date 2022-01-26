using CLC3_Project.Frontend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<BooksService>();
builder.Services.AddSingleton<BooksService>();
builder.Services.AddHttpClient<ReadListService>();
builder.Services.AddSingleton<ReadListService>();

builder.Services.AddRazorPages();

var app = builder.Build();



// Configure the HTTP request pipeline.


app.UseExceptionHandler("/Home/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
