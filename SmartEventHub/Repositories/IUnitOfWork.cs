using SmartEventHub.Entities;

namespace SmartEventHub.Repositories
{
    // Centralizes all repository operations and database saves
    public interface IUnitOfWork : IDisposable
    {
        // Define repositories for our entities
        IGenericRepository<User> Users { get; }
        IGenericRepository<Event> Events { get; }
        IGenericRepository<Session> Sessions { get; }
        IGenericRepository<Registration> Registrations { get; }
        IGenericRepository<Notification> Notifications { get; }

        // Saves all changes made in this context to the database
        Task<int> CompleteAsync();
    }
}