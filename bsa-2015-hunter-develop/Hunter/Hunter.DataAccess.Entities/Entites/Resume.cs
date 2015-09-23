using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hunter.DataAccess.Entities
{
    [Table("Resume")]
    public partial class Resume : IEntity
    {
        public int Id { get; set; }

        public int FileId { get; set; }

        public virtual File File { get; set; }

        public int CandidateId { get; set; }

        public virtual Candidate Candidate { get; set; }

       
    }
}
