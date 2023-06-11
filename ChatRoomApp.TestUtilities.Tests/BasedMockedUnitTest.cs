using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace ChatRoomApp.TestUtilities.Tests
{
    public abstract class BasedMockedUnitTest
    {
        public Mock<DbSet<TEntity>> CreateMockDbSet<TEntity>(IQueryable<TEntity> data) where TEntity : class
        {
            var asyncEnumerable = data;//.AsQueryable();
            var mockDbSet = new Mock<DbSet<TEntity>>();
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<TEntity>(asyncEnumerable.Provider));
            mockDbSet.As<IQueryable>().Setup(m => m.Expression).Returns(asyncEnumerable.Expression);
            mockDbSet.As<IQueryable>().Setup(m => m.ElementType).Returns(asyncEnumerable.ElementType);
            mockDbSet.As<IQueryable>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            return mockDbSet;
        }
    }
}