using System;

namespace Ultraschall.Data.Abstractions
{
    public interface IEntity
    {
        Guid Id { get; }
    }
}