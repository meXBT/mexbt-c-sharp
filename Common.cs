using System.Runtime.Serialization;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using System;

namespace Mexbt.Api
{
	[DataContract]
	public abstract class MexbtResponse
	{
		[DataMember(Name = "rejectReason")] public string RejectReason { get; set; }
		[DataMember(Name = "isAccepted")]   public bool   IsAccepted   { get; set; }
	}

	public class MexbtEmptyResponseException : Exception
	{
		public MexbtEmptyResponseException(string message) : base (message) { }
		public MexbtEmptyResponseException() : base () { }
	}

	internal class Common
	{
		internal static HttpClient getClient(string url)
		{
			HttpClient client = new HttpClient();
			client.BaseAddress = new Uri(url);
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			return client;
		}

		internal static async Task<Res> PostRequest<Req, Res> (HttpClient client, string url, Req req) where Res : MexbtResponse
		{
			HttpResponseMessage response = await client.PostAsJsonAsync(url, req);
			response.EnsureSuccessStatusCode ();

			Res r = await response.Content.ReadAsAsync<Res> ();

			if (r == null) {
				throw new MexbtEmptyResponseException ();
			} else {
				return r;
			}
		}
	}
}