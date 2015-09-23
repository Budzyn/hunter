using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hunter.DataAccess.Entities
{
    [Table("Interview")]
    public partial class Interview : IEntity
    {
        public int Id { get; set; }

        public int CardId { get; set; }

        public DateTime StartTime { get; set; }

        [StringLength(3000)]
        public string Comment { get; set; }

        public int? FeedbackId { get; set; }

        public virtual Card Card { get; set; }
    }
}
