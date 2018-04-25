using System;
using System.ComponentModel.DataAnnotations;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class ContributorPresence : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Contributor { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
}