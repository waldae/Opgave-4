using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

Console.WriteLine("TCP Server:");

TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7);
listener.Start();
Console.WriteLine("Server started...");

while (true)
{
    TcpClient socket = await listener.AcceptTcpClientAsync();
    IPEndPoint? clientEndPoint = socket.Client.RemoteEndPoint as IPEndPoint;
    if (clientEndPoint != null)
    {
        Console.WriteLine("Client connected: " + clientEndPoint.Address);
    }

    Task.Run(() => HandleClient(socket));
}

async Task HandleClient(TcpClient socket)
{
    NetworkStream ns = socket.GetStream();
    StreamReader reader = new StreamReader(ns);
    StreamWriter writer = new StreamWriter(ns);

    writer.AutoFlush = true;

    while (socket.Connected)
    {
        string? message = await reader.ReadLineAsync();
        Console.WriteLine("Received: " + message);

        if (message == "random number")
        {
            writer.WriteLine("Enter the minimum number:");
            int minNumber = int.Parse(await reader.ReadLineAsync());
            writer.WriteLine("Enter the maximum number:");
            int maxNumber = int.Parse(await reader.ReadLineAsync());

            Random random = new Random();
            int randomNumber1 = random.Next(minNumber, maxNumber + 1);
            int randomNumber2 = random.Next(minNumber, maxNumber + 1);

            writer.WriteLine("Random number results: " + randomNumber1 + ", " + randomNumber2);
        }

        else if (message == "add")
        {
            writer.WriteLine("Enter the first number:");
            int number1 = int.Parse(await reader.ReadLineAsync());
            writer.WriteLine("Enter the second number:");
            int number2 = int.Parse(await reader.ReadLineAsync());

            int sum = number1 + number2;
            writer.WriteLine("Sum: " + sum);
        }

        else if (message == "subtract")
        {
            writer.WriteLine("Enter the first number:");
            int number1 = int.Parse(await reader.ReadLineAsync());
            writer.WriteLine("Enter the second number:");
            int number2 = int.Parse(await reader.ReadLineAsync());

            int difference = number1 - number2;
            writer.WriteLine("Difference: " + difference);
        }

        if (message == "stop")
        {
            writer.WriteLine("Goodbye world");
            socket.Close();
        }
    }
}
