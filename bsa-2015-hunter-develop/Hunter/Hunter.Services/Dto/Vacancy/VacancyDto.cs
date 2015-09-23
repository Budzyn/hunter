using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Hunter.Services
{
    public class VacancyDto
    {
        public VacancyDto()
        {
            //Card = new HashSet<Card>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Name { get; set; }

        public int Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(4000)]
        public string Description { get; set; }

//        public IEnumerable<PoolViewModel> Pools { get; set; }
        public IEnumerable<string> PoolNames { get; set; }
        public Dictionary<string, string> PoolColors { get; set; }

        public string StatusName { get; set; }

        public string UserLogin { get; set; }

        //public virtual ICollection<CardDTO> Card { get; set; }

        //public virtual PoolDTO Pool { get; set; }
    }

    public class VacancyRowDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

//        public IEnumerable<PoolViewModel> Pools { get; set; }
        public IEnumerable<string> PoolNames { get; set; }
        public Dictionary<string, string> PoolColors { get; set; }

        public string AddedByName { get; set; }

        public int AddedById { get; set; }
    }

    public class PageDto<T> where T: class, new()
    {
        public int TotalCount { get; set; }

        public IList<T> Rows { get; set; }
    }
}
