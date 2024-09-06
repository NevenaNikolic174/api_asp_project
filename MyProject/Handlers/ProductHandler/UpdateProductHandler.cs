using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.ProductOperations
{
    public class UpdateProductHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductHandler> _logger;
        private readonly AuditLogger _auditLogger;
        private readonly IApplicationActor _applicationActor;

        public UpdateProductHandler(MyDBContext context, 
                                    IMapper mapper, 
                                    ILogger<UpdateProductHandler> logger, 
                                    IApplicationActor applicationActor, 
                                    AuditLogger auditLogger)
        {
            _dbCon = context;
            _mapper = mapper;
            _logger = logger;
            _auditLogger = auditLogger;
            _applicationActor = applicationActor;
        }

        public IActionResult Handle(int id, ProductDTO dto)
        {
            try
            {
                var product = _dbCon.Products
                    .Include(p => p.Category)
                    .Include(p => p.Producer)
                    .Include(p => p.Status)
                    .FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    return new NotFoundObjectResult("Product not found.");
                }

                _mapper.Map(dto, product);
                product.ModifiedAt = DateTime.Now;
                _dbCon.SaveChanges();

                _auditLogger.LogAudit(_applicationActor.Username, "Update_Product", "Success");
                return new OkObjectResult("Product updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };

                _auditLogger.LogAudit(_applicationActor.Username, "Update_Product", "Failed");
                return new StatusCodeResult(500);
            }
        }
    }
}
