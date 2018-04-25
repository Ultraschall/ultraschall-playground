using System;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class Season : IEntity
    {
        public Guid Id { get; set; }
        public int Sequence { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
    }
}