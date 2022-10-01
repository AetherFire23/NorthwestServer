using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Models
{
    public class Skill
    {
        [Key]
        public Guid OwnerId { get; set; }
        public SkillType SkillType { get; set; }
    }
}
