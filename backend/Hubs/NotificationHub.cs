using backend.models.Enums;
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

    private static readonly HashSet<DesafiosEnum> DesafiosXss =
    [DesafiosEnum.DomXss, DesafiosEnum.StoredXss, DesafiosEnum.ReflectedXss];
    private const string PayloadXssEsperado = "<iframe src=\"javascript:alert(`XSS`)\">";

    public async Task SolveDesafioXss(string nomeDesafio, string payloadXss)
    {
        if (!EnumExtensions.TryGetEnumByNomeDisplay<DesafiosEnum>(nomeDesafio, out var enumDesafio)) return;
        if (!DesafiosXss.Contains(enumDesafio)) return;   
        await _desafioService.SolveIfAsync(enumDesafio, () => payloadXss == PayloadXssEsperado);
    }
}
