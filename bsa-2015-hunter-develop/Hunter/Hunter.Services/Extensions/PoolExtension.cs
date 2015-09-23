using System.Collections.Generic;
using System.Linq;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Entities.Enums;

namespace Hunter.Services.Extensions
{
    public static class PoolExtension
    {
        public static PoolViewModel ToPoolViewModel(this Pool pool)
        {
            return new PoolViewModel()
            {
                Id = pool.Id,
                Name = pool.Name,
                Color = pool.Color,
                PoolBackground = PoolBackground.BgColors
            };
        }

        public static IEnumerable<PoolViewModel> ToPoolViewModel(this IEnumerable<Pool> pools)
        {
            return pools.Select(p => new PoolViewModel()
            {
                Id = p.Id,
                Name = p.Name,
                Color = p.Color,
                PoolBackground = PoolBackground.BgColors
            }).ToList();
        }

        public static Pool ToPoolModel(this PoolViewModel poolView)
        {
            return new Pool
            {
                Id = poolView.Id,
                Name = poolView.Name,
                Color = poolView.Color,
                Vacancy = new List<Vacancy>(),
                Candidate = new List<Candidate>()
            };
        }
    }
}