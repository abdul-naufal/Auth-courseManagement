using DataAccess.Repository.IRepository;
using Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace RazorWebApplication.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;

        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));

        public int EndIndex { get; set; } = 0;

        public bool ShowPrevious => CurrentPage > 1;
        public bool ShowNext => CurrentPage < TotalPages;
        public bool ShowFirst => CurrentPage != 1;
        public bool ShowLast => CurrentPage != TotalPages;
        public IEnumerable<InstructorUser> Instructors { get; set; }

        public async Task OnGetAsync()
        {
            Instructors = await GetPaginatedResult(CurrentPage, PageSize);
            Count = await GetCount();

            if (Count < ((CurrentPage - 1) * PageSize + PageSize)) 
            {
                EndIndex = Count;
            }
            else
            {
                EndIndex = (CurrentPage - 1) * PageSize + PageSize;
            }
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
