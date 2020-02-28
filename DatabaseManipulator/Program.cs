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

using System.Data.SqlClient;
using System.Collections;
using Json.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DatabaseManipulator
{
    class Program
    {

        static string path;
        static void Main(string[] args)
        {
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 5000;
            aTimer.Enabled = true;
            Console.WriteLine("Starting...");

            path = @"C:\Users\Goodson\Messages";

            Console.ReadLine();
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            ReadFiles(path);//TODO - Make this method asyncronious
                            //try
                            //{

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("A critical error occured. Please contact your Systems Administrator : Error " + ex.Message);
            //}
        }



        private static void ReadFiles(string filePath)
        {
            //check all files
            string[] fileArray = Directory.GetFiles(filePath, "*.json", SearchOption.AllDirectories);

            foreach (var file in fileArray)
            {
                string files = File.ReadAllText(file);
                string fileName = GetLastParts(file, "\\", 1);

                //var jsonData = JsonConvert.DeserializeObject<RootObject>(files);
                var json = JObject.Parse(files);



                string connString = json["ConnectionString"].ToObject<string>();
                string table = json["Table"].ToObject<string>();


                string criteriaForUpdate = " where 1=1 ";
                string updateStructure = " ";

                var criterions = json["Criteria"].ToObject<Dictionary<string, string>>().ToList();
                var criteriaOperators = json["CriteriaOperators"].ToObject<Dictionary<string, string>>().ToList();
                var properties = json["PropertyValues"].ToObject<Dictionary<string, string>>().ToList();

                int i = 0;
                foreach (var item in criterions)
                {
                    criteriaForUpdate += String.Format(" and {0} {1} '{2}' ", criterions[i].Key, criteriaOperators[i].Value, criterions[i].Value);
                    i++;
                }

                i = 0;
                foreach (var item in properties)
                {
                    if (i + 1 == properties.Count)
                    {
                        updateStructure += string.Format(" {0} = '{1}' ", properties[i].Key, properties[i].Value);
                    }
                    else
                    {
                        updateStructure += string.Format(" {0} = '{1}', ", properties[i].Key, properties[i].Value);
                    }
                    i++;
                }

                try
                {
                    var query = string.Format("Update {0} set {1} {2} ;", table, updateStructure, criteriaForUpdate);

                    using (SqlConnection conn = new SqlConnection(connString))
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }

                    Console.WriteLine("Message Processed Successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occured while updating data : Message " + ex.Message);

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
