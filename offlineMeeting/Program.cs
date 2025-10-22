using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using offlineMeeting.Models.Entity.Login;
using offlineMeeting.Models.Entity.Share;

var builder = WebApplication.CreateBuilder(args);

var provider = new FileExtensionContentTypeProvider();

// AddSignalR to the container.
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add PWA to the container.
builder.Services.AddProgressiveWebApp();

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider,
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath)),

});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.Environment.ContentRootPath = Environment.CurrentDirectory;
EnvironmentEntity.ContentRootPath = Environment.CurrentDirectory;

// Hub  routing  ݒ 
app.MapHub<MyHub>("/hub");

app.UseDefaultFiles();
app.UseHttpsRedirection();
//app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
