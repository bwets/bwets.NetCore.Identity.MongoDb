using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace bwets.NetCore.Identity.Service
{
	public abstract class BaseApiController : Controller
	{
		protected BaseApiController(ILogger log)
		{
			Log = log;
		}

		protected ILogger Log { get; }


		protected T ExecuteWithLog<T>(Func<T> execute)
		{
			Log.LogDebug($"Request started: {Request.Path}");
			try
			{
				var result = execute();
				Log.LogDebug($"Request completed: {Request.Path}");
				return result;
			}
			catch (Exception e)
			{
				Log.LogError($"Request  failed: {Request.Path}");
				throw;
			}
		}

		protected void ExecuteWithLog(Action execute)
		{
			Log.LogDebug($"Request started: {Request.Path}");
			try
			{
				execute();
				Log.LogDebug($"Request completed: {Request.Path}");
			}
			catch (Exception e)
			{
				Log.LogError(e, $"Request failed: {Request.Path}");
				throw;
			}
		}
	}
}