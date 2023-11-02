using System;
namespace DCDServer.Subscription.Middlewares
{
	public static class DatabaseSubscriptionMiddleware
	{
		public static void UseDatabaseSubscription<T>(this WebApplication webApplication,string tableName) where T:class,IDatabaseSubscription
		{
			var subscription = (T)webApplication.Services.GetService(typeof(T));
			subscription.Configure(tableName);
		}
	}
}

