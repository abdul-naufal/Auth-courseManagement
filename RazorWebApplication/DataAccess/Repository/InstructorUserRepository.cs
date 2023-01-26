using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class InstructorUserRepository : Repository<InstructorUser>, IInstructorUserRepository
    {
        private readonly ApplicationDbContext _db;

        public InstructorUserRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }
        
    }
}
