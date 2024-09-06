using MyProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Application.UseCases.Queries
{
    public abstract class PageSearch : MyEntity
    {
        public int PerPage { get; set; } = 3;
        public int Page { get; set; } = 1;

        public int CalculatePage(int totalCount)
        {
            return Math.Max(Page, 1);
        }

        public int CalculateItemsPerPage()
        {
            return Math.Max(PerPage, 3);
        }

        public int CalculateMaxPage(int totalCount)
        {
            int perPage = CalculateItemsPerPage();
            return (totalCount + perPage - 1) / perPage;
        }
    }
}
