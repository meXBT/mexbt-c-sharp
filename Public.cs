using System.Runtime.Serialization;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace Mexbt.Api
{
	[DataContract]
	internal class TickerRequest
	{
		[DataMember(Name = "productPair")] public string ProductPair { get; set; }
	}

	[DataContract]
	public class TickerResponse : MexbtResponse
	{
		[DataMember(Name = "volume")] public double Volume { get; set; }
		[DataMember(Name = "high")]   public double High   { get; set; }
		[DataMember(Name = "last")]   public double Last   { get; set; }
		[DataMember(Name = "ask")]    public double Ask    { get; set; }
		[DataMember(Name = "bid")]    public double Bid    { get; set; }
		[DataMember(Name = "low")]    public double Low    { get; set; }

		[DataMember(Name = "Total24HrProduct2Traded")] public double Total24HrProduct2Traded { get; set; }
		[DataMember(Name = "Total24HrQtyTraded")]      public double Total24HrQtyTraded      { get; set; }
		[DataMember(Name = "Total24HrNumTrades")]      public int    Total24HrNumTrades      { get; set; }

		[DataMember(Name = "numOfCreateOrders")] public int NumOfCreateOrders { get; set; }
		[DataMember(Name = "sellOrderCount")]    public int SellOrderCount    { get; set; }
		[DataMember(Name = "buyOrderCount")]     public int BuyOrderCount     { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Ticker: RejectReason={0}, IsAccepted={1}, High={2}, Last={3}, Bid={4}, Volume={5}, Low={6}, Ask={7}, Total24HrQtyTraded={8}, Total24HrProduct2Traded={9}, Total24HrNumTrades={10}, SellOrderCount={11}, BuyOrderCount={12}, NumOfCreateOrders={13}]", RejectReason, IsAccepted, High, Last, Bid, Volume, Low, Ask, Total24HrQtyTraded, Total24HrProduct2Traded, Total24HrNumTrades, SellOrderCount, BuyOrderCount, NumOfCreateOrders);
		}
	}

	[DataContract]
	internal class TradesRequest
	{
		[DataMember(Name = "startIndex")] public int StartIndex { get; set; }
		[DataMember(Name = "count")]      public int Count      { get; set; }

		[DataMember(Name = "ins")] public string Ins { get; set; }
	}

	[DataContract]
	public class Trade
	{
		[DataMember(Name = "tid")] public int Tid { get; set; }

		[DataMember(Name = "qty")] public double Qty { get; set; }
		[DataMember(Name = "px")]  public double Px { get; set; }

		[DataMember(Name = "unixtime")] public long UnixTime { get; set; }
		[DataMember(Name = "utcticks")] public long UtcTicks { get; set; }

		[DataMember(Name = "incomingOrderSideId")] public int IncomingOrderSideId { get; set; }
		[DataMember(Name = "incomingOrderSide")]   public int IncomingOrderSide   { get; set; }
		[DataMember(Name = "bookServerOrderId")]   public int BookServerOrderId   { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Trade: Tid={0}, Px={1}, Qty={2}, UnixTime={3}, UtcTicks={4}, IncomingOrderSide={5}, IncomingOrderSideId={6}, BookServerOrderId={7}]",
				Tid, Px, Qty, UnixTime, UtcTicks, IncomingOrderSide, IncomingOrderSideId, BookServerOrderId);
		}
	}

	[DataContract]
	public class TradesResponse : MexbtResponse
	{
		[DataMember(Name = "dateTimeUtc")] public long DateTimeUtc { get; set; }
		[DataMember(Name = "startIndex")]  public int  StartIndex  { get; set; }
		[DataMember(Name = "count")]       public int  Count       { get; set; }

		[DataMember(Name = "ins")] public string Ins { get; set; }

		[DataMember(Name = "trades")] public Trade[] Trades { get; set; }

		public override string ToString ()
		{
			return string.Format ("[TradesResponse: DateTimeUtc={0}, StartIndex={1}, Count={2}, Ins={3}, Trades={4}]",
				DateTimeUtc, StartIndex, Count, Ins, Trades);
		}
	}

	[DataContract]
	internal class TradesByDateRequest
	{
		[DataMember(Name = "startDate")] public long StartDate { get; set; }
		[DataMember(Name = "endDate")]   public long EndDate   { get; set; }

		[DataMember(Name = "ins")] public string Ins { get; set; }
	}

	[DataContract]
	public class TradesByDateResponse : MexbtResponse
	{
		[DataMember(Name = "dateTimeUtc")] public long DateTimeUtc { get; set; }

		[DataMember(Name = "ins")] public string Ins { get; set; }

		[DataMember(Name = "startDate")] public long StartDate { get; set; }
        [DataMember(Name = "endDate")]   public long EndDate   { get; set; }

		[DataMember(Name = "trades")] public Trade[] Trades { get; set; }

		public override string ToString ()
		{
			return string.Format ("[TradesByDateResponse: DateTimeUtc={0}, Ins={1}, StartDate={2}, EndDate={3}, Trades={4}]",
				DateTimeUtc, Ins, StartDate, EndDate, Trades);
		}
	}

	[DataContract]
	public class Order
	{
		[DataMember(Name = "qty")] public double Qty { get; set; }
		[DataMember(Name = "px")]  public double Px  { get; set; }

		public override string ToString ()
		{
			return string.Format ("[Order: Qty={0}, Px={1}]", Qty, Px);
		}
	}

	[DataContract]
	internal class OrderBookRequest
	{
		[DataMember(Name = "productPair")] public string ProductPair { get; set; }
	}

	[DataContract]
	public class OrderBookResponse : MexbtResponse
	{
		[DataMember(Name = "asks")] public Order[] Asks { get; set; }
		[DataMember(Name = "bids")] public Order[] Bids { get; set; }

		public override string ToString ()
		{
			return string.Format ("[OrderBookResponse: Asks={0}, Bids={1}]", Asks, Bids);
		}
	}

	[DataContract]
	public class ProductPair
	{
		[DataMember(Name = "product1DecimalPlaces")] public string Product1DecimalPlaces { get; set; }
		[DataMember(Name = "product2DecimalPlaces")] public string Product2DecimalPlaces { get; set; }

		[DataMember(Name = "product1Label")] public string Product1Label { get; set; }
		[DataMember(Name = "product2Label")] public string Product2Label { get; set; }

		[DataMember(Name = "productPairCode")] public int    ProductPairCode { get; set; }
		[DataMember(Name = "name")]            public string Name            { get; set; }

		public override string ToString ()
		{
			return string.Format ("[ProductPair: Product1DecimalPlaces={0}, Product2DecimalPlaces={1}, Product1Label={2}, Product2Label={3}, ProductPairCode={4}, Name={5}]",
				Product1DecimalPlaces, Product2DecimalPlaces, Product1Label, Product2Label, ProductPairCode, Name);
		}
	}

	[DataContract]
	internal class ProductPairsRequest { }

	[DataContract]
	public class ProductPairsResponse : MexbtResponse
	{
		[DataMember(Name = "productPairs")] public ProductPair[] ProductPairs { get; set; }

		public override string ToString ()
		{
			return string.Format ("[ProductPairsResponse: ProductPairs={0}]", ProductPairs);
		}
	}

	public class Public
	{
		private static HttpClient PUBLIC_CLIENT = Common.getClient("https://public-api.mexbt.com/v1/");

		public static async Task<TradesByDateResponse> TradesByTrade(string ins, long startDate, long endDate)
		{
			return await Common.PostRequest<TradesByDateRequest, TradesByDateResponse> (PUBLIC_CLIENT, "trades-by-date", new TradesByDateRequest {
				StartDate = startDate,
				EndDate   = endDate,
				Ins       = ins
			});
		}

		public static async Task<TradesResponse> Trades(string ins, int startIndex = -1, int count = 20)
		{
			return await Common.PostRequest<TradesRequest, TradesResponse> (PUBLIC_CLIENT, "trades", new TradesRequest {Ins = ins, StartIndex = startIndex, Count = count});
		}

		public static async Task<OrderBookResponse> OrderBook(string productPair)
		{
			return await Common.PostRequest<OrderBookRequest, OrderBookResponse> (PUBLIC_CLIENT, "order-book", new OrderBookRequest {ProductPair = productPair});
		}

		public static async Task<ProductPairsResponse> ProductPairs()
		{
			return await Common.PostRequest<ProductPairsRequest, ProductPairsResponse> (PUBLIC_CLIENT, "product-pairs", new ProductPairsRequest {});
		}

		public static async Task<TickerResponse> Ticker(string productPair)
		{
			return await Common.PostRequest<TickerRequest, TickerResponse> (PUBLIC_CLIENT, "ticker", new TickerRequest {ProductPair = productPair});
		}
	}
}