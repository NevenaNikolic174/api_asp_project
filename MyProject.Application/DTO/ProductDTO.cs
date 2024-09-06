using Microsoft.AspNetCore.Http;
using MyProject.Application.UseCases.Queries;

namespace MyProject.Api.DTO
{
    public class ProductDTO : PageSearch
    {
        public string Name { get; set; }
        public int ProductCode { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public int? Year { get; set; }
        public IFormFile? ImageFile { get; set; }
        public int CategoryId { get; set; }
        public int ProducerId { get; set; }
        public int StatusId { get; set; }

        

    }

}
