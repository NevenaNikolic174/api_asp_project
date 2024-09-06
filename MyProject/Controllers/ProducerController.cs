using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.DTO;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using AutoMapper.QueryableExtensions;
using MyProject.Implementation.Validators;
using MyProject.Api.Extensions;
using MyProject.Application.UseCases.Queries;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Api.Core;
using MyProject.Application.DTO;
using MyProject.Api.Handlers.CategoryHandler;
using MyProject.Api.Handlers.PaymentHandler;
using MyProject.Api.Handlers.ProducerHandler;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProducerController : ControllerBase
    {
        private readonly GetAllProducersHandler _getAllProducers;
        private readonly GetProducerByIdHandler _getProducerById;
        private readonly CreateProducerHandler _createProducer;
        private readonly UpdateProducerHandler _updateProducer;
        private readonly DeleteProducerHandler _deleteProducer;

        public ProducerController(GetAllProducersHandler getAllProducers,
                                  GetProducerByIdHandler getProducerById,
                                  CreateProducerHandler createProducer,
                                  UpdateProducerHandler updateProducer,
                                  DeleteProducerHandler deleteProducer)
        {
            _getAllProducers = getAllProducers;
            _getProducerById = getProducerById;
            _createProducer = createProducer;
            _updateProducer = updateProducer;
            _deleteProducer = deleteProducer;
        }

        [HttpGet]
        [AuthorizeRole(2)]
        public IActionResult Get([FromQuery] SearchByNameDTO search)
        {
            return _getAllProducers.Handle(search);
        }

        [HttpGet("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Get(int id)
        {
            return _getProducerById.Handle(id);
        }

        [HttpPost]
        [AuthorizeRole(2)]
        public IActionResult Post([FromBody] ProducerDTO dto)
        {
            return _createProducer.Handle(dto);
        }

        [HttpPut("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Put(int id, [FromBody] ProducerDTO dto)
        {
            return _updateProducer.Handle(id, dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Delete(int id)
        {
            return _deleteProducer.Handle(id);
        }
    }
 }
