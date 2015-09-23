using Hunter.DataAccess.Entities.Entites.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hunter.Services.Dto
{
    public class ActivityFilterDto
    {
        public int FilterId { get; set; }
        public int OptionId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
