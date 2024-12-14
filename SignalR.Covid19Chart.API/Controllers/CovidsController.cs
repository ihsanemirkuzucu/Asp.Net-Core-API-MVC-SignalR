using Microsoft.AspNetCore.Mvc;
using SignalR.Covid19Chart.API.Models;

namespace SignalR.Covid19Chart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidsController(CovidService covidService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> SaveCovid(Covid covid)
        {
            await covidService.SaveCovid(covid);
            IQueryable covidList = covidService.GetList();
            return Ok(covidService.GetCovidChartList());
        }

        [HttpGet]
        public IActionResult InitializeCovid()
        {
            Random random = new();
            Enumerable.Range(1, 10).ToList().ForEach(x =>
            {
                foreach (ECity item in Enum.GetValues(typeof(ECity)))
                {
                    var newCovid = new Covid
                    {
                        City = item,
                        Count = random.Next(100, 1000),
                        CovidDate = DateTime.Now.AddDays(x)
                    };
                    covidService.SaveCovid(newCovid).Wait();
                    //Thread.Sleep(1000);
                }
            });
            return Ok("Covid 19 dataları veritabanına kaydedildi");
        }
    }
}
