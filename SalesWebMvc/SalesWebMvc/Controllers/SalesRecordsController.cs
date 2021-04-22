using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;
using SalesWebMvc.Models;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {

        private readonly SalesRecordService _salesRecordService; //dependencia dos servicos
        private readonly SellerService _sellerService;


        public SalesRecordsController(SalesRecordService salesRecordService, SellerService sellerService)
        {
            _salesRecordService = salesRecordService;
            _sellerService = sellerService;
        }


        public async Task<IActionResult> Index()
        {
            List<Seller> sellers = await _sellerService.FindAllAsync();


            SalesFormViewModel viewModel = new SalesFormViewModel { Sellers = sellers};

            return View(viewModel);
        }

        public async Task<IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {

            if (!minDate.HasValue) //Se o label nao for preenchido, pegará o ano atual, e dia/mes 1 do ano
            {
                minDate = new DateTime(1998, 1, 1);
            }

            if (!maxDate.HasValue)
            {
                maxDate = DateTime.Now; //Se o label nao for preenchido, pegara a data atual
            }
            //deixar o campo selecionado após inserção da data
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); //passaremos os dados para a view desta forma com o dicionario viewData
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd"); //para incluir na view, adicionar no input value=@ViewData["minDate"]

            var result = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(result);

        }

        public async Task<IActionResult> GroupingSearch(DateTime? minDate, DateTime? maxDate)
        {

            if (!minDate.HasValue) //Se o label nao for preenchido, pegará o ano atual, e dia/mes 1 do ano
            {
                minDate = new DateTime(1998, 1, 1);
            }

            if (!maxDate.HasValue) //só funciona com o ? no argumento
            {
                maxDate = DateTime.Now; //Se o label nao for preenchido, pegara a data atual
            }
            //deixar o campo selecionado após inserção da data
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd"); //passaremos os dados para a view desta forma com o dicionario viewData
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd"); //para incluir na view, adicionar no input value=@ViewData["minDate"]

            var result = await _salesRecordService.FindByDateGroupingAsync(minDate, maxDate);
            return View(result);
        }
    

    public async Task<IActionResult> NameSearch(int? id)
    {
       var minDate = new DateTime(1998, 1, 1);
       var maxDate = DateTime.Now;

       var result = await _salesRecordService.FindByNameAsync(minDate,maxDate,id);
       return View(result);
    }

    }
}
