using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCoreFeatures.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private static List<WeatherForecast> forecasts = new List<WeatherForecast>();//List that will serve us for the post method instead of db

        //We add log4net in the program class
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly LinkGenerator _linkGenerator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, LinkGenerator linkGenerator)
        {
            _logger = logger;
            _linkGenerator = linkGenerator;
        }



        //The status code below will be displayed on swagger, this example taken from the flights project

        /// <summary>
        /// Get all weather forecasts
        /// </summary>
        /// <response code="204">The airline company has been updated successfully</response>
        /// <response code="400">If the airline company id is different between the url and the body</response> 
        /// <response code="401">If the user is not authenticated as administrator</response> 
        /// <response code="404">If the country id doesn't point to existing country</response> 
        /// <response code="409">If there is another airline company with same name</response> 
        [HttpGet]
        public ActionResult<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Id=index,
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        /// <summary>
        /// Get forecast by id
        /// </summary>
        /// <response code="200">If the wethear forecast has been found</response>
        /// <response code="404">If the weather forecast hasn't been found</response> 
        [HttpGet("Forecasts/{id}")]
        public ActionResult<WeatherForecast> GetForecastById(int id)
        {
            WeatherForecast forecast = forecasts.FirstOrDefault(f => f.Id == id);
            if (forecasts == null)
                return NotFound();

            return Ok(forecast);         
        }

        //Here we create sample request that we be displayed in the swagger

        /// <summary>
        /// Create new weather forecast
        /// </summary>
        /// <returns>Weather forecast that has been created</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /WeatherForecast
        ///     {  
        ///         "id": 1,
        ///         "date": 2021-10-10 18:00:00,
        ///         "temperatureC": 32,
        ///         "summary": "Chilly",
        ///     }
        /// </remarks>  
        /// <param name="forecast">Weather forecast to create</param>
        /// <response code="201">If the wethear forecast has created successfuly</response>

        [HttpPost]
        public ActionResult<WeatherForecast> Post(WeatherForecast forecast)
        {
            forecasts.Add(forecast);
            //Generate the uri with name of the action, name of the controller and the id to the location of the saved object
            string uri = _linkGenerator.GetPathByAction(nameof(WeatherForecastController.GetForecastById), "WeatherForecast", new { id = forecast.Id });

            //The route will be return in the response headers as location
            //We can see in the swagger
            return Created(uri, forecast);
        }
    }
}
