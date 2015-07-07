using NUnit.Framework;
using System;

namespace Mexbt.Api
{
	[TestFixture ()]
	public class PublicTest
	{
		[Test ()]
		public void TestTicker ()
		{
			var ticker = Public.Ticker("BTCMXN").Result;
			TestUtils.AssertAccepted (ticker);
		}

		[Test ()]
		public void TestTrades ()
		{
			var trades = Public.Trades ("BTCMXN").Result;
			TestUtils.AssertAccepted (trades);
		}

		[Test ()]
		public void TestTradesByDate ()
		{
			var tradesByDate = Public.TradesByTrade (ins: "BTCMXN", startDate: 1416530012, endDate: 1416559390).Result;
			TestUtils.AssertAccepted (tradesByDate);
		}

		[Test ()]
		public void TestOrderBook ()
		{
			var orderBook = Public.OrderBook ("BTCMXN").Result;
			TestUtils.AssertAccepted (orderBook);
		}

		[Test ()]
		public void TestProductPairs ()
		{
			var productPairs = Public.ProductPairs ().Result;
			TestUtils.AssertAccepted (productPairs);
		}
	}
}

