using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.ProductOperations
{
    public class GetProductByIdHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetProductByIdHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(int id)
        {
            var product = _dbCon.Products
                .Where(p => p.Id == id)
                .Select(product => _mapper.Map<ProductDTO>(product))
                .FirstOrDefault();

            if (product == null)
            {
                return new NotFoundObjectResult("Product not found.");
            }

            return new OkObjectResult(product);
        }
    }
}
