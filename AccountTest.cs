using NUnit.Framework;
using System;

namespace Mexbt.Api
{
	[TestFixture ()]
	internal class AccountTest
	{
		private static AuthData authData = new AuthData(
			publicKey:  "<YOUR PUBLIC KEY>",
			privateKey: "<YOUR PRIVATE KEY>",
			userId:     "<YOUR USER ID>"
		);

		private static Account account = new Account(authData, isSandbox: true);

		[Test ()]
		public void TestCreateOrder ()
		{
			var createOrderResponse = account.CreateLimitOrder (
				side: "buy",
				ins: "BTCUSD",
				qty: 1.0,
				px: 342.99
			).Result;

			TestUtils.AssertAccepted (createOrderResponse);
		}

		[Test ()]
		public void TestInfo ()
		{
			var info = account.Info ().Result;
			Assert.IsTrue (info.IsAccepted);
		}

		[Test ()]
		public void TestBalance ()
		{
			var balance = account.Balance ().Result;
			TestUtils.AssertAccepted (balance);
		}

		[Test ()]
		public void TestTrades ()
		{
			var trades = account.Trades ("BTCUSD").Result;
			TestUtils.AssertAccepted (trades);
		}

		[Test ()]
		public void TestOrders ()
		{
			var orders = account.Orders ().Result;
			TestUtils.AssertAccepted (orders);
		}

		[Test ()]
		public void TestDepositAddresses ()
		{
			var depositAddresses = account.DepositAddresses ().Result;
			TestUtils.AssertAccepted (depositAddresses);
		}

		[Test ()]
		public void TestWithdraw ()
		{
			var withdraw = account.Withdraw ("BTC", "1yL8LFT5qqzPJY3hMRQCJd5CTs2F7SHjv", 1.12345678).Result;
			TestUtils.AssertAccepted (withdraw);
		}
	}
}

