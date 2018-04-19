using Mongodb.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
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
            return timeSpan.TotalSeconds.ToString();
        }

        static void Main(string[] args)
        {
            string serverHost = "mongodb://sure:HUANGsl@localhost/Sure_mongodbStudy";
            string databaseName = "Sure_mongodbStudy";
            string collectionName = "Sure_mongodbStudy";
            DateTime bengin = DateTime.Now;
            Console.WriteLine("开始执行....");
            var monodbHelper = new MongodbHelper<Details>(serverHost, databaseName, collectionName);

            List<Details> listDetails = new List<Details>();

            //单条新增
            for (int i = 1; i <= 1000; i++)
            {
                //monodbHelper.Insert(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });
            }

            //var resul = monodbHelper.Update(i => i.id == "5ad83a1268869e121c56558e", new Details() { id = "5ad83a1268869e121c56558e", name = $"黄森霖-888", age = 888,sex="男" });

            listDetails.Add(new Details() { name = $"黄森霖-777", age = 77, sex = 3 % 2 == 0 ? "男" : "女" });

            var model = new Details() { name = $"黄森霖-777", age = 77, sex = 3 % 2 == 0 ? "男" : "女" };

            monodbHelper.UpdateOne(i => i.id == "5ad83a1268869e121c56558e", model);

            //修改

            //删除
            //monodbHelper.Delete(x => x.name.Contains("黄森霖"));

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
