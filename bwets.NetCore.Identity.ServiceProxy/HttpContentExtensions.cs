using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace bwets.NetCore.Identity.ServiceProxy
{
	public static class HttpContentExtensions
	{
		public static async Task<T> ReadAsAsync<T>(this HttpContent content)
		{
			var json = await content.ReadAsStringAsync();
			var value = JsonConvert.DeserializeObject<T>(json);
			return value;
		}
	}
}