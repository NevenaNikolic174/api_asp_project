using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Api.Handlers.PaymentHandler;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;
using System.Linq;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly GetAllPaymentsHandler _getAllPayments;
        private readonly GetPaymentByIdHandler _getPaymentById;
        private readonly CreatePaymentHandler _createPayment;
        private readonly UpdatePaymentHandler _updatePayment;
        private readonly DeletePaymentHandler _deletePayment;

        public PaymentController(GetAllPaymentsHandler getAllPayments,
                                  GetPaymentByIdHandler getPaymentById,
                                  CreatePaymentHandler createPayment,
                                  UpdatePaymentHandler updatePayment,
                                  DeletePaymentHandler deletePayment)
        {
            _getAllPayments = getAllPayments;
            _getPaymentById = getPaymentById;
            _createPayment = createPayment;
            _updatePayment = updatePayment;
            _deletePayment = deletePayment;
        }

        // GET: api/payment
        // search: api/payment?method=
        [HttpGet]
        [AuthorizeRole(2)]
        public IActionResult Get([FromQuery] SearchByPaymentMethodDTO search)
        {
            return _getAllPayments.Handle(search);
        }

        [HttpGet("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Get(int id)
        {
            return _getPaymentById.Handle(id);
        }

        [HttpPost]
        [AuthorizeRole(2)]
        public IActionResult Post([FromBody] PaymentDTO dto)
        {
            return _createPayment.Handle(dto);
        }

        [HttpPut("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Put(int id, [FromBody] PaymentDTO dto)
        {
            return _updatePayment.Handle(id, dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Delete(int id)
        {
            return _deletePayment.Handle(id);
        }
    }
}
