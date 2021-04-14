using System;


namespace SalesWebMvc.Services.Exceptions
{
    public class NotFoundException : ApplicationException //herdar app exception
    {
        public NotFoundException (string message) : base(message)
        {
        }
    }
}
