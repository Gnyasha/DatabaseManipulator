﻿using System;
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
using System.Collections;
using Json.Net;

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
                Console.WriteLine("A critical error occured. Please contact your Systems Administrator : Error " + ex.Message);
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


                var messages = jsonData.data.Columns;
                var connString = jsonData.data.ConnectionString;
                foreach (var item in messages)
                {
                    string table = jsonData.data.Table;
                    //string column = item.Column;
                    //string value = item.Value;

                    try
                    {
                        string updateCols = string.Empty;

                        foreach (var col in jsonData.data.Columns)
                        {
                            updateCols += col.Values.Split(':')[0]+" ";
                        }
                        Console.WriteLine(updateCols);
                        //var query = string.Format("Update {0} set({1}) Values ('{2}');", table, column, value);

                        //using (SqlConnection conn = new SqlConnection(connString))
                        //using (SqlCommand cmd = new SqlCommand(query, conn))
                        //{
                        //    conn.Open();
                        //    cmd.ExecuteNonQuery();
                        //    conn.Close();
                        //}

                        Console.WriteLine("Message Processed Successfully");

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("An error occured while updating data");
                    }

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
