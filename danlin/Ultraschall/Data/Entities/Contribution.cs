using System;
using System.ComponentModel.DataAnnotations;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class Contribution : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public ContributorPresence Presence { get; set; }
        public ContributorRole Role { get; set; }
    }
}