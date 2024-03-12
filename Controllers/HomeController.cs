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
				AllowedAttributes = new HashSet<string>(),
			};
            var sanitizer = new HtmlSanitizer(options);

            // This mimics the HTML-Encoded message received from the client-side
            // The decoded version of the message is "<b>asdf & asdfls &lt;b&gt;asdf &amp; asdfls&lt;/b&gt;</b>"
            var rawEncodedHtml = "&lt;b&gt;asdf &amp; asdfls &amp;lt;b&amp;gt;asdf &amp;amp; asdfls&amp;lt;/b&amp;gt;&lt;/b&gt;";

			// Decode the message content only once
			var decodedHtml = WebUtility.HtmlDecode(rawEncodedHtml);

			// Sanitize the content
			var sanitizedHtml = sanitizer.Sanitize(decodedHtml);

			// Call the API to save the message ...

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
