using System.Linq;
using Hunter.DataAccess.Db.Base;
using Hunter.DataAccess.Entities;
using Hunter.DataAccess.Interface;
using Hunter.DataAccess.Interface.Base;

namespace Hunter.DataAccess.Db
{
    public class CardRepository : Repository<Card>, ICardRepository
    {
        public CardRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {

        }

        public Card GetByCandidateAndVacancy(int candidateId, int vacancyId)
        {
            return Query().FirstOrDefault(x => x.CandidateId == candidateId && x.VacancyId == vacancyId);
        }
    }
}
