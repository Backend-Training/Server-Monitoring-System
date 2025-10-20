using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ServerMonitoringSystem.SignalRHub.AnomalyAlertsHubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<AlertHub>("/alertHub");

app.Run();