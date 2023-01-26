﻿using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class AdminUserRepository : Repository<AdminUser>, IAdminUserRepository
    {
        private readonly ApplicationDbContext _db;

        public AdminUserRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;
        }
        
    }
}
