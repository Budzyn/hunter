using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Hunter.DataAccess.Entities.Entites.Enums;

namespace Hunter.DataAccess.Entities
{
    [Table("Feedback")]
    public partial class Feedback : IEntity
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Feedback()
        {
            Test = new HashSet<Test>();
        }

        public int Id { get; set; }

        public int CardId { get; set; }

        public int? ProfileId { get; set; }

        [Required]
        [StringLength(3000)]
        public string Text { get; set; }

        public DateTime Added { get; set; }

        public DateTime? Edited { get; set; }

        public int Type { get; set; }

        public int Status { get; set; }

        public SuccessStatus SuccessStatus { get; set; }

        public virtual Card Card { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Test> Test { get; set; }
    }
}
