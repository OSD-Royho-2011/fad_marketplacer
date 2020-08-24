using GraphQL;
using GraphQL.Types;
using NCB.Store.Api.GraphQL.GraphQLMutation;
using NCB.Store.Api.GraphQL.GraphQLQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCB.Store.Api.GraphQL.GraphQLSchema
{
    public class AppSchema : Schema
    {

        // the IdependencyResolver which is going to help us resolve our Query, Mutation, or Subscription objects.
        public AppSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<AppQuery>();
            Mutation = resolver.Resolve<AppMutation>();
        }
    }
}
