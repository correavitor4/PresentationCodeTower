namespace PresentationCodeTower.Models
{
    public class User : BaseModel
    {
        public string Name { get; set; }
        
        public User(string Name)
        {
            this.Name = Name;
            this.CreatedAt = DateTime.Now;
        }
       
    }
}