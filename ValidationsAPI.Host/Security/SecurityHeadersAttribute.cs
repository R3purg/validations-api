using Microsoft.AspNetCore.Mvc.Filters;

namespace ValidationsAPI.Host.Security
{
	public class SecurityHeadersAttribute : ActionFilterAttribute
	{
		public override void OnResultExecuting(ResultExecutingContext context)
		{
			if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Type-Options"))
				context.HttpContext.Response.Headers.Add("X-Content-Type-Options", "nosniff");

			if (!context.HttpContext.Response.Headers.ContainsKey("X-Frame-Options"))
				context.HttpContext.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

			var csp = "default-src 'self'; object-src 'none'; img-src 'self' data:; frame-ancestors 'none'; sandbox allow-forms allow-same-origin allow-scripts allow-popups; base-uri 'self';";

			// For standards compliant browsers
			if (!context.HttpContext.Response.Headers.ContainsKey("Content-Security-Policy"))
				context.HttpContext.Response.Headers.Add("Content-Security-Policy", csp);

			// And once again for IE
			if (!context.HttpContext.Response.Headers.ContainsKey("X-Content-Security-Policy"))
				context.HttpContext.Response.Headers.Add("X-Content-Security-Policy", csp);

			var referrer_policy = "no-referrer";

			if (!context.HttpContext.Response.Headers.ContainsKey("Referrer-Policy"))
				context.HttpContext.Response.Headers.Add("Referrer-Policy", referrer_policy);

			var transport_security = "max-age=31536000; includeSubDomains";

			if (!context.HttpContext.Response.Headers.ContainsKey("Strict-Transport-Security"))
				context.HttpContext.Response.Headers.Add("Strict-Transport-Security", transport_security);

			var permissions_policy = "*";

			if (!context.HttpContext.Response.Headers.ContainsKey("Permissions-Policy"))
				context.HttpContext.Response.Headers.Add("Permissions-Policy", permissions_policy);
		}
	}
}
