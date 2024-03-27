using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using GamifyingTasks.Firebase.DB;
using Fluxor;
using GamifyingTasks.Data;
using GamifyingTasks.Firebase.DB.Interfaces;
using Blazored.LocalStorage;

var builder = WebApplication.CreateBuilder(args);

// Add Services to the web application
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddFluxor(o =>
{
    o.ScanAssemblies(typeof(Program).Assembly);
    o.UseReduxDevTools(rdt =>
      {
          rdt.Name = "My application";
      });
});

// Add Custom Services
builder.Services.AddScoped<IDBCore, DBCore>();
builder.Services.AddScoped<IUsers, DBCoreUsers>();
builder.Services.AddScoped<ITasks, DBCoreTasks>();
builder.Services.AddScoped<IEvents, DBCoreEventsReminders>();
builder.Services.AddScoped<IGoals, DBCoreGoals>();

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
