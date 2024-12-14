using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.Covid19Chart.API.Hubs;

namespace SignalR.Covid19Chart.API.Models
{
    public class CovidService(AppDbContext context, IHubContext<CovidHub> hubContext)
    {

        public IQueryable<Covid> GetList()
        {
            return context.Covids.AsQueryable();
        }

        public async Task SaveCovid(Covid covid)
        {
            await context.Covids.AddAsync(covid);
            await context.SaveChangesAsync();
            await hubContext.Clients.All.SendAsync("ReceiveCovidList", GetCovidChartList());
        }

        public List<CovidChart> GetCovidChartList()
        {
            List<CovidChart> covidCharts = new();
            using (var command = context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = @"SELECT TARIH, [1], [2], [3], [4], [5] FROM
                                        (SELECT [City], [Count],CAST([CovidDate] AS DATE) AS TARIH FROM Covids) as CT
                                        Pivot
                                        (Sum(Count) for City IN([1], [2], [3], [4], [5])) as PT
                                        order by TARIH asc";

                command.CommandType = System.Data.CommandType.Text;
                context.Database.OpenConnection();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CovidChart covidChart = new();
                        covidChart.CovidDate = reader.GetDateTime(0).ToShortDateString();
                        Enumerable.Range(1,5).ToList().ForEach(x =>
                        {
                            if (System.DBNull.Value.Equals(reader[x]))
                            {
                                covidChart.Counts.Add(0);
                            }
                            else
                            {
                                covidChart.Counts.Add(reader.GetInt32(x));
                            }
                        });
                        covidCharts.Add(covidChart);
                    }

                }
                context.Database.CloseConnection();
                return covidCharts;
            }
        }
    }
}
