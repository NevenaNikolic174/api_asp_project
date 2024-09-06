using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.PaymentHandler
{
    public class GetPaymentByIdHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetPaymentByIdHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(int id)
        {
            var payment = _dbCon.Payments
                .Where(p => p.Id == id)
                .Select(c => _mapper.Map<PaymentDTO>(c))
                .FirstOrDefault();

            if (payment == null)
            {
                return new NotFoundObjectResult("Payment not found.");
            }

            return new OkObjectResult(payment);
        }
    }
}
