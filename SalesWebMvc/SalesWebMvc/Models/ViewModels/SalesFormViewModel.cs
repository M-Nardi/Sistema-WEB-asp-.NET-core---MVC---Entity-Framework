using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesWebMvc.Models.ViewModels
{
    public class SalesFormViewModel
    {
        public ICollection<Seller> Sellers { get; set; }
       
    }
}
