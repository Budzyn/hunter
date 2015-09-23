using System;
using System.Collections.Generic;
using System.Linq;
using Hunter.Common.Interfaces;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;
using NSubstitute;
using NUnit.Framework;
using Hunter.Services;
using Hunter.DataAccess.Entities;
using Hunter.Services.Interfaces;

namespace Hunter.Tests.Services
{
    class PoolServiceTests
    {

        private readonly IPoolRepository _repository;
        private readonly IPoolService _service;

        public PoolServiceTests()
        {
            _repository = Substitute.For<IPoolRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var logger = Substitute.For<ILogger>();
            var activityHelperService = Substitute.For<IActivityHelperService>();

            _service = new PoolService(_repository, unitOfWork, logger, activityHelperService);
        }

        [SetUp]
        public void TestSetup()
        {
            _repository.Get(Arg.Any<int>()).Returns(new Pool { Id = 1, Name = "Pool name" });
            _repository.Query().Returns(new List<Pool>()
            {
                new Pool(){Id = 1, Name = "Pool name #1"},
                new Pool(){Id = 2, Name = "Pool name #2"},
                new Pool(){Id = 3, Name = "Pool name #3"}
            }.AsQueryable());
            _repository.Query().Returns(new List<Pool>()
            {
                new Pool(){Id = 1, Name = "Pool name #1"},
                new Pool(){Id = 2, Name = "Pool name #2"},
                new Pool(){Id = 3, Name = "Pool name #1"}
            }.AsQueryable());
        }

        [Test]
        public void Pool_Should_correctly_returns_When_pool_is_getted_by_id()
        {
            // Arrange

            // Act
            var result = _service.GetPoolById(1);

            // Assert
            Assert.AreEqual("Pool name", result.Name);
        }

        [Test]
        public void Pools_Should_correctly_return_When_all_pools_are_getted()
        {
            // Arrange

            // Act
            var results = _service.GetAllPools();

            // Assert
            Assert.AreEqual(3, results.Count());
        }

        [Test]
        public void Should_correctly_add_new_pool_When_pool_is_posted()
        {
            // Arrange
            var counter = 0;

            // Act
            _repository.When(r => r.UpdateAndCommit(Arg.Any<Pool>()))
                .Do(r => counter++);
            _repository.When(r => r.UpdateAndCommit(Arg.Any<Pool>()))
                .Do(r => { throw new Exception(); });

            _service.CreatePool(new PoolViewModel() { Id = 1, Name = "Pool name" });

            // Assert
            Assert.AreEqual(1, counter);
            
            // TODO we should not check repository logic here, only service
            Assert.DoesNotThrow(()=>_repository.UpdateAndCommit(Arg.Any<Pool>()));
        }

        [Test]
        public void Should_correctly_edit_pool_When_pool_is_putted()
        {
            // Arrange
            var counter = 0;

            // Act
            _repository.When(r => r.Update(Arg.Any<Pool>()))
                .Do(r => counter++);

            _service.UpdatePool(new PoolViewModel() { Id = 1, Name = "Pool name" });

            // Assert
            Assert.AreEqual(1, counter);
            Assert.DoesNotThrow(() => _repository.Update(Arg.Any<Pool>()));
        }

        [Test]
        public void Should_correctly_delete_pool_When_pool_is_deleted()
        {
            // Arrange
            var counter = 0;

            // Act
            _repository.When(r => r.Delete(Arg.Any<Pool>()))
                .Do(r => counter++);

            _service.DeletePool(1);

            // Assert
            Assert.AreEqual(1, counter);
            Assert.DoesNotThrow(() => _repository.Delete(Arg.Any<Pool>()));
        }

        [Test]
        public void Should_return_true_When_pool_name_exists()
        {
            // Arrange

            // Act
            var result = _service.IsPoolNameExist("Pool name #1");
            
            // Assert
            Assert.True(result);
        }

        [Test]
        public void Should_return_true_When_pool_exists()
        {
            // Arrange

            // Act
            var result = _service.IsPoolExist(1);

            // Assert
            Assert.True(result);
        }

        [Test]
        public void Should_return_true_When_another_pool_with_same_name_exists()
        {
            // Arrange

            // Act
            var result = _service.IsPoolNameExistExceptCurrentPool(new PoolViewModel() { Id = 1, Name = "Pool name #1" });

            // Assert
            Assert.True(result);
        }

    }
}
