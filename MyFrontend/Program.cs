using MyFrontend;
using MyFrontend.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IZeebeService, ZeebeService>();

DotEnv.Load(Path.Combine(Directory.GetCurrentDirectory(), "CamundaCloud.env"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();