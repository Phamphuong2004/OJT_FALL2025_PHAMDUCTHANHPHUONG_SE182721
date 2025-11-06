using System;

public class AuditLog
{
    public int Id { get; set; }
    public string Entity { get; set; } // e.g. "Stock", "Order", "User"
    public int EntityId { get; set; }
    public string Action { get; set; } // e.g. "AdjustStock", "ChangeStatus"
    public string PerformedBy { get; set; } // user id/email
    public string Reason { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string Data { get; set; } // optional JSON
}