using Mongodb.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            List<Details> aggregate = new List<Details>();

            //单条新增
            //for (int i = 1; i <= 1000; i++)
            //{
            //    monodbHelper.Insert(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });
            //}

            //var resul = monodbHelper.Update(i => i.id == "5ad83a1268869e121c56558e", new Details() { id = "5ad83a1268869e121c56558e", name = $"黄森霖-888", age = 888,sex="男" });

            //listDetails.Add(new Details() { name = $"黄森霖-777", age = 77, sex = 3 % 2 == 0 ? "男" : "女" });

            //var model = new Details() { name = $"黄森霖-777", age = 77, sex = 3 % 2 == 0 ? "男" : "女" };

            //修改
            //monodbHelper.Update(i => i.id == "5ad83a1268869e121c56558e", model);

            //删除
            //monodbHelper.Delete(x => x.name.Contains("黄森霖"));

            //List<int> aggregate = new List<int>();
            //int frequency = 20000000;
            //int forfrequency = 30000;
            //int lastIndex = frequency % forfrequency;
            //int forIndex = (frequency - lastIndex) / forfrequency;

            //int index = 1;

            //for (int i = 1; i <= frequency; i++)
            //{
            //    //没有剩余
            //    if (lastIndex == 0)
            //    {
            //        aggregate.Add(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });

            //        if (aggregate.Count == forfrequency)
            //        {
            //            index++;
            //            Console.WriteLine($"OK{index - 1}");
            //            monodbHelper.InsertBatch(aggregate);
            //            aggregate.Clear();
            //            continue;
            //        }
            //    }

            //    //有剩余
            //    if (lastIndex > 0)
            //    {
            //        if (i == 1)
            //            forIndex = forIndex + 1;
            //        aggregate.Add(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });
            //        if (aggregate.Count == forfrequency && index <= forIndex - 1)
            //        {
            //            index++;
            //            Console.WriteLine($"OK{index - 1}");
            //            monodbHelper.InsertBatch(aggregate);
            //            aggregate.Clear();
            //            continue;
            //        }
            //        if (aggregate.Count == lastIndex && index == forIndex)
            //        {
            //            Console.WriteLine($"OK{index}");
            //            monodbHelper.InsertBatch(aggregate);
            //            aggregate.Clear();
            //            continue;
            //        }
            //    }

            //}
            int pageCount = 0;
            Expression<Func<Details, bool>> func = i => i.sex.Equals("男");
            Func<Details, int> funcOrderby = i => i.age;
            var result = monodbHelper.FindAll(out pageCount, func, i => i.age);

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
