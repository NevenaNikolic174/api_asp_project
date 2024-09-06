using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MyProject.Api.DTO;
using MyProject.Domain.Entities;
using MyProject.DataAccess;
using MyProject.Implementation.Validators;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyProject.Api.Handlers.ProductOperations
{
    public class CreateProductHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;
        private readonly CreateProductValidator _validator;
        private readonly ILogger<CreateProductHandler> _logger;
        private readonly IWebHostEnvironment _env;

        public CreateProductHandler(MyDBContext context,
                                    IMapper mapper,
                                    CreateProductValidator validator,
                                    ILogger<CreateProductHandler> logger,
                                    IWebHostEnvironment env)
        {
            _dbCon = context;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _env = env;
        }

        public async Task<IActionResult> Handle(ProductDTO dto)
        {
            var result = _validator.Validate(dto);
            if (!result.IsValid)
            {
                return new BadRequestObjectResult(result.Errors);
            }

            string imageName = null;
            try
            {
                if (dto.ImageFile != null && dto.ImageFile.Length > 0)
                {
                    var fileName = GenerateUniqueFileName(dto.ImageFile.FileName);
                    var filePath = Path.Combine(_env.WebRootPath, "images", fileName);

                    await SaveFileAsync(dto.ImageFile, filePath);
                    imageName = fileName;
                }

                var product = _mapper.Map<Product>(dto);
                product.Image = imageName;
                product.IsActive = true;

                _dbCon.Products.Add(product);
                await _dbCon.SaveChangesAsync();

                return new OkObjectResult("Product successfully created.");
            }
            catch (Exception ex)
            {
                var innerExceptionMessage = ex.InnerException?.Message ?? "No inner exception";
                _logger.LogError($"Error creating product: {ex.Message}. Inner exception: {innerExceptionMessage}");
                return new StatusCodeResult(500);
            }
        }

        private string GenerateUniqueFileName(string originalFileName)
        {
            var fileExtension = Path.GetExtension(originalFileName);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $"{Path.GetFileNameWithoutExtension(originalFileName)}_{timestamp}{fileExtension}";
        }

        private async Task SaveFileAsync(IFormFile file, string path)
        {
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }

}
