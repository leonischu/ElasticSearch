using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobPortal.Application.Interfaces
{
    public interface IElasticService<T>
    {
        Task IndexAsync(T entity);

        Task<IEnumerable<T>> SearchAsync(string Keyword);
    }
}
