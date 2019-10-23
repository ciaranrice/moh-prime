using System;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Prime;
using PrimeTests.Utils;

namespace PrimeTests.Services
{
    public class BaseServiceTests<T> : IDisposable where T : class
    {
        protected string _databaseName;
        protected ApiDbContext _dbContext;
        protected HttpContextAccessor _httpContext;
        protected T _service;

        public BaseServiceTests()
        {
            _databaseName = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<ApiDbContext>()
                        .UseInMemoryDatabase(databaseName: _databaseName)
                      .Options;

            _httpContext = new HttpContextAccessor();
            _httpContext.HttpContext = new DefaultHttpContext();

            _dbContext = new ApiDbContext(options, _httpContext);

            TestUtils.InitializeDbForTests(_dbContext);

            _service = (T)Activator.CreateInstance(typeof(T), new object[] { _dbContext, _httpContext, });
        }

        public void Dispose()
        {
            TestUtils.DetachAllEntities(_dbContext);
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}