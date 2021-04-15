using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context;

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result =
                from obj in _context.SalesRecord select obj; //pegar salesRecord do tipo DBset para construir objeto em cima dele.

            if (minDate.HasValue)
            {
                result = result.Where(p => p.Date >= minDate.Value); //trabalhará em cima do result
            }

            if (maxDate.HasValue)
            {
                result = result.Where(p => p.Date <= maxDate.Value);
            }

            return await result
                .Include(x => x.Seller)  //include - join com os sellers e departamentos
                .Include(x => x.Seller.Department)
                .OrderByDescending(x => x.Date) // ordenar resultado por data decrescente
                .ToListAsync();
        }

    }

}
