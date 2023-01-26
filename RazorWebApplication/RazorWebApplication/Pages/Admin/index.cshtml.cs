using DataAccess.Repository.IRepository;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace RazorWebApplication.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IEnumerable<InstructorUser> Instructors { get; set; }

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));


        public async Task OnGetAsync()
        {
            Instructors = await GetPaginatedResult(CurrentPage, PageSize);
            Count = await GetCount();
        }

        private async Task<IEnumerable<InstructorUser>> GetData()
        {
            return _unitOfWork.InstructorUser.GetAll();

        }

        public async Task<IEnumerable<InstructorUser>> GetPaginatedResult(int currentPage, int pageSize = 10)
        {
            var data = await GetData();
            return data.OrderBy(d => d.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<int> GetCount()
        {
            var data = await GetData();
            return data.Count();
        }


    }
}
