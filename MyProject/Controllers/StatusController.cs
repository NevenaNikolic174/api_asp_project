using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MyProject.Api.Core;
using MyProject.Api.DTO;
using MyProject.Api.Extensions;
using MyProject.Api.Handlers.StatusHandler;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using MyProject.Implementation.Validators;
using System;
using System.Data;
using System.Linq;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly GetAllStatusesHandler _getStatus;
        private readonly GetStatusByIdHandler _getStatusById;
        private readonly CreateStatusHandler _createStatus;
        private readonly UpdateStatusHandler _updateStatus;
        private readonly DeleteStatusHandler _deleteStatus;

        public StatusController(GetAllStatusesHandler getStatus,
                                GetStatusByIdHandler getStatusById,
                                CreateStatusHandler createStatus,
                                UpdateStatusHandler updateStatus,
                                DeleteStatusHandler deleteStatus)
        {
            _getStatus = getStatus;
            _getStatusById = getStatusById;
            _createStatus = createStatus;
            _updateStatus = updateStatus;
            _deleteStatus = deleteStatus;
        }

        [HttpGet]
        [AuthorizeRole(2)]
        public IActionResult Get([FromQuery] SearchByValueDTO search)
        {
            return _getStatus.Handle(search);
        }

        [HttpGet("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Get(int id)
        {
            return _getStatusById.Handle(id);
        }

        [HttpPost]
        [AuthorizeRole(2)]
        public IActionResult Post([FromBody] StatusDTO dto)
        {
            return _createStatus.Handle(dto);
        }

        [HttpPut("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Put(int id, [FromBody] StatusDTO dto)
        {
            return _updateStatus.Handle(id, dto);
        }

        [HttpDelete("{id}")]
        [AuthorizeRole(2)]
        public IActionResult Delete(int id)
        {
            return _deleteStatus.Handle(id);
        }
    }
}
