using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public string Message { get; set; } //acrescentar mensagem customizada

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); //id interno que poderemos mostrar na pag de erro


    }
}