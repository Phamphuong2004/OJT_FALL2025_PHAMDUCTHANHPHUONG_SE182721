using System;

namespace GameStoreMini.Models
{
    public interface ITimestamped
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
