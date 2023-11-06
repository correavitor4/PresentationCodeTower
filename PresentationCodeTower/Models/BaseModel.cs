namespace PresentationCodeTower.Models
{
    public class BaseModel
    {
        // Id is a randem int
        public int Id { get; set; } = new Random().Next(1, int.MaxValue);
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
