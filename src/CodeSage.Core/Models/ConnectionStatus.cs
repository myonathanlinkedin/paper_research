using System;

namespace CodeSage.Core.Models;

public class ConnectionStatus
{
    public bool IsConnected { get; set; }
    public DateTime LastConnected { get; set; }
    public DateTime LastDisconnected { get; set; }
    public TimeSpan Uptime => IsConnected ? DateTime.UtcNow - LastConnected : TimeSpan.Zero;
    public string Status { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public int RetryCount { get; set; }
    public int MaxRetries { get; set; } = 3;
    public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);

    public ConnectionStatus()
    {
        IsConnected = false;
        LastConnected = DateTime.MinValue;
        LastDisconnected = DateTime.UtcNow;
    }

    public void Connected()
    {
        IsConnected = true;
        LastConnected = DateTime.UtcNow;
        Status = "Connected";
        ErrorMessage = string.Empty;
        RetryCount = 0;
    }

    public void Disconnected(string errorMessage = "")
    {
        IsConnected = false;
        LastDisconnected = DateTime.UtcNow;
        Status = "Disconnected";
        ErrorMessage = errorMessage;
        RetryCount++;
    }
} 