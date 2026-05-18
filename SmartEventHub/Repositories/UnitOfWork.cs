using SmartEventHub.Data;
using SmartEventHub.Entities;

namespace SmartEventHub.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IGenericRepository<User> Users { get; private set; }
        public IGenericRepository<Event> Events { get; private set; }
        public IGenericRepository<Session> Sessions { get; private set; }
        public IGenericRepository<Registration> Registrations { get; private set; }
        public IGenericRepository<Notification> Notifications { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            // Initialize all repositories with the same context
            Users = new GenericRepository<User>(_context);
            Events = new GenericRepository<Event>(_context);
            Sessions = new GenericRepository<Session>(_context);
            Registrations = new GenericRepository<Registration>(_context);
            Notifications = new GenericRepository<Notification>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            // Executes all tracked database operations in one go
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}