using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using NCB.Core.DataAccess.BaseRepository;
using NCB.Store.Api.DataAccess.BaseRepository;
using NCB.Store.Api.DataAccess.BaseUnitOfWork;
using NCB.Store.Api.DataAccess.Entities;
using NCB.Store.Api.GraphQL.GraphQLTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.GraphQL.GraphQLMutation
{
    public class AppMutation : ObjectGraphType
    {
        public AppMutation(IBaseRepository<City> cityRepository, IUnitOfWork unitOfWork)
        {
            FieldAsync<CityType>(
               "createCity",
               arguments: new QueryArguments(
                   new QueryArgument<NonNullGraphType<CityInputType>> { Name = "city" }),
               resolve: async context =>
               {
                   var city = context.GetArgument<City>("city");
                   cityRepository.InsertAsync(city);
                   await unitOfWork.SaveAsync();
                   return await cityRepository.GetAll().Where(x => !x.RecordDeleted && x.Id == city.Id).FirstOrDefaultAsync();
               });
        }
    }
}
