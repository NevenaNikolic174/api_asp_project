using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.StatusHandler
{
    public class GetStatusByIdHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetStatusByIdHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(int id)
        {
            var status = _dbCon.Statuses
                .Where(p => p.Id == id)
                .Select(c => _mapper.Map<StatusDTO>(c))
                .FirstOrDefault();

            if (status == null)
            {
                return new NotFoundObjectResult("Status not found.");
            }

            return new OkObjectResult(status);
        }
    }
}
