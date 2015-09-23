
using System.Collections.Generic;
namespace Hunter.Services.Dto
{
    public class FilterInfoDto
    {
        public IEnumerable<UserProfileDto> Users { get; set; }
        public IEnumerable<PoolViewModel> Pools { get; set; }
    }
}
