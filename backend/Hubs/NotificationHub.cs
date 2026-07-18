using backend.Services.Interfaces;
using backend.Services.Interfaces.Util;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

public sealed class NotificationHub : Hub
{
    private readonly INotificationService _notificationService;
    private readonly IDesafioService _desafioService;

    public NotificationHub(INotificationService notificationService, IDesafioService desafioService)
    {
        _notificationService = notificationService;
        _desafioService = desafioService;
    }

    public override async Task OnConnectedAsync()
    {
        await _notificationService.ReplayNotifications(Context.ConnectionId);

        await base.OnConnectedAsync();
    }

    public async Task AcknowledgeNotification(int desafioId)
    {
        await _notificationService.AcknowledgeNotification(desafioId);
    }

    public async Task SolveDesafioDomXss(string payload)
    {
        await _desafioService.SolveIfAsync("DOM XSS", () => payload == "<iframe src=\"javascript:alert(`XSS`)\">");
    }

    public async Task SolveDesafioStoredXss(string payload)
    {
        await _desafioService.SolveIfAsync("Stored XSS", () => payload == "<iframe src=\"javascript:alert(`XSS`)\">");
    }
}
