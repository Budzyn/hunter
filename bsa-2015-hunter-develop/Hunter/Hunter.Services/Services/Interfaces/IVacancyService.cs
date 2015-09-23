using System.Collections.Generic;
using Hunter.Services.Dto.Vacancy;

namespace Hunter.Services.Interfaces
{
    public interface IVacancyService
    {
        PageDto<VacancyRowDto> Get(VacancyFilterParams filterParams);
        IList<VacancyRowDto> Get();
        VacancyDto Get(int id);
        void Add(VacancyDto dto);
        void Update(VacancyDto entity);
        void Delete(int id);
        VacancyLongListDto GetLongList(int id);
        IEnumerable<AddedByDto> GetLongListAddedBy(int id);
        IEnumerable<AddedByDto> GetVacanciesAddedBy();

        IEnumerable<VacancyByStateDto> GetVacancyByState(int id);
        Dictionary<string, string> GetColors(int vacancyId);
    }
}
