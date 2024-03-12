using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using TestApp.Models;

namespace TestApp.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public IActionResult Index()
		{
            var options = new HtmlSanitizerOptions
            {
				AllowedTags = new HashSet<string> { "b", "i", "u" },
				AllowedAttributes = new HashSet<string> { "class" },
			};
            var sanitizer = new HtmlSanitizer(options);

			var rawEncodedHtml = "&amp;lt;p&amp;gt;&amp;lt;strong&amp;gt;hello world &amp;amp;lt;div&amp;amp;gt;test &amp;amp;amp;&amp;amp;lt;/div&amp;amp;gt;&amp;lt;/strong&amp;gt;&amp;lt;/p&amp;gt;";
			var decodedHtml = WebUtility.HtmlDecode(rawEncodedHtml);

			var sanitizedHtml = sanitizer.Sanitize(decodedHtml);
			//ViewBag.HtmlContent = WebUtility.HtmlDecode(sanitizedHtml);
			ViewBag.HtmlContent = sanitizedHtml;
            return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
