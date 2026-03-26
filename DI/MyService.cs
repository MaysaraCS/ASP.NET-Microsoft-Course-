using System;
public class MyService : IMyService
{
    private readonly int _serviceId;
    public MyService()
    {
        _serviceId = new Random().Next(1000, 9999);
    }
    public void LogCreation(string message)
    {
        Console.WriteLine($"Service ID: {_serviceId} - {message}");
    }
}