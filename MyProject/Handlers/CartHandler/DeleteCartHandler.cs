using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProject.Api.Core;
using MyProject.Application.Actor;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.CartHandler
{
    public class DeleteCartHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly ILogger<DeleteCartHandler> _logger;
        private readonly IApplicationActor _applicationActor;
        private readonly AuditLogger _auditLogger;

        public DeleteCartHandler(MyDBContext context, 
                                 ILogger<DeleteCartHandler> logger, 
                                 IApplicationActor applicationActor, 
                                 AuditLogger auditLogger)
        {
            _dbCon = context;
            _logger = logger;
            _applicationActor = applicationActor;
            _auditLogger = auditLogger;
        }

        public IActionResult Handle(int id)
        {
            try
            {
                var cartItem = _dbCon.Carts
                .FirstOrDefault(c => c.Id == id && c.UserId == _applicationActor.Id);

                if (cartItem == null)
                {
                    return new NotFoundObjectResult("Cart item not found.");
                }

                cartItem.IsDeleted = true;
                cartItem.IsActive = false;
                cartItem.DeletedAt = DateTime.Now;

                _dbCon.SaveChanges();

                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Cart", "Success");
                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: \n{ex}\n");
                var error = new ServerError { Error500 = ex.Message };

                _auditLogger.LogAudit(_applicationActor.Username, "Delete_Cart", "Success");
                return new StatusCodeResult(500);
            }
        }
    }
}
