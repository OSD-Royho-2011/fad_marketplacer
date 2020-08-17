using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.Api.UnitTests
{
    public class AsyncEnumerableQuery<T> : EnumerableQuery<T>, IAsyncEnumerable<T>
    {
        public AsyncEnumerableQuery(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        public AsyncEnumerableQuery(Expression expression) : base(expression)
        {
        }

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new InMemoryDbAsyncEnumerator<T>(((IEnumerable<T>)this).GetEnumerator());
        }

    }
    public class InMemoryDbAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public InMemoryDbAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask();
        }

        public T Current => _enumerator.Current;
    }
}
