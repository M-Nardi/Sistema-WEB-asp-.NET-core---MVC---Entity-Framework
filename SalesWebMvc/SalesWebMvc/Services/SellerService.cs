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

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public Seller FindById(int id) //utilizados no controlador Sellers Delete/details
        {
           return _context.Seller.Include(obj => obj.Department).FirstOrDefault(p => p.Id == id);
        }

        public void Remove(int id) //usado no controlador Delete
        {
            var obj = _context.Seller.Find(id); //pegar o obj pelo id
            _context.Seller.Remove(obj); //remoção do objeto no DBset
            _context.SaveChanges(); //salvar as alteracoes no entity framework 
        }

        public void Insert(Seller obj) //usado no controlador Create
        {
            _context.Add(obj); //inserir dado no BD
            _context.SaveChanges();
        }

        public void Update(Seller obj) //usado no controlador Edit
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id)) //existe no BD?
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                _context.SaveChanges(); //poderá retornar uma excecao de conflito de concorrencia, utilizaremos um bloco try e um catch para capturar uma possivel excecao
            }
            catch (DbUpdateConcurrencyException e) //intercepcao de nivel de acesso a dados
            {
                throw new DbConcurrencyException(e.Message); //mensagem do banco de dados, relançando exceção em nivel de serviço
            }                                                //controlador irá lidar com exceções da camada de serviço. Exceções de repositórios será capturada pela Camada de serviço, e lançada a nivel de serviço.
        
        }

    }
}
