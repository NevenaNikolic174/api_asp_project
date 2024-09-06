using Microsoft.AspNetCore.Mvc;
using MyProject.Application.DTO;
using MyProject.Application.DTO.SearchEntities;
using MyProject.Application.UseCases.Queries;
using MyProject.DataAccess;
using MyProject.Domain.Entities;
using System;
using System.Linq;

namespace MyProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly MyDBContext _dbCon;

        public AuditLogController(MyDBContext dbCon)
        {
            _dbCon = dbCon;
        }

        // GET: api/AuditLog
        [HttpGet]
        [AuthorizeRole(2)] // Pretpostavka da je ovo atribut za autorizaciju
        public IActionResult SearchAuditLogs([FromQuery] SearchAuditLogDTO searchDto)
        {
            var query = _dbCon.AuditLogs.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchDto.UserName))
            {
                query = query.Where(log => log.UserName.Contains(searchDto.UserName));
            }
            if (!string.IsNullOrWhiteSpace(searchDto.UseCaseName))
            {
                query = query.Where(log => log.UseCaseName.Contains(searchDto.UseCaseName));
            }
            if (searchDto.FromDate.HasValue)
            {
                query = query.Where(log => log.ExecutedAt >= searchDto.FromDate.Value);
            }
            if (searchDto.ToDate.HasValue)
            {
                query = query.Where(log => log.ExecutedAt <= searchDto.ToDate.Value);
            }

            var totalCount = query.Count();
            var page = searchDto.Page > 0 ? searchDto.Page : 1;
            var maxPage = (int)Math.Ceiling((double)totalCount / searchDto.PerPage);

            if (page > maxPage)
            {
                return NotFound(new { message = $"No data available for page {page}." });
            }

            var auditLogs = query
                .Skip((page - 1) * searchDto.PerPage)
                .Take(searchDto.PerPage)
                .ToList();

            var auditLogDtos = auditLogs.Select(log => new AuditLog
            {
                Id = log.Id,
                UserName = log.UserName,
                UseCaseName = log.UseCaseName,
                ExecutedAt = log.ExecutedAt,
            }).ToList();

            var response = new PageSearchResponse<AuditLog>
            {
                TotalCount = totalCount,
                CurrentPage = page,
                ItemsPerPage = searchDto.PerPage,
                Items = auditLogDtos
            };

            return Ok(response);
        }
    }
}
