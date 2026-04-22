using JobPortal.Application.Interfaces;
using JobPortal.Application.Query;
using JobPortal.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Handlers
{
    public class SearchJobHandler
    {
        private readonly IElasticService<Job> _elastic;
        public SearchJobHandler(IElasticService<Job> elastic)
        {
            _elastic = elastic;
        }
        public async Task<IEnumerable<Job>> Handle(SearchJobQuery query)
        {
            return await _elastic.SearchAsync(query.Keyword);
        }
    }
}
