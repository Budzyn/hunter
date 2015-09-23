using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Interface
{
    public interface ICardRepository : IRepository<Card>
    {
        Card GetByCandidateAndVacancy(int candidateId, int vacancyId);
    }
}
