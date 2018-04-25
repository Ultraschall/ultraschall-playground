using System;
using System.ComponentModel.DataAnnotations;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class Category : IEntity
    {
        [Key]        
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public Category Parent { get; set; }
    }
}
