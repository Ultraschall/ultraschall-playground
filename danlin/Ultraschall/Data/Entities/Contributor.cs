using System;
using System.ComponentModel.DataAnnotations;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class Contributor : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}