using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models.ViewModels
{
    public class SellerFormViewModel
    { //quais os atributos necessários para uma tela de cadastro de vendedor? importante para passar as informações

        public Seller Seller { get; set; }
        public ICollection<Department> Departments { get; set; } //importante colocar em plural para a conversao dos dados http para objeto


    }
}
