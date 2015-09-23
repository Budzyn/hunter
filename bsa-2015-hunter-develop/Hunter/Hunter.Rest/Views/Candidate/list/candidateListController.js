(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CandidateListController', CandidateListController);

    CandidateListController.$inject = [
        '$location',
        '$routeParams',
        '$filter',
        '$scope',
        '$rootScope',
        'AuthService',
        '$odataresource',
        'PoolsHttpService',
        '$odata',
        'EnumConstants',
        'CandidateHttpService',
        'LonglistHttpService',
        'VacancyHttpService',
        'CandidateService'
    ];

    function CandidateListController($location, $routeParams, $filter, $scope, $rootScope, authService, $odataresource, PoolsHttpService, $odata, EnumConstants, CandidateHttpService, longlistHttpService, vacancyHttpService, CandidateService) {
        var vm = this;
        //Here we should write all vm variables default values. For Example:
        vm.name = 'Candidates';

        $rootScope.candidateDetails = {
            id: null,
            show: false
        };
        //vm.tableSize = 'col-md-9';
        vm.sortOptions = [];
        vm.totalItems = 0;
        vm.inviters = [];
        vm.pools = [];
        vm.statuses = [];
        vm.filter = {};

        vm.vacancy;
        vm.vacancyId;
        vm.tableSpinner = false;


        vm.getCandidates = getCandidates;

        vm.pageConfig = {
            pageTitle: 'Candidates (general pool)',
            isAddToVacancyButton: true,
            locationAfterAdding: '/candidate/list'
        };

        if ($routeParams.addToVacancy) {
            vm.vacancyId = $routeParams.addToVacancy;

            vm.pageConfig.isAddToVacancyButton = false;
            vm.pageConfig.locationAfterAdding = '/vacancy/' + vm.vacancyId + '/longlist';
        }


        function getCandidates(filter) {
            vm.tableSpinner = true;
            CandidateHttpService.getOdataCandidateList(filter).then(function (result) {
                vm.candidateList = result.items;
                vm.totalItems = result.count;
                vm.tableSpinner = false;
            });
        }

        vm.updateCandidate = function () {
            $location.search(vm.filter);
        };

        $scope.$on('$routeUpdate', function () {

            var filter = CandidateService.convertRouteParamsToFilter($routeParams);
            vm.getCandidates(filter);
        });

        vm.ShowDetails = function (item) {
            if ($rootScope.candidateDetails.id != item.id) {
                $rootScope.candidateDetails.id = item.id;
                $rootScope.candidateDetails.show = true;
                $rootScope.candidateDetails.shortListed = item.shortListed;
                //vm.tableSize = 'col-md-5';
            } else if ($rootScope.candidateDetails.id === item.id && $rootScope.candidateDetails.show === true) {
                $rootScope.candidateDetails.show = false;
                //vm.tableSize = 'col-md-9';
            } else {
                $rootScope.candidateDetails.show = true;
                //vm.tableSize = 'col-md-5';
            }
        }

        vm.ActiveTr = function (id) {
            if (id == $rootScope.candidateDetails.id && $rootScope.candidateDetails.show === true) {
                return 'info';
            }
            else {
                return '';
            }
        }

        // signatures for user actions callback method
        vm.addCandidateToLongList = addCandidateToLongList;
        // user actions methods

        // add cardidates to Long List
        function addCandidateToLongList() {
            var cards = createCardRequestBody();
            //console.log(cards);
            longlistHttpService.addCards(cards, vm.pageConfig.locationAfterAdding);
        }

        // not user-event functions 
        vm.selectedCandidates = [];

        vm.vacancyByState = [];
        vm.vacancyStateIds = [EnumConstants.vacancyStates[1].id, EnumConstants.vacancyStates[2].id];

        function createCardRequestBody() {
            var cards = [];
            for (var i = 0; i < vm.selectedCandidates.length; i++) {
                cards.push({
                    CandidateId: vm.selectedCandidates[i],
                    VacancyId: vm.vacancyId,
                    Stage: EnumConstants.cardStages[0].id
                });
            }

            return cards;
        }

        function isObjectEmpty(obj) {
            return (Object.getOwnPropertyNames(obj).length === 0);
        }

        // initializating function
        (function () {

            vm.sortOptions = [
                { text: 'Name \u25BC', options: 'FirstName_asc'  },
                { text: 'Name \u25B2', options: 'FirstName_desc'  },
                { text: 'Added \u25BC', options:  'AddDate_asc'  },
                { text: 'Added \u25B2', options:  'AddDate_desc' },
                { text: 'Status \u25BC', options:  'Resolution_asc' },
                { text: 'Status \u25B2', options:  'Resolution_desc' },
                { text: 'Email \u25BC', options:  'Email_asc' },
                { text: 'Email \u25B2', options:  'Email_desc' },
                { text: 'Years Of Experience \u25BC', options:  'YearsOfExperience_asc' },
                { text: 'Years Of Experience \u25B2', options:  'YearsOfExperience_desc' },
                { text: 'Company \u25BC', options:  'Company_asc' },
                { text: 'Company \u25B2', options:  'Company_desc' },
                { text: 'Location \u25BC', options:  'Location_asc' },
                { text: 'Location \u25B2', options:  'Location_desc' },
                { text: 'Salary \u25BC', options:  'Salary_asc' },
                { text: 'Salary \u25B2', options:  'Salary_desc' }
            ];


            vm.statuses = EnumConstants.resolutions;

            vm.filter = CandidateService.convertRouteParamsToFilter($routeParams);
            vm.getCandidates(vm.filter);

            // get vacancy info
            vacancyHttpService.getLongList(vm.vacancyId).then(function (result) {
                console.log(result);
                vm.vacancy = result;
                vm.pageConfig.pageTitle = "Add Candidates to '" + vm.vacancy.name + "'";
            });

            PoolsHttpService.getAllPools().then(function (result) {
                vm.pools = result;
            });

            CandidateHttpService.getAddedByList().then(function (result) {
                vm.inviters = result;
            });

            //getting vacancies with "open" and "on hold" states
            for (var i = 0; i < vm.vacancyStateIds.length; i++) {
                vacancyHttpService.getVacancyByState(vm.vacancyStateIds[i]).then(function (result) {
                    console.log(result);
                    for (var j = 0; j < result.length; j++) {
                        vm.vacancyByState.push(result[j]);
                    }
                });
            }
        })();
    }

})();