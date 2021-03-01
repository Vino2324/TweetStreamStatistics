using JHA.TweetStream.StatisticalServices.Services.Statistics;
using Microsoft.AspNetCore.Mvc;

namespace JHA.TweetStream.StatisticalServices.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;

        /// <summary>
        /// Inject the singleton statistic service
        /// </summary>
        /// <param name="statisticsService"></param>
        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet]
        public StatisticsService Get()
        {
            return _statisticsService;
        }
    }
}
