using DataAccess.Data;
using DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            AdminUser = new AdminUserRepository(_db);
            InstructorUser = new InstructorUserRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
        }

        public IAdminUserRepository AdminUser { get; private set; }
        public IInstructorUserRepository InstructorUser { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
