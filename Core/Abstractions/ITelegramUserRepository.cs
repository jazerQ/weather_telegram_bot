using Core.Entities;

namespace Core.Abstractions
{
    public interface ITelegramUserRepository
    {
        Task AddUser(TelegramUser user, CancellationToken cancellationToken);
        Task Update(long id, string name, CancellationToken cancellationToken);
        Task<string> GetNameById(long id, CancellationToken cancellationToken);
    }
}