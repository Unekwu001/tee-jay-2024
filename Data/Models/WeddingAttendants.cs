using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Models
{
    public class WeddingAttendants
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string firstname {  get; set; }

        [Required]
        [StringLength(50)]
        public string lastname { get; set; }

        [Required]
        public FamilyMember Family { get; set; }

        [Required]
        public RelationshipType Relationship { get; set; }

        [Required]
        [Phone]
        public string phoneNumber {  get; set; }

        [Required]
        [EmailAddress]
        public string email { get; set; }

        [Required]
        [Url]
        public string url { get; set; }
    }
}
