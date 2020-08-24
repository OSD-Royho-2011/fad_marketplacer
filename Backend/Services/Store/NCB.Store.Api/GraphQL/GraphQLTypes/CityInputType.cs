using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.GraphQL.GraphQLTypes
{
    public class CityInputType : InputObjectGraphType
    {
        public CityInputType()
        {
            Name = "CityInput";
            Field<NonNullGraphType<StringGraphType>>("name");
            Field<NonNullGraphType<StringGraphType>>("zipcode");
        }
    }
}
