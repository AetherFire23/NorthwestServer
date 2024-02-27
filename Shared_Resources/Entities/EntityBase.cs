using Shared_Resources.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities;
public abstract class EntityBase : IEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();


}