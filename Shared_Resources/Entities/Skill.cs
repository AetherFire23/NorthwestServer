using Shared_Resources.Entities;
using Shared_Resources.DTOs;
using Shared_Resources.Enums;
using System.ComponentModel.DataAnnotations;

namespace Shared_Resources.Entities
{
    public class Skill
    {
        [Key]
        public Guid OwnerId { get; set; }
        public SkillType SkillType { get; set; }
    }
}
