using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace NCB.Core.Models
{
    public class BaseRequestListViewModel
    {
        public BaseRequestListViewModel()
        {
            IsDesc = false;
        }

        public string Query { get; set; }

        public string SortName { get; set; }

        public bool IsDesc { get; set; }

        public int? Page { get; set; }

        public int? Size { get; set; }
    }
}
