using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Api.Handlers.ProductOperations;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GetAllProductsHandler _getProductHandler;
        private readonly GetProductByIdHandler _getProductByIdHandler;
        private readonly CreateProductHandler _createProductHandler;
        private readonly UpdateProductHandler _updateProductHandler;
        private readonly DeleteProductHandler _deleteProductHandler;

        public ProductController(GetAllProductsHandler getProductHandler,
                                 GetProductByIdHandler getProductByIdHandler,
                                 CreateProductHandler createProductHandler,
                                 UpdateProductHandler updateProductHandler,
                                 DeleteProductHandler deleteProductHandler)
        {
            _getProductHandler = getProductHandler;
            _getProductByIdHandler = getProductByIdHandler;
            _createProductHandler = createProductHandler;
            _updateProductHandler = updateProductHandler;
            _deleteProductHandler = deleteProductHandler;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] SearchProductsDTO search)
        {
            return _getProductHandler.Handle(search);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return _getProductByIdHandler.Handle(id);
        }

        [HttpPost]
        [AuthorizeRole(2)]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post([FromForm] ProductDTO dto)
        {
            return await _createProductHandler.Handle(dto);
        }

        [HttpPut("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Put(int id, [FromBody] ProductDTO dto)
        {
            return _updateProductHandler.Handle(id, dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Delete(int id)
        {
            return _deleteProductHandler.Handle(id);
        }
    }
}
