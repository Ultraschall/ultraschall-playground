using System;
using System.Collections.Generic;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class Podcast : IEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public string Summary { get; set; }
        public string Language { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public Contributor Owner { get; set; }
        public List<Category> Categories { get; set; }
    }
}