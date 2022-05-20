using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace DOSAttacker
{
    class Caller
    {
        private readonly string _url;

        private volatile bool _shouldKeepCalling = true;

        private System.Timers.Timer _timer = new System.Timers.Timer();
        private static Random _random = new Random((int)DateTime.UtcNow.Ticks);
        private Task _currentTask;
        private HttpClient _client;

        public Caller(int callerId, HttpClient client)
        {
            _client = client;
            _url = String.Format(Consts.fullUrlTemplate, callerId);
            _timer.Interval = _random.Next(Consts.minNextCallIntervalMs, Consts.maxNextCallIntervalMs);
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = false;
            _timer.Start();
        }
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _currentTask = Call();
            if (_shouldKeepCalling)
            {
                _timer.Interval = _random.Next(Consts.minNextCallIntervalMs, Consts.maxNextCallIntervalMs);
                _timer.Start();
            }
        }

        private Task Call()
        {
            return Task.Run(async () =>
            {
                var id = Task.CurrentId; //easier to understand the logs
                log(id,$"Calling {_url}");
                try
                {                    
                    var response = await _client.GetAsync(_url);                    
                    log(id, $"  got response {response.StatusCode}");
                }
                catch (Exception ex)
                {
                    log(id, $"ERROR {ex.Message}, {ex.InnerException?.Message}");
                }
                
            });
        }

        private void log(int? id, string message)
        {
            Console.WriteLine($"[{DateTime.Now.ToString("h:mm:ss.fff")}] TaskId={id} {message}");
        }

        public Task Stop()
        {
            _shouldKeepCalling = false;
            return _currentTask;
        }
    }
}