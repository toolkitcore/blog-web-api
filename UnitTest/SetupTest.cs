﻿using Api.ApplicationLogic.Interface;
using Api.ApplicationLogic.Mapper;
using Api.Infrastructure;
using Api.Infrastructure.Interface;
using Api.Infrastructure.Persistence;
using AutoFixture;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace UnitTest
{
    public class SetupTest : IDisposable
    {
        protected readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<IBookReadService> _bookReadServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;
        protected readonly Mock<ICacheService> _cacheServiceMock;
        protected readonly Mock<IBookRepository> _bookRepositoryMock;
        protected readonly ApplicationDbContext _dbContext;
        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _bookReadServiceMock = new Mock<IBookReadService>();
            _currentTimeMock = new Mock<ICurrentTime>();
            _cacheServiceMock = new Mock<ICacheService>();
            _bookRepositoryMock = new Mock<IBookRepository>();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
        }
        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
