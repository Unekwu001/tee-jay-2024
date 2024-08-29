using Data.Enums;


namespace Data.Models
{
    public class Attendant
    {   
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public FamilyMemberEnum Family { get; set; }
        public RelationshipTypeEnum Relationship { get; set; }
        public string PhoneNumber {  get; set; }
        public string Email { get; set; }
        public string Url { get; set; }
        public string? TokenId { get; set; }
    }
}
