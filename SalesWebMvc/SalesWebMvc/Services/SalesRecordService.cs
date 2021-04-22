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
                from obj in _context.SalesRecord  select obj; //pegar salesRecord do tipo DBset para construir objeto em cima dele.

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

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
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
                .GroupBy(x => x.Seller.Department) //groupby por departamento
                .ToListAsync();
        }

        public async Task<List<SalesRecord>> FindByNameAsync(DateTime? minDate, DateTime? maxDate, int? id)
        {
            var result =
                from obj in _context.SalesRecord where obj.Seller.Id == id select obj; //pegar salesRecord do tipo DBset para construir objeto em cima dele.

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
