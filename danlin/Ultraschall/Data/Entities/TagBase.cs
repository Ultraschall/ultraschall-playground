using System;
using System.ComponentModel.DataAnnotations;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class TagBase : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Comment { get; set; }
    }
}