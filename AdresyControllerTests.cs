using Xunit;
using Moq;
using TOKYO_GARDEN.Controllers;
using IBL;
using Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using FluentAssertions;

public class AdresyControllerTests
{
    private readonly Mock<IObslugaAdresow> _mockObslugaAdresow;
    private readonly AdresyController _controller;

    public AdresyControllerTests()
    {
        _mockObslugaAdresow = new Mock<IObslugaAdresow>();
        _controller = new AdresyController(_mockObslugaAdresow.Object);
    }

    [Fact]
    public void GetSortedAdresy_ShouldReturnOkWithAdresyList()
    {
        var expectedAdresy = new List<Adresy>
        {
            new Adresy { },
            new Adresy { }
        };
        _mockObslugaAdresow.Setup(s => s.PobierzPosortowaneAdresy()).Returns(expectedAdresy);

        var result = _controller.GetSortedAdresy();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().BeEquivalentTo(expectedAdresy);
    }

    [Fact]
    public void GetAdresyCount_ShouldReturnOkWithCount()
    {
        var expectedCount = 42;
        _mockObslugaAdresow.Setup(s => s.PoliczAdresy()).Returns(expectedCount);

        var result = _controller.GetAdresyCount();

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult.StatusCode.Should().Be(200);
        okResult.Value.Should().Be(expectedCount);
    }
}
