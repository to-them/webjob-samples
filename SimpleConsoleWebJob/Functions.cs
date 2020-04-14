using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryEmailer;
using Microsoft.Azure.WebJobs;

namespace SimpleConsoleWebJob
{
    public class Functions
    {
        [NoAutomaticTrigger]
        public static async Task ProcessMethod(TextWriter log)
        {
            // Do something.      
            string emsg = SendEmail();
            Console.WriteLine(emsg);
            log.WriteLine(emsg); 
            log.WriteLine("This ProcessMethod method is executed");
            Console.WriteLine("This ProcessMethod method is executed");

            await Task.Delay(TimeSpan.FromMinutes(1));
        }

        //// This function will get triggered/executed when a new message is written 
        //// on an Azure Queue called queue.
        //public static void ProcessQueueMessage([QueueTrigger("queue")] string message, TextWriter log)
        //{
        //    log.WriteLine(message);
        //}

        public static string SendEmail()
        {
            string emailToList = "services@ayitech.com";
            Emailer em = new Emailer("donotreply@email.com", emailToList, "Test subject", "Test message", "", "");
            return em.SendEmail();
        }
    }

    
}
