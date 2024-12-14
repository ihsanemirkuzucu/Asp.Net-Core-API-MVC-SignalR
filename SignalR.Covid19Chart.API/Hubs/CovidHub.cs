using Microsoft.AspNetCore.SignalR;
using SignalR.Covid19Chart.API.Models;

namespace SignalR.Covid19Chart.API.Hubs
{
    public class CovidHub(CovidService covidService):Hub
    {
        public async Task GetCovidList()
        {
            await Clients.All.SendAsync("ReceiveCovidList", covidService.GetCovidChartList());
        }
    }
}
