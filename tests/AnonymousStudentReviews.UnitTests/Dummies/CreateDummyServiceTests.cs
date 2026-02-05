using AnonymousStudentReviews.Core.Abstractions;
using AnonymousStudentReviews.Core.Aggregates.Dummy;
using AnonymousStudentReviews.UseCases.Abstractions;
using AnonymousStudentReviews.UseCases.Dummies.Create;
using AnonymousStudentReviews.UseCases.Registration.Abstractions;

using Moq;

namespace AnonymousStudentReviews.UnitTests.Dummies;

public class CreateDummyServiceTests
{
    private readonly CreateDummyService _createDummyService;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly Mock<IDummyRepository> _dummyRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserManager> _userManagerMock;

    public CreateDummyServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _dummyRepositoryMock = new Mock<IDummyRepository>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _userManagerMock = new Mock<IUserManager>();

        _createDummyService = new CreateDummyService(_dummyRepositoryMock.Object, _unitOfWorkMock.Object,
            _currentUserServiceMock.Object, _userManagerMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldSuccess_WhenDummyName_IsNotNullOrWhitespace()
    {
        var dummyName = "some valid name";
        var dto = new CreateDummyDto { Name = dummyName };

        var result = await _createDummyService.ExecuteAsync(dto);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ExecuteAsync_ShouldFail_WhenDummyName_IsNullOrWhitespace()
    {
        var dummyName = "";
        var dto = new CreateDummyDto { Name = dummyName };

        var result = await _createDummyService.ExecuteAsync(dto);

        Assert.False(result.IsSuccess);
    }
}
