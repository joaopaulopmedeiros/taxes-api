using System;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Taxes.Api.Controllers;
using Taxes.Api.Models;
using Taxes.Api.Requests;
using Taxes.Api.Responses;
using Taxes.Api.Services;

using Xunit;

namespace Taxes.Api.Tests;

public class TaxesControllerTests
{
    [Fact]
    public async Task WhenAnyItemsFoundMustReturn200StatusCode()
    {
        //arrange
        var request = new TaxSearchRequest 
        {
            StartAt = DateTime.Now,
            EndAt = DateTime.Now.AddDays(2) 
        };

        var mockService = new Mock<ITaxSearchService>();

        var taxes = new TaxSearchResponse
        {
          new Tax(request.StartAt.ToString("dd/MM/yyyy"), "5000", TaxType.Selic)
        };

        mockService.Setup(s => s.SearchByAsync(request)).ReturnsAsync(taxes);

        var sut = new TaxesController();

        //act 
        var response = await sut.SearchSelicTaxAsync(request, mockService.Object);

        //assert
        response.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task WhenNoItemsFoundMustReturn204StatusCode()
    {
        //arrange
        var request = new TaxSearchRequest
        {
            StartAt = DateTime.Now.AddYears(1),
            EndAt = DateTime.Now.AddYears(2)
        };

        var mockService = new Mock<ITaxSearchService>();

        var emptyTaxes = new TaxSearchResponse();

        mockService.Setup(s => s.SearchByAsync(request)).ReturnsAsync(emptyTaxes);

        var sut = new TaxesController();

        //act
        var response = await sut.SearchSelicTaxAsync(request, mockService.Object);

        //assert
        response.Should().BeOfType<NoContentResult>();
    }
}