using System.Collections.Concurrent;
using backend.DTOs.Desafio;
using backend.Hubs;
using backend.Services.Interfaces.Util;
using Microsoft.AspNetCore.SignalR;

namespace backend.Services.Implementations.Util;
public class NotificationService : INotificationService
{
    private readonly IHubContext<NotificationHub> _notificationHub;

    private readonly ConcurrentDictionary<int, DesafioResponse> _pendingNotifications = [];

    public NotificationService(IHubContext<NotificationHub> notificationHub)
    {
        _notificationHub = notificationHub;
    }

    public async Task NotifyDesafioSolved(DesafioResponse desafio)
    {
        if (_pendingNotifications.ContainsKey(desafio.Id))      
            return; 
        
        _pendingNotifications[desafio.Id] = desafio;
        await _notificationHub.Clients.All.SendAsync("DesafioSolved", desafio);
    }

    public async Task ReplayNotifications(string connectionId)
    {
        var notificacoesPendentes = _pendingNotifications.Values.ToList();

        foreach (var desafio in notificacoesPendentes) 
            await _notificationHub.Clients.Client(connectionId).SendAsync("DesafioSolved", desafio);
    }

    public async Task AcknowledgeNotification(int desafioId)
    {
        _pendingNotifications.TryRemove(desafioId, out _);
    }
}
