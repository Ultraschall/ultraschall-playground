using System;
using Ultraschall.Domain.Abstractions;

namespace Ultraschall.Domain.Models
{
    public class CategoryReferene : IKeyReference
    {
        public Guid Id { get; set; }
    }
    
    public class CategoryModel : IModel
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        
        public IKeyReference Parent { get; set; }
    }
}
