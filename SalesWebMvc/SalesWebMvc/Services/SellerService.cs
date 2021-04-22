using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;


namespace SalesWebMvc.Services
{
    public class SellerService
    {

        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task<Seller> FindByIdAsync(int id) //utilizados no controlador Sellers Delete/details
        {
           return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(p => p.Id == id); //injecao de departamento no seller include
        }

        public async Task RemoveAsync(int id) //usado no controlador Delete
        {
            try { 
            var obj = await _context.Seller.FindAsync(id); //pegar o obj pelo id
            _context.Seller.Remove(obj); //remoção do objeto no DBset
            await _context.SaveChangesAsync(); //salvar as alteracoes no entity framework 
            }

            catch (DbUpdateException) //exceção de remover um vendedor com vendas
            {
                throw new IntegrityException("Can't remove seller because he/she has sales"); //lançar exception em nivel de serviço
            }
        }

        public async Task InsertAsync(Seller obj) //usado no controlador Create
        {
            _context.Add(obj); //inserir dado no BD
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller obj) //usado no controlador Edit
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny) //existe no BD?
            {
                throw new NotFoundException("Id not found");
            } 

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync(); //poderá retornar uma excecao de conflito de concorrencia, utilizaremos um bloco try e um catch para capturar uma possivel excecao
            }
            catch (DbUpdateConcurrencyException e) //intercepcao de nivel de acesso a dados
            {
                throw new DbConcurrencyException(e.Message); //mensagem do banco de dados, relançando exceção em nivel de serviço
            }                                                //controlador irá lidar com exceções da camada de serviço. Exceções de repositórios será capturada pela Camada de serviço, e lançada a nivel de serviço.
        
        }

        public async Task<List<Seller>> FindAllNameAsync() //implementacao utilizada no salesrecord
        {
            return await _context.Seller.OrderBy(x => x.Name).ToListAsync();
        }



    }
}
