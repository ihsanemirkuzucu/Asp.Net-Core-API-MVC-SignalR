﻿// See https://aka.ms/new-console-template for more information

using Microsoft.AspNetCore.SignalR.Client;
using SignalR.Client.ConsoleApp;

Console.WriteLine("SignalR Console Client");
var connection = new    HubConnectionBuilder().WithUrl("https://localhost:7068/exampleTypeSafeHub").Build();

connection.StartAsync().ContinueWith((result) =>
{
    Console.WriteLine(result.IsCompletedSuccessfully ? "Connected" : "Connected Fail");
});

connection.On<Product>("ReceiveTypedMessageForAllClient", (product) =>
{
    Console.WriteLine($"Received message: {product.Id}--{product.Name}--{product.Price}");
});

while (true)
{
    var key = Console.ReadLine();
    if(key == "exit") break;
    var newProduct = new Product(200, "Pen 5", 140);
    await connection.InvokeAsync("BroadcastTypedMessageToAllClient", newProduct);

}