using System;

namespace Ultraschall.Domain.Abstractions
{
    public interface IKeyReference
    {
        Guid Id { get; set; }
    }
}