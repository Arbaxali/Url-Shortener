using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using UrlShortner.Models;
using UrlShortner.Services;

namespace UrlShortner.Controllers
{
   
    public class UrlShController : Controller
    {
        
        // GET: UrlShController
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{shortCode}")]
        public ActionResult redirecturl(string shortCode)
        {
            MongoService mongoService = new MongoService();
            var model = mongoService.FindByShortCode(shortCode);
            if (model == null)
            {
                return NotFound(); // Or handle the case where the shortened URL is not found
            }

            string redirectUrl = model.OriginalUrl;
            if(!redirectUrl.StartsWith("http://") && !redirectUrl.StartsWith("https://"))
            {
                redirectUrl = "http://" + redirectUrl;
            }

            mongoService.IncrementVisitorCount();
            return Redirect(redirectUrl);
        }



        [HttpPost]
        [Route("Urlshort")]
        public ActionResult GenerateshortUrl([FromQuery] string originalUrl)
        {
            MongoService mongoService = new MongoService();
            try
            {
                if (string.IsNullOrEmpty(originalUrl))
                {
                    return BadRequest("Original URL cannot be null or empty");
                }

                string res = UrlService.GenerateShortUrl(originalUrl);
                mongoService.IncrementVisitorCount();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        [HttpGet]
        [Route("api/visitorCount")]
        public IActionResult GetVisitorCount()
        {
            MongoService mongoService = new MongoService();
            var visitorCount = mongoService.GetCountcollection();
            if (visitorCount == null)
            {
                visitorCount = new VisitorCount { Count = 0 };
                mongoService.insertifnone(visitorCount);
            }
            return Ok(visitorCount.Count);
        }



    }
}
