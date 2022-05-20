Nitsan, some notes about the code:
1. Things I didn't have time to complete: graceful exit on server side, and using port 8080.
2. You requested this to be an API, but it makes more sense to be a middlewere (on the way to another API which actually does something. WeatherForecastController is exactly that api). But in order to satisfy the exact requirement i do have a working version of it as an API if you'd like to see.
3. Note that for production-grade code I'd use performance automation tests, not a client tool which is more manual.

------------ Below, actual readme --------------

2 C# solutions:
1. A console app to simulate DOS attacks by repeatedly requesting the same HttpRequest
2. A server to handle such requested and to throw 503 error if requests are done from same clientID too much (=more then 5 requests in 5 seconds timeframe)
To run:
Launch 2 VS instances, for client and for server and hit F5. 
You'll see that from time to time server returns 503 error.

