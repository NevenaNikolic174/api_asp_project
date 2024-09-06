using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.RestaurantHandler
{
    public class GetRestaurantByIdHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetRestaurantByIdHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(int id)
        {
            var restaurant = _dbCon.Restaurants
                .Where(p => p.Id == id)
                .Select(c => _mapper.Map<RestaurantDTO>(c))
                .FirstOrDefault();

            if (restaurant == null)
            {
                return new NotFoundObjectResult("Restaurant not found.");
            }

            return new OkObjectResult(restaurant);
        }
    }
}
