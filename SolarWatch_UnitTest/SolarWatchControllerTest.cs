using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SolarWatch.Contracts;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Services;

namespace SolarWatch_UnitTest;

public class SolarWatchControllerTest
{
    private Mock<ILogger<SolarWatchController>> _loggerMock;
    private SolarWatchController _controller;
    private Mock<ISolarWatchService> _solarWatchServiceMock;
    private Mock<ICityService> _cityServiceMock;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SolarWatchController>>();
        _solarWatchServiceMock = new Mock<ISolarWatchService>();
        _cityServiceMock = new Mock<ICityService>();
        _controller = new SolarWatchController(
            _loggerMock.Object,
            _solarWatchServiceMock.Object,
            _cityServiceMock.Object);
    }

    [Test]
    public async Task Test_GetSunriseAndSunset_ReturnsNotFoundResultIfSolarWatchDataProviderFails()
    {
        // Arrange
        _solarWatchServiceMock.Setup(x => x.GetSolarWatch(It.IsAny<City>())).Throws(new Exception());

        // Act
        var result = await _controller.GetSunriseAndSunset(new SolarWatchRequest("London"));

        // Assert
        Assert.IsInstanceOf<ActionResult>(result);
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }

    [Test]
    public async Task Test_GetSolarWatches_ReturnsNotFoundIfNoSolarWatches()
    {
        // Arrange
        _solarWatchServiceMock.Setup(x => x.GetAllSolarwatches())
            .ReturnsAsync(Enumerable.Empty<SolarWatch.Models.SolarWatch>());

        //Act 
        var result = await _controller.GetSolarWatches();

        //Assert
        Assert.IsInstanceOf<ActionResult>(result);
        Assert.IsInstanceOf<NotFoundObjectResult>(result);
    }
}