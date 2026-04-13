using GestionAbsences.DTOs.Notification;

namespace GestionAbsences.Services.Notification
{
    public interface INotificationService
    {
        Task CreateNotificationAsync(int userId, string title, string message);
        Task<IEnumerable<NotificationResponseDto>> GetUserNotificationsAsync(int userId);
        Task<IEnumerable<NotificationResponseDto>> GetUnreadNotificationsAsync(int userId);
        Task<bool> MarkAsReadAsync(int notificationId, int userId);
        Task<bool> DeleteNotificationAsync(int notificationId, int userId);
    }
}
