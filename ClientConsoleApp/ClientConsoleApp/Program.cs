// See https://aka.ms/new-console-template for more information
using AlcatrazGrpcService;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;

Console.WriteLine("Hello, World!");

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("http://localhost:5036");
var client = new TimeService.TimeServiceClient(channel);
var reply = await client.GetTimeAsync(new Empty());
Console.WriteLine("Time: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();