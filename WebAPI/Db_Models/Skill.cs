using System.ComponentModel.DataAnnotations;
using WebAPI.Enums;

namespace WebAPI.Db_Models
{
    public class Skill
    {
        [Key]
        public Guid OwnerId { get; set; }
        public SkillType SkillType { get; set; }
    }
}
