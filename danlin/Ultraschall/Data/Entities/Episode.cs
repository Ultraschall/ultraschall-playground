using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class Episode : IEntity
    {
        public enum EpisodeType
        {
            Full = 1,
            Trailer = 2,
            Bonus = 3
        }
        
        [Key]
        public Guid Id { get; set; }
        public int Sequence { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Link { get; set; }
        public string PublicationDate { get; set; }
        public string Guid { get; set; }
        public int Duration { get; set; }
        public EpisodeType Type { get; set; }
        public List<Contribution> Contributions { get; set; }
    }
}