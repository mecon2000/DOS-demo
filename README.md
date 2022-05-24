2 C# solutions:
1. A console app to simulate DOS attacks by repeatedly requesting the same HttpRequest
2. A server to handle such requested and to throw 503 error if requests are done from same clientID too much (=more then 5 requests in 5 seconds timeframe)
To run:
Launch 2 VS instances, for client and for server and hit F5. 
You'll see that from time to time server returns 503 error.
