using static FGC.Domain.Core.NotificationModel;
 
namespace FGC.Domain.Core
{
    public interface INotification
    {
        NotificationModel NotificationModel { get; }
        bool HasNotification { get; }
        void AddNotification(string key, string message, ENotificationType notificationType);
    }
}
