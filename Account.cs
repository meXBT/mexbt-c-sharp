using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace Mexbt.Api
{
	public class AuthData
	{
		public string PrivateKey { get; }
		public string PublicKey  { get; }
		public string UserId     { get; }

		public bool IsSandbox { get; }

		public AuthData(string publicKey, string privateKey, string userId, bool isSandbox = true)
		{
			PrivateKey = privateKey;
			PublicKey  = publicKey;
			UserId     = userId;

			IsSandbox = isSandbox;
		}
	}

	[DataContract]
	internal abstract class PrivateRequest
	{
		[DataMember(Name = "apiNonce")] public long   ApiNonce { get; set; }
		[DataMember(Name = "apiKey")]   public string ApiKey   { get; set; }
		[DataMember(Name = "apiSig")]   public string ApiSig   { get; set; }

		private bool isSigned = false;

		public bool IsSigned
		{
			get { return isSigned; }
		}

		public void Sign(AuthData authData)
		{
			Tuple<long, string> a = sign (authData);
			string message = a.Item2;
			long nonce = a.Item1;
			string digest = hmac (authData.PrivateKey, message);

			ApiNonce = nonce;
			ApiKey   = authData.PublicKey;
			ApiSig   = digest;

			isSigned = true;
		}

		private static Tuple<long, string> sign(AuthData authData)
		{
			long nonce = DateTime.UtcNow.Ticks;
			string message = nonce + authData.UserId + authData.PublicKey;

			return new Tuple<long, string> (nonce, message);
		}

		private static string hmac(string secret, string message)
		{
			var encoding = new System.Text.UTF8Encoding();
			byte[] keyBytes = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);

			using (var hmacsha256 = new HMACSHA256(keyBytes))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				return BitConverter.ToString (hashmessage).Replace("-", "").ToUpper();
			}
		}
	}

	[DataContract]
	internal class CreateOrderRequest : PrivateRequest
	{
		[DataMember(Name = "orderType")] public int    OrderType { get; set; }
		[DataMember(Name = "side")]      public string Side      { get; set; }
		[DataMember(Name = "ins")]       public string Ins       { get; set; }

		[DataMember(Name = "qty")] public double Qty { get; set; }
		[DataMember(Name = "px")]  public double  Px  { get; set; }
	}

	[DataContract]
	public class CreateOrderResponse : MexbtResponse
	{
		[DataMember(Name = "serverOrderId")] public int  ServerOrderId { get; set; }
		[DataMember(Name = "dateTimeUtc")]   public long DateTimeUtc   { get; set; }

		public override string ToString ()
		{
			return string.Format ("[CreateOrderResponse: ServerOrderId={0}, DateTimeUtc={1}]", ServerOrderId,
				DateTimeUtc);
		}
	}

	[DataContract]
	internal class ModifyOrderRequest : PrivateRequest
	{
		[DataMember(Name = "serverOrderId")] public int    ServerOrderId { get; set; }
		[DataMember(Name = "modifyAction")]  public int    ModifyAction  { get; set; }
		[DataMember(Name = "ins")]           public string Ins           { get; set; }
	}

	[DataContract]
	public class ModifyOrderResponse : MexbtResponse
	{
		[DataMember(Name = "serverOrderId")] public int  ServerOrderId { get; set; }
		[DataMember(Name = "dateTimeUtc")]   public long DateTimeUtc   { get; set; }
	}

	[DataContract]
	internal class CancelOrderRequest : PrivateRequest
	{
		[DataMember(Name = "serverOrderId")] public int    ServerOrderId { get; set; }
		[DataMember(Name = "ins")]           public string Ins           { get; set; }
	}

	[DataContract]
	public class CancelOrderResponse : MexbtResponse
	{
		[DataMember(Name = "serverOrderId")] public int  ServerOrderId { get; set; }
		[DataMember(Name = "dateTimeUtc")]   public long DateTimeUtc   { get; set; }
	}

	[DataContract]
	internal class CancelAllOrdersRequest : PrivateRequest
	{
		[DataMember(Name = "ins")] public string Ins { get; set; }
	}

	[DataContract]
	public class CancelAllOrdersResponse : MexbtResponse { }

	[DataContract]
	internal class WithdrawRequest : PrivateRequest
	{
		[DataMember(Name = "sendToAddress")] public string SendToAddress { get; set; }
		[DataMember(Name = "amount")]        public string Amount        { get; set; }
		[DataMember(Name = "ins")]           public string Ins           { get; set; }

		internal void formatProductionAmount(double amount)
		{
			Amount = amount.ToString ("0.00000000");
		}

		internal void formatSandboxAmount(double amount)
		{
			Amount = amount.ToString ("0.000000");
		}
	}

	[DataContract]
	public class KeyValuePair
	{
		[DataMember(Name = "value")] public string Value { get; set; }
		[DataMember(Name = "key")]   public string Key   { get; set; }

		public override string ToString ()
		{
			return string.Format ("[KeyValuePair: Key={0}, Value={1}]", Key, Value);
		}
	}

	[DataContract]
	public class WithdrawResponse : MexbtResponse { }

	[DataContract]
	internal class InfoRequest : PrivateRequest { }

	[DataContract]
	public class InfoResponse : MexbtResponse
	{
		[DataMember(Name = "userInfoKVP")] public KeyValuePair[] UserInfoKVP { get; set; }

		public override string ToString ()
		{
			return string.Format ("[MeResponse: UserInfoKVP={0}]", UserInfoKVP);
		}
	}

	[DataContract]
	public class Currency
	{
		[DataMember(Name = "balance")] public double Balance { get; set; }
		[DataMember(Name = "hold")]    public double Hold    { get; set; }

		[DataMember(Name = "tradeCount")] public int    TradeCount { get; set; }
		[DataMember(Name = "name")]       public string Name       { get; set; }
	}

	[DataContract]
	internal class BalanceRequest : PrivateRequest { }

	public class BalanceResponse : MexbtResponse
	{
		[DataMember(Name = "currencies")] public Currency[] Currencies { get; set; }
	}

	[DataContract]
	public class AccountTrade
	{
		[DataMember(Name = "time")] public long Time { get; set; }
		[DataMember(Name = "tid")]  public int  Tid  { get; set; }

		[DataMember(Name = "qty")] public double Qty { get; set; }
		[DataMember(Name = "px")]  public double Px  { get; set; }

		[DataMember(Name = "incomingServerOrderId")] public int IncomingServerOrderId { get; set; }
		[DataMember(Name = "incomingOrderSide")]     public int IncomingOrderSide     { get; set; }
		[DataMember(Name = "bookServerOrderId")]     public int BookServerOrderId     { get; set; }
	}

	[DataContract]
	internal class AccountTradesRequest : PrivateRequest
	{
		[DataMember(Name = "startIndex")] public int StartIndex { get; set; }
		[DataMember(Name = "count")]      public int Count      { get; set; }

		[DataMember(Name = "dateTimeUtc")] public long DateTimeUtc { get; set; }

		[DataMember(Name = "ins")] public string Ins { get; set; }
	}

	[DataContract]
	public class AccountTradesResponse : MexbtResponse
	{
		[DataMember(Name = "startIndex")] public int StartIndex { get; set; }
		[DataMember(Name = "count")]      public int Count      { get; set; }

		[DataMember(Name = "ins")] public string Ins { get; set; }
	}

	[DataContract]
	public class OpenOrder
	{
		[DataMember(Name = "QtyRemaining")] public double QtyRemaining { get; set; }
		[DataMember(Name = "QtyTotal")]     public double QtyTotal     { get; set; }
		[DataMember(Name = "Price")]        public double Price        { get; set; }

		[DataMember(Name = "ServerOrderId")] public int ServerOrderId { get; set; }
		[DataMember(Name = "AccountId")]     public int AccountId     { get; set; }
		[DataMember(Name = "Side")]          public int Side          { get; set; }

		[DataMember(Name = "ReceiveTime")] public long ReceiveTime { get; set; }
	}

	[DataContract]
	public class OpenOrderInfo
	{
		[DataMember(Name = "openOrders")] public OpenOrder[] OpenOrders { get; set; }
		[DataMember(Name = "ins")]        public string      Ins        { get; set; }
	}

	[DataContract]
	internal class AccountOrdersRequest : PrivateRequest { }

	[DataContract]
	public class AccountOrdersResponse : MexbtResponse
	{
		[DataMember(Name = "openOrdersInfo")] public OpenOrderInfo[] OpenOrdersInfo { get; set; }
		[DataMember(Name = "dateTimeUtc")]    public long            DateTimeUtc    { get; set; }
	}

	[DataContract]
	public class Address
	{
		[DataMember(Name = "depositAddress")] public string DepositAddress { get; set; }
		[DataMember(Name = "name")]           public string Name           { get; set; }
	}

	[DataContract]
	internal class DepositAdressesRequest : PrivateRequest { }

	[DataContract]
	public class DepositAddressesResponse : MexbtResponse
	{
		[DataMember(Name = "addresses")] public Address[] Addresses { get; set; }
	}

	public class Account
	{
		private static HttpClient SANDBOX_CLIENT = Common.getClient("https://private-api-sandbox.mexbt.com/v1/");
		private static HttpClient PRIVATE_CLIENT = Common.getClient("https://private-api.mexbt.com/v1/");

		public AuthData Credentials { get; }
		public bool     IsSandbox   { get; }

		private HttpClient client;

		public Account (AuthData authData, bool isSandbox = true)
		{
			Credentials = authData;
			IsSandbox   = isSandbox;

			client = isSandbox ? SANDBOX_CLIENT : PRIVATE_CLIENT;
		}

		public async Task<CreateOrderResponse> CreateOrder(string ins, string side, int orderType,double px,double qty)
		{
			var req = new CreateOrderRequest {
				OrderType = orderType,
				Side = side,
				Ins = ins,
				Qty = qty,
				Px = px
			};

			return await PostPrivateRequest<CreateOrderRequest, CreateOrderResponse> ("orders/create", req);
		}

		public async Task<CreateOrderResponse> CreateLimitOrder(string ins, string side, double px, double qty)
		{
			return await CreateOrder (ins, side, 0, px, qty);
		}

		public async Task<CreateOrderResponse> CreateMarketOrder(string ins, string side, double px, double qty)
		{
			return await CreateOrder (ins, side, 1, px, qty);
		}

		public async Task<ModifyOrderResponse> ModifyOrder(string ins, int modifyAction, int serverOrderId)
		{
			var req = new ModifyOrderRequest { ServerOrderId = serverOrderId, ModifyAction = modifyAction, Ins = ins };

			return await PostPrivateRequest<ModifyOrderRequest, ModifyOrderResponse> ("orders/modify", req);
		}

		public async Task<ModifyOrderResponse> MoveOrderToTop(string ins, int serverOrderId)
		{
			return await ModifyOrder (ins, 0, serverOrderId);
		}

		public async Task<ModifyOrderResponse> ExecuteOrderNow(string ins, int serverOrderId)
		{
			return await ModifyOrder (ins, 1, serverOrderId);
		}

		public async Task<CancelOrderResponse> CancelOrder(string ins, int serverOrderId)
		{
			var req = new CancelOrderRequest { Ins = ins, ServerOrderId = serverOrderId };

			return await PostPrivateRequest<CancelOrderRequest, CancelOrderResponse> ("orders/cancel", req);
		}

		public async Task<CancelAllOrdersResponse> CancelAllOrders(string ins)
		{
			var req = new CancelAllOrdersRequest { Ins = ins };

			return await PostPrivateRequest<CancelAllOrdersRequest, CancelAllOrdersResponse> ("orders/cancel-all", req);
		}

		public async Task<InfoResponse> Info()
		{
			var req = new InfoRequest ();

			return await PostPrivateRequest<InfoRequest, InfoResponse> ("me", req);
		}

		public async Task<BalanceResponse> Balance()
		{
			var req = new BalanceRequest();

			return await PostPrivateRequest<BalanceRequest, BalanceResponse> ("balance", req);
		}

		public async Task<AccountTradesResponse> Trades(string ins, int startIndex = -1, int count = 20)
		{
			var req = new AccountTradesRequest { Ins = ins, StartIndex = startIndex, Count = count };

			return await PostPrivateRequest<AccountTradesRequest, AccountTradesResponse> ("trades", req);
		}

		public async Task<AccountOrdersResponse> Orders()
		{
			var req = new AccountOrdersRequest ();

			return await PostPrivateRequest<AccountOrdersRequest, AccountOrdersResponse> ("orders", req);
		}

		public async Task<DepositAddressesResponse> DepositAddresses()
		{
			var req = new DepositAdressesRequest ();

			return await PostPrivateRequest<DepositAdressesRequest, DepositAddressesResponse> ("deposit-addresses",req);
		}

		public async Task<WithdrawResponse> Withdraw(string ins, string address, double amount)
		{
			var req = new WithdrawRequest { Ins = ins, SendToAddress = address };

			if (Credentials.IsSandbox) {
				req.formatSandboxAmount (amount);
			} else {
				req.formatProductionAmount (amount);
			}

			return await PostPrivateRequest<WithdrawRequest, WithdrawResponse> ("withdraw", req);
		}

		private async Task<Res> PostPrivateRequest<Req, Res>(string url, Req req)
			where Req : PrivateRequest
			where Res : MexbtResponse
		{
			req.Sign (Credentials);
			return await Common.PostRequest<Req, Res> (client, url, req);
		}
	}
}