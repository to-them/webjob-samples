using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;

namespace SimpleConsoleWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main(string[] args)
        {
            JobHostConfiguration config = new JobHostConfiguration();
            config.StorageConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString; //"Your_Azure_Storage_ConnectionString";
            config.DashboardConnectionString = ConfigurationManager.ConnectionStrings["AzureWebJobsDashboard"].ConnectionString; //"Your_Azure_Storage_ConnectionString";

            JobHost host = new JobHost(config);
            host.CallAsync(typeof(Functions).GetMethod("ProcessMethod"));
            host.RunAndBlock();

            //var host = new JobHost();
            //host.CallAsync(typeof(Functions).GetMethod("ProcessMethod"));
            //host.RunAndBlock();

            //var config = new JobHostConfiguration();

            //if (config.IsDevelopment)
            //{
            //    config.UseDevelopmentSettings();
            //}

            //var host = new JobHost(config);
            //// The following code ensures that the WebJob will be running continuously
            //host.RunAndBlock();

        }
    }
}
