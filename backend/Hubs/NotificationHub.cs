using backend.Services.Interfaces.Util;
using Microsoft.AspNetCore.SignalR;

namespace backend.Hubs;

public sealed class NotificationHub : Hub
{
    private readonly INotificationService _notificationService;

    public NotificationHub(INotificationService notificationService)
    {
        _notificationService = notificationService;
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

}
