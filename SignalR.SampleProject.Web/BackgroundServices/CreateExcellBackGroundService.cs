using System.Data;
using System.Threading.Channels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.FileProviders;
using NuGet.Packaging.Signing;
using SignalR.SampleProject.Web.Hubs;
using SignalR.SampleProject.Web.Models;

namespace SignalR.SampleProject.Web.BackgroundServices
{
    public class CreateExcellBackGroundService
            (
             Channel<(string userId, List<Product> products)> channel,
             IFileProvider fileProvider,
             IServiceProvider serviceProvider
            ) :BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (await channel.Reader.WaitToReadAsync(stoppingToken))
            {
                var (userId, products) = await channel.Reader.ReadAsync(stoppingToken);
                var wwwrootFolder = fileProvider.GetDirectoryContents("wwwroot");
                var filesFolder = wwwrootFolder.Single(x => x.Name == "Files");
                var newExcelFileName = $"{Guid.NewGuid()}.xlsx";
                var newExcellFilePath = Path.Combine(filesFolder.PhysicalPath, newExcelFileName);

                var workbook = new XLWorkbook();
                var dataset = new DataSet();
                dataset.Tables.Add(GetTable("Product List", products));
                workbook.Worksheets.Add(dataset);
                await using var excellFileStream = new FileStream(newExcellFilePath, FileMode.Create);
                workbook.SaveAs(excellFileStream);

                using (var scope = serviceProvider.CreateScope())
                {
                    var appHub = scope.ServiceProvider.GetRequiredService<IHubContext<AppHub>>();
                    await appHub.Clients.User(userId)
                        .SendAsync("AlertCompleteFile", $"/files/{newExcelFileName}", stoppingToken);
                }
            }
        }

        private DataTable GetTable(string tabloName, List<Product> products)
        {
            var table = new DataTable { TableName = tabloName };
            foreach (var item in typeof(Product).GetProperties()) table.Columns.Add(item.Name, item.PropertyType);

            products.ForEach(x => { table.Rows.Add(x.Id, x.Name, x.Price, x.Description, x.UserId);});

            return table;
        }
    }
}
