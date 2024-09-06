using MyProject.Application.UseCases.Queries;
using System.Collections.Generic;

namespace MyProject.Application.DTO
{
    public class OrderDTO : PageSearch
    {
      
        public int PaymentId { get; set; }
        public long? CreditCardNumber { get; set; } 
        public string UserAddress { get; set; }
        public double Amount { get; set; }

        public IEnumerable<OrderProductDTO> Products { get; set; }
    }

    public class OrderProductDTO
    {
        public int ProductId { get; set; }
        public int OrderId { get; set; }

    }
   
}
