using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hunter.DataAccess.Interface;
using Hunter.Services.Interfaces;
using NSubstitute;
using Hunter.DataAccess.Interface.Base;
using Hunter.Common.Interfaces;
using Hunter.Services;
using NUnit.Framework;
using Hunter.DataAccess.Entities;

namespace Hunter.Tests.Services
{
    class CandidateServiceTest
    {
        private readonly ICardRepository _cardRepository;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IPoolRepository _poolRepository;
        private readonly ICandidateService _candidateService;
        private IUserProfileRepository _userProfileRepository;

        public CandidateServiceTest()
        {
            _cardRepository= Substitute.For<ICardRepository>();
            _candidateRepository = Substitute.For<ICandidateRepository>();
            _poolRepository = Substitute.For<IPoolRepository>();
            _userProfileRepository = Substitute.For<IUserProfileRepository>();

            var logger = Substitute.For<ILogger>();
            var activityHelperService = Substitute.For<IActivityHelperService>();

            _candidateService = new CandidateService(_candidateRepository, _cardRepository,
                                                     _poolRepository, logger, _userProfileRepository, activityHelperService);
        }

        [SetUp]
        public void TestSetup()
        {
            var cardsList = new List<Card> {new Card {Id = 1, VacancyId = 1}, new Card {Id = 2, VacancyId = 1}};

            _cardRepository.Query().Returns(cardsList.AsQueryable());

        }

        [Test]
        public void Long_list_for_candidates_Would_correctly_return_When_long_list_is_getted_by_vacancy_id()
        {
            // Arrange

            // Act
            var result = _candidateService.GetLongList(1).Count();

            // Assert
            Assert.AreEqual(2, result);
        }
    }
}
