using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using NCB.Core.DataAccess.BaseRepository;
using NCB.Store.Api.DataAccess.BaseRepository;
using NCB.Store.Api.DataAccess.Entities;
using NCB.Store.Api.GraphQL.GraphQLTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.GraphQL.GraphQLQuery
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(IBaseRepository<City> cityRepository)
        {
            FieldAsync<ListGraphType<CityType>>(
                "cities",
                resolve: async context => await cityRepository.GetAll().Where(x => !x.RecordDeleted).ToListAsync()
                );
        }
    }
}
