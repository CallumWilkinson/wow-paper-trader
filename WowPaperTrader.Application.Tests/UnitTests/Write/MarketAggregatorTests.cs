using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates;
using WowPaperTrader.Application.Features.Write.AuctionHouseSnapshot.MarketAggregates.AggregateResult;
using WowPaperTrader.Application.Tests.UnitTests.Write.TestData;

namespace WowPaperTrader.Application.Tests.UnitTests.Write;

public sealed class MarketAggregatorTests
{
  [Fact]
  public void Aggregate_ShouldReturnMinimumUnitPrice()
  {
    //arrange
    var listings = new[]
    {
      MarketAggregatorTestData.CreateCommodityListing(unitPrice: 100),
      MarketAggregatorTestData.CreateCommodityListing(unitPrice: 50),
      MarketAggregatorTestData.CreateCommodityListing(unitPrice: 200),

    };
    
    //act
    var result = MarketAggregator.Aggregate(listings);

    //assert
    var aggregate = Assert.Single(result);
    
    Assert.Equal(50, aggregate.MinimumUnitPrice);
  }

  [Fact]
  public void Aggregate_ShouldReturnTotalQuantity()
  {
    //arrange 
    var listings = new[]
    {
      MarketAggregatorTestData.CreateCommodityListing(quantity: 5),
      MarketAggregatorTestData.CreateCommodityListing(quantity: 20),
      MarketAggregatorTestData.CreateCommodityListing(quantity: 3)
    };
    
    //act
    var result = MarketAggregator.Aggregate(listings);
    
    //assert
    var aggregate = Assert.Single(result);
    
    Assert.Equal(28, aggregate.TotalQuantity);
  }

  [Fact]
  public void Aggregate_ShouldReturnListingCount()
  {
    //arrange
    var listings = new[]
    {
      MarketAggregatorTestData.CreateCommodityListing(quantity: 50),
      MarketAggregatorTestData.CreateCommodityListing(quantity: 20),
      MarketAggregatorTestData.CreateCommodityListing(quantity: 5)
    };
    
    //act
    var result = MarketAggregator.Aggregate(listings);
    
    //assert
    var aggregate = Assert.Single(result);
    
    Assert.Equal(3, aggregate.ListingCount);
  }

  [Fact]
  public void Aggregate_ShouldGroupListingsByItemId()
  {
    //arrange
    var listings = new[]
    {
      MarketAggregatorTestData.CreateCommodityListing(
        itemId: 100, quantity: 5, unitPrice: 200),

      MarketAggregatorTestData.CreateCommodityListing(
        itemId: 200, quantity: 10, unitPrice: 500),

      MarketAggregatorTestData.CreateCommodityListing(
        itemId: 100, quantity: 15, unitPrice: 100),

      MarketAggregatorTestData.CreateCommodityListing(
        itemId: 200, quantity: 20, unitPrice: 400),

      MarketAggregatorTestData.CreateCommodityListing(
        itemId: 300, quantity: 8, unitPrice: 800)
    };
    
    //act
    var result = MarketAggregator.Aggregate(listings);
    
    //assert
    var item100 = Assert.Single(result, x => x.ItemId == 100);
    
    Assert.Equal(100, item100.ItemId);
    Assert.Equal(100, item100.MinimumUnitPrice);
    Assert.Equal(20, item100.TotalQuantity);
    Assert.Equal(2, item100.ListingCount);
    Assert.Equal(0, item100.VariantKey);

    var item200 = Assert.Single(result, x => x.ItemId == 200);
    
    Assert.Equal(200, item200.ItemId);
    Assert.Equal(400, item200.MinimumUnitPrice);
    Assert.Equal(30, item200.TotalQuantity);
    Assert.Equal(2, item200.ListingCount);
    Assert.Equal(0, item200.VariantKey);

    var item300 = Assert.Single(result, x => x.ItemId == 300);
    
    Assert.Equal(300, item300.ItemId);
    Assert.Equal(800, item300.MinimumUnitPrice);
    Assert.Equal(8, item300.TotalQuantity);
    Assert.Equal(1, item300.ListingCount);
    Assert.Equal(0, item300.VariantKey);
    
  }
  
  [Fact]
  public void Aggregate_ShouldHandleRepeatedPricesAndUnequalQuantities()
  {
    // Arrange
    var listings = new[]
    {
      MarketAggregatorTestData.CreateCommodityListing(
        quantity: 4, unitPrice: 100),

      MarketAggregatorTestData.CreateCommodityListing(
        quantity: 20, unitPrice: 100),

      MarketAggregatorTestData.CreateCommodityListing(
        quantity: 1, unitPrice: 100),

      MarketAggregatorTestData.CreateCommodityListing(
        quantity: 5, unitPrice: 101)
    };

    // Act
    var result = MarketAggregator.Aggregate(listings);

    // Assert
    var aggregate = Assert.Single(result);

    Assert.Equal(100, aggregate.MinimumUnitPrice);
    Assert.Equal(30, aggregate.TotalQuantity);
    Assert.Equal(4, aggregate.ListingCount);
  }

}



