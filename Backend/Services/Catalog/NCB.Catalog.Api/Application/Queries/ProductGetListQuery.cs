using MediatR;
using NCB.Catalog.Api.DataAccess;
using NCB.Catalog.Api.DataAccess.Entities;
using NCB.Catalog.Api.Services.Products.Models;
using NCB.Core.DataAccess.BaseRepository;
using NCB.Core.Models;
using NCB.Core.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Catalog.Api.Application.Queries
{
    public class ProductGetListQuery : BaseRequestListViewModel, IRequest<PagedList<ProductViewModel>>, IValidatableObject
    {
        //[CustomIntValidation(Value = 3)]
        //public int CustomValue { get; set; }

        public ProductGetListQuery() : base()
        {
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (Page != null && Page < 1)
            //    yield return new ValidationResult("Page must be greater than 1");
            yield return null;
        }
    }
}
