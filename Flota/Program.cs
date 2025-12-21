using System;
using Microsoft.EntityFrameworkCore;
using Flota.Components;
using Flota.Domain.Interfaces;
using Flota.Infrastructure.Data;
using Flota.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new Exception("Brak ConnectionString");
builder.Services.AddDbContext<FleetDbContext>(o => o.UseSqlServer(conn));

builder.Services.AddScoped<IPojazdSerwis, PojazdSerwis>();
builder.Services.AddScoped<ITankowanieSerwis, TankowanieSerwis>();

var app = builder.Build();

// Seeding bazy
using (var scope = app.Services.CreateScope()) {
    var db = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
    DbInitializer.Initialize(db);
}

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();