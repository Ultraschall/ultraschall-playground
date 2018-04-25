using System;

namespace Ultraschall.Data.Entities
{
    public class AnnotationBase : TagBase
    {
        public string Label { get; set; }
        public string Language { get; set; }
        public string Uri { get; set; }
    }
}