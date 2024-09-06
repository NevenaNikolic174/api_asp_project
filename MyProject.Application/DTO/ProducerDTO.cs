using MyProject.Application.UseCases.Queries;

namespace MyProject.Api.DTO
{
    public class ProducerDTO : PageSearch
    {
        public string Name { get; set; }
        public string? Phone { get; set; }
    }

}
