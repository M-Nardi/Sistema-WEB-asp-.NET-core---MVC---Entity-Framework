﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {

        private readonly SalesRecordService _salesRecordService; //dependencia dos servicos
        
        public SalesRecordsController (SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }
        
        
        public IActionResult Index()
        {
            return View();
        }

        public async Task <IActionResult> SimpleSearch(DateTime? minDate, DateTime? maxDate)
        {
            
            if (!minDate.HasValue) //Se o label nao for preenchido, pegará o ano atual, e dia/mes 1 do ano
            {
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
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

        public IActionResult GroupingSearch()
        {
            return View();
        }



    }
}
