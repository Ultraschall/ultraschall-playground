using System;
using Ultraschall.Data.Abstractions;

namespace Ultraschall.Data.Entities
{
    public class Chapter : AnnotationBase
    {
        public TimeSpan Duration { get; set; }
    }
}