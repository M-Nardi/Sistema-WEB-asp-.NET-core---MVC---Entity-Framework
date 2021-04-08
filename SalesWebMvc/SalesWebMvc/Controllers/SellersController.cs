using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {

        private readonly SellerService _sellerService;

        
        public SellersController(SellerService sellerService)
        {
            _sellerService = sellerService;
        }
        
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost] //declarando q o método é post
        [ValidateAntiForgeryToken] //previne ataques CSRF - aproveitamento de seção para enviar dados maliciosos
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller); //redirecionar a ação para o metodo index do SellersController (mesma classe)
            return RedirectToAction(nameof(Index));
        }

    }
}
