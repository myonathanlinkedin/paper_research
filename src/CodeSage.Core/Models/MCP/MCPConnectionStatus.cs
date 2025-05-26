using System;

namespace CodeSage.Core.Models.MCP
{
    public class MCPConnectionStatus
    {
        public bool IsConnected { get; set; }
        public DateTime LastConnectionTime { get; set; }
        public string? ConnectionId { get; set; }
        public string? ServerVersion { get; set; }
        public ConnectionState State { get; set; }
        public string? ErrorMessage { get; set; }
        public Dictionary<string, object> ConnectionDetails { get; set; } = new();
    }
} 