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

            try
            {
                ReadFiles(path);//TODO - Make this method asyncronious
            }
            catch (Exception ex)
            {
                throw;
                //Console.WriteLine("A critical error occured. Please contact your Systems Administrator : Error " + ex.Message);
            }
        }



        private static void ReadFiles(string filePath)
        {
            //check all files
            string[] fileArray = Directory.GetFiles(filePath, "*.json", SearchOption.AllDirectories);

            foreach (var file in fileArray)
            {
                string files = File.ReadAllText(file);
                string fileName = GetLastParts(file, "\\", 1);

                var jsonData = JsonConvert.DeserializeObject<RootObject>(files);

                string connectionString = jsonData.ConnectionString;
                string table = jsonData.Table;

                string setWhereString = " where ";

                string updateValues = string.Empty;

                int j = 0;
                foreach (var group in jsonData.Groups)
                {
                    string concatenator = "";
                    int i = 0;
                    foreach (var criteria in group.Criterias)
                    {
                        concatenator = (i + 1 < group.Criterias.Length) ? group.Criterias[i + 1].Concatenator : "";//Chec

                        string whereClause = string.Format(" {0} {1} '{2}' {3}", criteria.Field, criteria.Operator, criteria.Value, concatenator);
                        setWhereString += whereClause;
                        i++;
                    }

                    concatenator = (j + 1 < group.Criterias.Length && j + 1 < jsonData.Groups.Length) ? jsonData.Groups[j + 1].Concatenator : "";
                    setWhereString += string.Format(" {0} ", concatenator);

                    j++;
                }

                j = 0;
                foreach (var item in jsonData.PropertyValues)
                {

                    var comma = (j + 1 < jsonData.PropertyValues.Length) ? "," : "";

                    string values = string.Format(" {0} = '{1}' {2} ", item.Column, item.Value, comma);

                    updateValues += values;
                    j++;
                }


                try
                {
                    var query = string.Format("Update {0} set {1} {2} ;", table, updateValues, setWhereString);

                    using (SqlConnection conn = new SqlConnection(connectionString))
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
