(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('LongListController', LongListController);

    LongListController.$inject = [
        '$location',
        'VacancyHttpService',
        'CandidateHttpService',
        'LonglistHttpService',
        'LonglistService',
        '$routeParams',
        '$odataresource',
        '$odata',
        '$filter',
        '$scope',
        'EnumConstants',
        '$timeout',
        '$rootScope'
        //'$on'
    ];

    function LongListController($location, vacancyHttpService, candidateHttpService, longlistHttpService, longlistService, $routeParams, $odataresource, $odata, $filter, $scope, EnumConstants, $timeout, $rootScope) {
        var vm = this;

        vm.stages = [];
        vm.shortlisted = true;

        vm.name = '';
        vm.filter = {};
        vm.itemsPerPage = [];
        vm.vacancy = {};
        vm.candidateDetails = {};
        vm.viewCandidateInfo = viewCandidateInfo;
        vm.getCandidatesForLongList = getCandidatesForLongList;
        vm.listSpinner = false;

        // click on candidate item shows candidates preview
        $rootScope.candidatePreview = {
            vid: $routeParams.id,
            cid: 0
        };

        vm.activateItemId = 0;
        function viewCandidateInfo(cid) {
            $rootScope.candidatePreview.cid = cid;
            vm.activateItemId = cid;
        }

        $scope.$on('hideCandidatePreview', function () {
            vm.activateItemId = 0;
        });

        $scope.$on('getCardsAfterCardDeleting', function () {
            vm.getCandidatesForLongList(vm.filter);
        });

        vm.updateUrl = function () {
            $location.search(vm.filter);
        };

        $scope.$on('$routeUpdate', function () {
            vm.filter = longlistService.convertRouteParamsToFilter($routeParams);
            vm.getCandidatesForLongList(vm.filter);
        });

       

        function getCandidatesForLongList(filter) {
            vm.listSpinner = true;
            longlistHttpService.getOdataLongList(filter).then(function (result) {
                vm.candidatesList = result.items;
                vm.listSpinner = false;
                if (vm.candidatesList.length > 0) {
                    vm.viewCandidateInfo(vm.candidatesList[0].id);
                }
                vm.totalItems = result.count;
            });
        };

        // initializating function
        (function () {
            vm.name = 'CandidatesForLongList';
            vm.itemsPerPage = [5, 10, 20, 40];
            vm.sortOptions = [
                { text: 'Added Date (new first)', options: 'AddDate_desc' },
                { text: 'Added Date (old first)', options: 'AddDate_asc' },
                { text: 'Name (A-Z)', options: 'FirstName_asc' },
                { text: 'Name (Z-A)', options: 'FirstName_desc' }
            ];

            // get vacancy info
            vacancyHttpService.getLongList($routeParams.id).then(function (result) {
                vm.vacancy = result;
            });

            // get all Added by
            vacancyHttpService.getLongListAddedBy($routeParams.id).then(function (result) {
                vm.addedByList = result;
            });

            vm.stages = EnumConstants.cardStages;
            vm.filter = longlistService.convertRouteParamsToFilter($routeParams);
            vm.getCandidatesForLongList(vm.filter);

        })();

    }
})();