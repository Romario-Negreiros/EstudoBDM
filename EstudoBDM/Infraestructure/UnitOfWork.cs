#pragma warning disable CS0649

using EstudoBDM.Configs;
using EstudoBDM.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EstudoBDM.Infraestructure
{
    public interface IUnitOfWork
    {
        public IEmployeeRepository EmployeeRepository { get; }
        void Commit();
        void Rollback();
        void Dispose();
    }
    public class UnitOfWork(DatabaseConnection _dbCon) : IUnitOfWork
    {
        private readonly EmployeeRepository? _employeeRepository;

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                return _employeeRepository ?? new EmployeeRepository(dbCon);
            }
        }

        public DatabaseConnection dbCon = _dbCon;

        public async void Commit()
        {
            await dbCon.SaveChangesAsync();
        }

        public void Rollback()
        {
            EntityState[] statesToRollback = [EntityState.Added, EntityState.Modified, EntityState.Deleted];

            foreach (var entry in dbCon.ChangeTracker.Entries())
            {
                if (statesToRollback.Contains(entry.State))
                {
                    entry.State = EntityState.Detached;
                }
            }
        }
        public async void Dispose()
        {
            await dbCon.DisposeAsync();
        }
    }
}
