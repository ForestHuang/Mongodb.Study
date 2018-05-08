using Mongodb.Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mongodb.Test
{
    class Program
    {

        public static void CenterConsole()
        {
            IntPtr hWin = GetConsoleWindow();
            RECT rc;
            GetWindowRect(hWin, out rc);
            Screen scr = Screen.FromPoint(new Point(rc.left, rc.top));
            int x = scr.WorkingArea.Left + (scr.WorkingArea.Width - (rc.right - rc.left)) / 2;
            int y = scr.WorkingArea.Top + (scr.WorkingArea.Height - (rc.bottom - rc.top)) / 2;
            MoveWindow(hWin, x, y, rc.right - rc.left, rc.bottom - rc.top, true);
        }
        private struct RECT { public int left, top, right, bottom; }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT rc);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool MoveWindow(IntPtr hWnd, int x, int y, int w, int h, bool repaint);

        public static string ExecDateDiff(DateTime dateBegin, DateTime dateEnd)
        {
            TimeSpan timeSpanBengin = new TimeSpan(dateBegin.Ticks);
            TimeSpan timeSpanEnd = new TimeSpan(dateEnd.Ticks);
            TimeSpan timeSpan = timeSpanBengin.Subtract(timeSpanEnd).Duration();
            return timeSpan.TotalSeconds.ToString();
        }

        static SemaphoreSlim _sem = new SemaphoreSlim(3, 3);
        static void Main(string[] args)
        {

            System.Diagnostics.Debug.WriteLine("输出到VS输出窗口");

            CenterConsole();
            //Console.WriteLine($"Main Start -- Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            //var aa = TestAsync();
            //Console.WriteLine("jg " + aa);
            //Console.WriteLine($"Main End -- Thread Id: {Thread.CurrentThread.ManagedThreadId}");
            var task = new Task[15];
            for (int i = 1; i <= 15; i++)
                task[i - 1] = Task.Run(() => Enter(i));
            Task.WaitAll(task);
            Console.ReadLine();
        }


        static void Enter(object id)
        {
            ConsoleWriteLine($"[{Task.CurrentId}] -- 开始排队...", ConsoleColor.Green);
            _sem.Wait();

            ConsoleWriteLine($"[{Task.CurrentId}] -- 开始执行！", ConsoleColor.Green);
            Thread.Sleep(1000 * (int)Task.CurrentId);

            ConsoleWriteLine($"[{Task.CurrentId}] -- 执行完毕，离开！", ConsoleColor.Red);
            _sem.Release();
        }

        public static void ConsoleWriteLine(string msg, ConsoleColor forecolor = ConsoleColor.Red, ConsoleColor backcolor = ConsoleColor.Black)
        {
            Console.Title = "senlin.huang";
            Console.ForegroundColor = forecolor;
            Console.BackgroundColor = backcolor;
            Console.WriteLine(msg);
        }

        static int TestAsync()
        {
            var name = Sum();   //我们这里没有用 await,所以下面的代码可以继续执行
            Console.WriteLine(name.Result);
            return name.Result;
        }


        static async Task<int> Sum()
        {
            return await Task.Run(() =>
            {
                Console.WriteLine($"Sum() -- Start -- Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                int sum = 0;
                for (int i = 1; i <= 100000; i++) { sum += i; }
                Console.WriteLine($"Sum() -- End -- Thread Id: {0}", Thread.CurrentThread.ManagedThreadId);
                return sum;
            });
        }


        //static void Main(string[] args)
        //{
        //    string serverHost = "mongodb://sure:HUANGsl@localhost/Sure_mongodbStudy";
        //    string databaseName = "Sure_mongodbStudy";
        //    string collectionName = "Sure_mongodbStudy";
        //    DateTime bengin = DateTime.Now;
        //    Console.WriteLine("开始执行....");
        //    var monodbHelper = new MongodbHelper<Details>(serverHost, databaseName, collectionName);

        //    List<Details> aggregate = new List<Details>();

        //    //单条新增
        //    //for (int i = 1; i <= 1000; i++)
        //    //{
        //    //    monodbHelper.Insert(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });
        //    //}

        //    //var resul = monodbHelper.Update(i => i.id == "5ad83a1268869e121c56558e", new Details() { id = "5ad83a1268869e121c56558e", name = $"黄森霖-888", age = 888,sex="男" });

        //    //listDetails.Add(new Details() { name = $"黄森霖-777", age = 77, sex = 3 % 2 == 0 ? "男" : "女" });

        //    //var model = new Details() { name = $"黄森霖-777", age = 77, sex = 3 % 2 == 0 ? "男" : "女" };

        //    //修改
        //    //monodbHelper.Update(i => i.id == "5ad83a1268869e121c56558e", model);

        //    //删除
        //    //monodbHelper.Delete(x => x.name.Contains("黄森霖"));

        //    //List<int> aggregate = new List<int>();
        //    //int frequency = 20000000;
        //    //int forfrequency = 30000;
        //    //int lastIndex = frequency % forfrequency;
        //    //int forIndex = (frequency - lastIndex) / forfrequency;

        //    //int index = 1;

        //    //for (int i = 1; i <= frequency; i++)
        //    //{
        //    //    //没有剩余
        //    //    if (lastIndex == 0)
        //    //    {
        //    //        aggregate.Add(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });

        //    //        if (aggregate.Count == forfrequency)
        //    //        {
        //    //            index++;
        //    //            Console.WriteLine($"OK{index - 1}");
        //    //            monodbHelper.InsertBatch(aggregate);
        //    //            aggregate.Clear();
        //    //            continue;
        //    //        }
        //    //    }

        //    //    //有剩余
        //    //    if (lastIndex > 0)
        //    //    {
        //    //        if (i == 1)
        //    //            forIndex = forIndex + 1;
        //    //        aggregate.Add(new Details() { name = $"黄森霖-{i}", age = i, sex = i % 2 == 0 ? "男" : "女" });
        //    //        if (aggregate.Count == forfrequency && index <= forIndex - 1)
        //    //        {
        //    //            index++;
        //    //            Console.WriteLine($"OK{index - 1}");
        //    //            monodbHelper.InsertBatch(aggregate);
        //    //            aggregate.Clear();
        //    //            continue;
        //    //        }
        //    //        if (aggregate.Count == lastIndex && index == forIndex)
        //    //        {
        //    //            Console.WriteLine($"OK{index}");
        //    //            monodbHelper.InsertBatch(aggregate);
        //    //            aggregate.Clear();
        //    //            continue;
        //    //        }
        //    //    }

        //    //}
        //    //int pageCount = 0;
        //    //Expression<Func<Details, bool>> func = i => i.sex.Equals("男");
        //    //Func<Details, int> funcOrderby = i => i.age;
        //    //var result = monodbHelper.FindAll(out pageCount, func, i => i.age);

        //    //DateTime end = DateTime.Now;

        //    //Console.WriteLine("耗时：" + ExecDateDiff(bengin, end));
        //}
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
