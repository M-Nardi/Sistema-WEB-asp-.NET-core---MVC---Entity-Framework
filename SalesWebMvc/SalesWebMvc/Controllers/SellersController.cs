using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {

        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService; //nova dependencia, injetar tambem no construtor

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _departmentService = departmentService;
            _sellerService = sellerService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create() //metodo que abre formulario para cadastro do vendedor.
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //declarando q o método é post
        [ValidateAntiForgeryToken] //previne ataques CSRF - aproveitamento de seção para enviar dados maliciosos
        public IActionResult Create(Seller seller)
        {
            _sellerService.Insert(seller); //inserir no BD
            return RedirectToAction(nameof(Index));//redirecionar a ação para o metodo index do SellersController (mesma classe)
        }


        public IActionResult Delete(int? id)//este GET apenas confirmará a deleção. A deleção será feita no método POST - deleção vendedor
        {//recebera como parametro int opcional
            
            if (id == null) //requisição foi feita corretamente?
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value); //colocar .value pois o id do parametro é opcional "nullable"
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Details(int? id)
        {
            if (id == null) //requisição foi feita corretamente?
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) //existe o id é igual a nulo? ele foi informado?
            {
                return NotFound();
            }

            var obj = _sellerService.FindById(id.Value);

            if (obj == null) //existe no DB este vendedor deste id?
            {
                return NotFound();
            }

            List<Department> departments = _departmentService.FindAll(); //carregar os departamentos para povoar a caixa de seleção da view
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }; //como será uma tela de edição, preencheremos ja os campos dados do obj. Passaremos o departments também.
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (id != seller.Id) //verifica a integridade dos dados do id passado e do vendedor
            {
                return BadRequest();
            }

            try
            {
                _sellerService.Update(seller);
                return RedirectToAction(nameof(Index));
            }

            catch (NotFoundException)
            {
                return NotFound();
            }

            catch (DbConcurrencyException)
            {
                return BadRequest();
            }

        }

    }
}
