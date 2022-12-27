using System.ComponentModel.DataAnnotations;

namespace WebAPI.Entities
{
    public class Expedition
    {
        [Key]
        public Guid Id { get; set; }


        public Guid GameId { get; set; }

        public string Name { get; set; }

        public Guid LeaderId { get; set; }

        public bool IsAvailableForCreation { get; set; }

        public bool IsCreated { get; set; }




    }
}
