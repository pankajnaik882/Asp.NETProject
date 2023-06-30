namespace TaskParallelDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var source = new CancellationTokenSource();

            try
            {
                var t1 = Task.Factory.StartNew(() => { DoSomeVeryImportantTask(1, 1500 , source.Token); }).ContinueWith((prevTask) => DoSomeOtherVeryImportantTask(1, 1000));
                source.Cancel();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType());
            }
            

            //var t2 = Task.Factory.StartNew(() => {  DoSomeVeryImportantTask(2, 3000); });


            //var t3 = Task.Factory.StartNew(() => { DoSomeVeryImportantTask(3, 1000); });


            //var tasklist = new List<Task>() { t1,t2,t3};
            //Task.WaitAll(tasklist.ToArray());

            //for (var i = 0; i < 10;i++)
            //{
            //    Console.WriteLine("Doing some other work");
            //    Thread.Sleep(250);
            //    Console.WriteLine("i - {0}", i);
            //}

            //var intlist = new List<int>() { 10,20,30,5,7,4,8,45,76,87,23,57,3,1};

            //Parallel.ForEach(intlist, (i) => Console.WriteLine(i));

            Console.WriteLine("Press any key to Quit");
            Console.ReadKey();
        }

        static void DoSomeVeryImportantTask(int id, int sleeptime, CancellationToken token)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Cancellation Requested");
                token.ThrowIfCancellationRequested();
            }
            Console.WriteLine("Task {0} is  beginning", id);
            Thread.Sleep(sleeptime);
            Console.WriteLine("Task {0} end", id);
        }

        static void DoSomeOtherVeryImportantTask(int id, int sleeptime)
        {
            Console.WriteLine("Task {0} is  beginning other work", id);
            Thread.Sleep(sleeptime);
            Console.WriteLine("Task {0} end other work", id);
        }
    }
}
