using System.Linq.Expressions;

namespace SmartEventHub.Repositories
{
    // A generic interface that can handle any entity (User, Event, Session, etc.)
    public interface IGenericRepository<T> where T : class
    {
        // Get all records, optionally with a condition
        Task<IEnumerable<T>> GetAllAsync();

        // Find a specific record based on a condition (e.g., finding a user by email)
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        // Get a single record by its ID
        Task<T?> GetByIdAsync(Guid id);

        // Add a new record
        Task AddAsync(T entity);

        // Update an existing record
        void Update(T entity);

        // Delete a record
        void Remove(T entity);
    }
}