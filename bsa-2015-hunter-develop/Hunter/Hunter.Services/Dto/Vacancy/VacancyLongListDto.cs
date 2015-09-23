using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services
{
    public class VacancyLongListDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        public string AddedByName { get; set; }
        //public int PoolId { get; set; }
        public IEnumerable<string> PoolNames { get; set; }
        public Dictionary<string, string> PoolColors { get; set; }
    }
}
