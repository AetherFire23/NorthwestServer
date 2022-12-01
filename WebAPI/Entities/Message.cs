using System.ComponentModel.DataAnnotations;

namespace WebAPI.Db_Models
{
    public class Message
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public Guid GameId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime? Created { get; set; }
    }
}