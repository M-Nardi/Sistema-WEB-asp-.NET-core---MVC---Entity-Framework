using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

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

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create() //metodo que abre formulario para cadastro do vendedor.
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel { Departments = departments };
            return View(viewModel);
        }

        [HttpPost] //declarando q o método é post
        [ValidateAntiForgeryToken] //previne ataques CSRF - aproveitamento de seção para enviar dados maliciosos
        public async Task<IActionResult> Create(Seller seller)
        {
            if (!ModelState.IsValid) //verifica se ao editar os campos foram mandados conforme regras
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
                
                return View(viewModel);
            }

            await _sellerService.InsertAsync(seller); //inserir no BD
            return RedirectToAction(nameof(Index));//redirecionar a ação para o metodo index do SellersController (mesma classe)
        }


        public async Task<IActionResult> Delete(int? id)//este GET apenas confirmará a deleção. A deleção será feita no método POST - deleção vendedor
        {//recebera como parametro int opcional
            
            if (id == null) //requisição foi feita corretamente?
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value); //colocar .value pois o id do parametro é opcional "nullable"
            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _sellerService.RemoveAsync(id);
                return RedirectToAction(nameof(Index));
            }

            catch (IntegrityException e) //deleção de vendedor com vendas, chave estrangeira, captura exception 
            {
                return RedirectToAction(nameof(Error), new { message = e.Message});
            }
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) //requisição foi feita corretamente?
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) //existe o id é igual a nulo? ele foi informado?
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);

            if (obj == null) //existe no DB este vendedor deste id?
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync(); //carregar os departamentos para povoar a caixa de seleção da view
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments }; //como será uma tela de edição, preencheremos ja os campos dados do obj. Passaremos o departments também.
            return View(viewModel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {

            if (!ModelState.IsValid) //verifica se ao editar os campos foram mandados conforme regras
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };

                return View(viewModel);
            }

            if (id != seller.Id) //verifica a integridade dos dados do id passado e do vendedor
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" }); //badrequest
            }

            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }

            catch (ApplicationException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }

        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
            Message = message,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //pegar id interno da requisição
            };

            return View(viewModel);

        }

    }
}
