using GraphQL.Types;
using NCB.Store.Api.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;

namespace NCB.Store.Api.GraphQL.GraphQLTypes
{
    public class CityType : ObjectGraphType<City>
    {
        public CityType()
        {
            Field(x => x.Id, type: typeof(StringGraphType)).Description("Id property from the store object.");
            Field(x => x.Name).Description("Name property from the store object.");
            Field(x => x.Zipcode, type: typeof(IntGraphType)).Description("Zipcode property from the store object.");
        }
    }
}
