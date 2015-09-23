using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hunter.DataAccess.Entities.Enums;

namespace Hunter.DataAccess.Entities
{
    [Table("Candidate")]
    public partial class Candidate : BaseSoftDeleteEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Candidate()
        {
            Card = new HashSet<Card>();
            Pool = new HashSet<Pool>();

            Resume = new HashSet<Resume>();
            SpecialNote = new HashSet<SpecialNote>();
            Origin = Origin.Sourced;
            Resolution = Resolution.None;
            Shortlisted = false;
        }

        [Column(TypeName = "image")]
        public byte[] Photo { get; set; }

        [Column(TypeName="date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(300)]
        public string FirstName { get; set; }

        [StringLength(300)]
        public string LastName { get; set; }

        [StringLength(300)]
        public string Email { get; set; }

        [StringLength(300)]
        public string CurrentPosition { get; set; }

        [StringLength(300)]
        public string Company { get; set; }

        [StringLength(300)]
        public string Location { get; set; }

        [StringLength(300)]
        public string Skype { get; set; }

        [StringLength(300)]
        public string Phone { get; set; }

        [StringLength(300)]
        public string Linkedin { get; set; }

        public double? Salary { get; set; }

        public double? YearsOfExperience { get; set; }

        public int? AddedByProfileId { get; set; }

        public string ResumeSummary { get; set; }

        public virtual ICollection<Resume> Resume { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public DateTime AddDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EditDate { get; set; }

        public Origin Origin { get; set; }

        public Resolution Resolution { get; set; }

        public bool Shortlisted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Card> Card { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pool> Pool { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SpecialNote> SpecialNote { get; set; }

        public double CalculateYearsOfExperiance()
        {
            TimeSpan gap = DateTime.Now - EditDate;

            double years = Math.Round(((double)gap.Days) / 365, 1);

            return years;
        }
    }
}
