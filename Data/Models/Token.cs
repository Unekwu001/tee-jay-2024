namespace Data.Models
{
    public class Token
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public bool IsUsed { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
