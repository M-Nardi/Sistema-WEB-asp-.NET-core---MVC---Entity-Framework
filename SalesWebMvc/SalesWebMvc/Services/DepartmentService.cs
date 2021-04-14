using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class DepartmentService
    {
        private SalesWebMvcContext _context;

        public DepartmentService (SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Department>> FindAllAsync() //implementacao assincrona task
        {
            return await _context.Department.OrderBy(x => x.Name).ToListAsync();
        }


    }
}
