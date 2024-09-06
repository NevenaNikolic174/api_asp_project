using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.Application.DTO;
using MyProject.DataAccess;

namespace MyProject.Api.Handlers.ProducerHandler
{
    public class GetProducerByIdHandler
    {
        private readonly MyDBContext _dbCon;
        private readonly IMapper _mapper;

        public GetProducerByIdHandler(MyDBContext context, IMapper mapper)
        {
            _dbCon = context;
            _mapper = mapper;
        }

        public IActionResult Handle(int id)
        {
            var producer = _dbCon.Producers
                .Where(p => p.Id == id)
                .Select(c => _mapper.Map<ProducerDTO>(c))
                .FirstOrDefault();

            if (producer == null)
            {
                return new NotFoundObjectResult("Producer not found.");
            }

            return new OkObjectResult(producer);
        }
    }
}
