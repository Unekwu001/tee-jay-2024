using Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class Attendant: IdentityUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        public FamilyMember Family { get; set; }

        [Required]
        public RelationshipType Relationship { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber {  get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }
    }
}
