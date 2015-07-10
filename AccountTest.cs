using NUnit.Framework;
using System;

namespace Mexbt.Api
{
	[TestFixture ()]
	internal class AccountTest
	{
		private static Account account = new Account(
			publicKey:  "<YOUR PUBLIC KEY>",
			privateKey: "<YOUR PRIVATE KEY>",
			userId:     "<YOUR USER ID>"
		);

		[Test ()]
		public void Test_ManageOrders ()
		{
			var cancelAllOrders = account.CancelAllOrders ("BTCUSD").Result;
			TestUtils.AssertAccepted (cancelAllOrders);

			var createOrder = account.CreateLimitOrder (
				side: "buy",
				ins: "BTCUSD",
				qty: 1.0,
				px: 342.99
			).Result;

			TestUtils.AssertAccepted (createOrder);

			var moveOrderToTop = account.MoveOrderToTop ("BTCUSD", createOrder.ServerOrderId).Result;
			TestUtils.AssertAccepted (moveOrderToTop);

			var executeOrderNow = account.ExecuteOrderNow ("BTCUSD", createOrder.ServerOrderId).Result;
			TestUtils.AssertAccepted (executeOrderNow);

			var cancelOrder = account.CancelOrder ("BTCUSD", createOrder.ServerOrderId).Result;
			TestUtils.AssertAccepted (cancelOrder);
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
		public void TestDepositAddressByCurrency ()
		{
			var depositAddressByCurrency = account.DepositAddressByCurrency ("BTC").Result;
			TestUtils.AssertAccepted (depositAddressByCurrency);
		}

		[Test ()]
		public void TestWithdraw ()
		{
			var withdraw = account.Withdraw ("BTC", "1yL8LFT5qqzPJY3hMRQCJd5CTs2F7SHjv", 1.12345678).Result;
			TestUtils.AssertAccepted (withdraw);
		}
	}
}

