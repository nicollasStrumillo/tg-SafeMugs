using backend.DTOs.Desafio;

namespace backend.Services.Interfaces.Util;
public interface INotificationService
{
    Task NotifyDesafioSolved(DesafioResponse desafio);
    Task ReplayNotifications(string connectionId);
    Task AcknowledgeNotification(int desafioId);
}
