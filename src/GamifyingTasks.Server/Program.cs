using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using GamifyingTasks.Firebase.DB;
using Fluxor;
using GamifyingTasks.Data;
using GamifyingTasks.Firebase.DB.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddFluxor(o =>
{
    o.ScanAssemblies(typeof(Program).Assembly);
    o.UseReduxDevTools(rdt =>
      {
          rdt.Name = "My application";
      });
});

builder.Services.AddScoped<IDBCore>();
builder.Services.AddScoped<IUsers>();
builder.Services.AddScoped<ITasks>();
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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();
