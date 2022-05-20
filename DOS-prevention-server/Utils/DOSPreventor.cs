using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DOS_prevention_server.Utils
{
    public class DOSPreventor
    {
        private class RequestData
        {
            public int count;
            public DateTime time;

            public RequestData(int count, DateTime time)
            {
                this.count = count;
                this.time = time;
            }
            public override string ToString()
            {
                return $"{time.ToString("mm:ss.fff")} / {count}";
            }
        }

        private static readonly ConcurrentDictionary<int, RequestData> _requestsCount = new ConcurrentDictionary<int, RequestData>();

        public static bool ShouldAllowRequest(IQueryCollection queryCol)
        {
            int clientId;
            if (!Int32.TryParse(queryCol["clientId"], out clientId)) return false;

            var initialReqData = new RequestData(1, DateTime.Now);
            var reqData = _requestsCount.GetOrAdd(clientId, initialReqData);
            if (reqData == initialReqData) return true;

            var secsFrom1stRequest = (DateTime.Now - reqData.time).TotalSeconds;
            if (secsFrom1stRequest >= Consts.maxTimeFrameSeconds)
            {
                _requestsCount[clientId] = initialReqData;
                return true;
            }
            else
            {
                if (reqData.count < Consts.maxRequestsInTimeFrame)
                {
                    reqData.count += 1;
                    _requestsCount[clientId] = reqData;
                    return true;
                }
                return false;
            }
        }
    }
}
