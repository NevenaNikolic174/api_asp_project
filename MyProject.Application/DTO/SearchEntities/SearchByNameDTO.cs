using MyProject.Application.UseCases.Queries;

namespace MyProject.Application.DTO.SearchEntities
{
    public class SearchByNameDTO : PageSearch
    {
        public string Name { get; set; }
        public OrderByFunction? OrderByName { get; set; }

        public enum OrderByFunction
        {
            ASC,
            DESC
        }
    }
}
