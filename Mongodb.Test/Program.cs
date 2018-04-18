﻿using Mongodb.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongodb.Test
{
    class Program
    {
        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan timeSpanBengin = new TimeSpan(dateBegin.Ticks);
            TimeSpan timeSpanEnd = new TimeSpan(dateEnd.Ticks);
            TimeSpan timeSpan = timeSpanBengin.Subtract(timeSpanEnd).Duration();
            return timeSpan.TotalMilliseconds.ToString();
        }

        static void Main(string[] args)
        {
            string serverHost = "mongodb://sure:HUANGsl@localhost/MongodbStudy";
            string databaseName = "MongodbStudy";
            string collectionName = "MongodbStudy";
            DateTime bengin = DateTime.Now;
            Console.WriteLine("开始执行....");
            List<Details> listDetails = new List<Details>();
            //for (int i = 1; i <= 10000000; i++)
            //{
            //    listDetails.Add(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });
            //    new MongodbHelper<Details>(serverHost, databaseName, collectionName).InsertBatch(listDetails);
            //}
                   
            //List<Details> list = new MongodbHelper<Details>(serverHost, databaseName, collectionName).FindAll();
            DateTime end = DateTime.Now;

            Console.WriteLine("耗时：" + ExecDateDiff(bengin, end));
        }
    }

    public class Details
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { set; get; }
        public string name { set; get; }
        public int age { set; get; }
        public string sex { set; get; }

    }
}
