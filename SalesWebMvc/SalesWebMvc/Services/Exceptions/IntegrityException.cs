using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Services.Exceptions
{
    public class IntegrityException : ApplicationException //erros de serviço de integridade referencial. apagar vendedores que possuam vendas.
    {
        public IntegrityException (string message) : base(message) //repassar a chamada para a superclasse
        { }



    }
}
