using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace DatabaseManipulator
{
    class Program
    {
        static ManipulatorDbContext db = new ManipulatorDbContext();

        static void Main(string[] args)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 3000;
            aTimer.Enabled = true;

            Console.WriteLine("Starting...");
            Console.ReadLine();
        }


        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                //TODO - Make this method asyncronious
                ReadFiles();
            }
            catch (Exception ex)
            {
                Console.WriteLine("A critical error occured. Please contact your Systems Administrator : Error " + ex.Message);
            }
        }

        private static  void ReadFiles()
        {
           //check all files
            string[] fileArray = Directory.GetFiles(@"C:\Users\Goodson\Messages", "*.json", SearchOption.AllDirectories);

          
            foreach (var file in fileArray)
            {
                string files = File.ReadAllText(file);
                string fileName = GetLastParts(file, "\\", 1);
                var saved = db.tblReceivedMessages.Select(a => a.FileName).ToList();

                if (!saved.Contains(fileName)) //check for new files
                {
                    //Add a new entry of the received or dumped file
                    tblReceivedMessage receivedMessage = new tblReceivedMessage();
                    receivedMessage.FileName = fileName;
                    receivedMessage.ReadDate = DateTime.Now;
                    db.tblReceivedMessages.Add(receivedMessage);
                    db.SaveChanges();

                    var receivedUpdate = db.tblReceivedMessages.Where(a => a.FileName == receivedMessage.FileName).FirstOrDefault();

                    var jsonData = JsonConvert.DeserializeObject<RootObject>(files);

                    var messages = jsonData.data.EntityMessage;

                    foreach (var item in messages)
                    {
                        string database = item.Database;
                        string table = item.Table;
                        string column = item.Column;
                        string value = item.Value;
                       
                        try
                        {
                            var query = string.Format("insert into {0}.dbo.{1} ({2}) Values ('{3}');", database, table, column, value);
                            db.Database.ExecuteSqlCommand(query);

                            receivedUpdate.Status = "Message Processed Successfully";
                            receivedUpdate.Success = true;

                        }
                        catch (Exception)
                        {
                            receivedUpdate.Status = "An error occured while updating data";
                            receivedUpdate.Success = false;
                        }
                        db.SaveChanges();
                    }
                    
                    db.SaveChanges();


                }
                Console.WriteLine("Message process completed");

            }

        }

        static string GetLastParts(string text, string separator, int count)
        {
            string[] parts = text.Split(new string[] { separator }, StringSplitOptions.None);
            return string.Join(separator, parts.Skip(parts.Count() - count).Take(count).ToArray());
        }

        

        
    }
}
