(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CandidatePartialController', CandidatePartialController);

    CandidatePartialController.$inject = [
        '$scope',
        '$location',
        '$rootScope',
        'AuthService',
        'CandidateHttpService',
        'EnumConstants',
        'LonglistHttpService',
        'SpecialNoteHttpService',
        'CandidatePartialProfileService'
    ];

    function CandidatePartialController($scope, $location, $rootScope, authService, candidateHttpService, EnumConstants,
        longlistHttpService, specialNoteHttpService, candidatePartialProfileService) {
        var vm = this;
        //Here we should write all vm variables default values. For Example:
        vm.isEmpty = false;

        vm.stages = EnumConstants.cardStages;
        vm.tabs = [
            { name: 'Notes', route: 'specialnotes' },
            { name: 'App results', route: 'appresults' }
        ];
        vm.templateToShow = '';
        vm.candidate;
        vm.appResults = [];
        vm.specialNotes = [];
        vm.resolutions = EnumConstants.resolutions;
        vm.specialNotes = [];

        vm.updateResolution = updateResolution;
        vm.changeTemplate = changeTemplate;
        vm.showResume = showResume;
        $rootScope.$watch(
            '$root.candidateDetails.id',
            function () {
                if ($rootScope.candidateDetails.id == 0) {
                    vm.isEmpty = true;
                } else {
                    getCandidateDetails($rootScope.candidateDetails.id);
                    vm.isEmpty = false;
                    vm.changeTemplate(vm.tabs[1]);
                }
            });

        $rootScope.$watch(
            '$root.candidateDetails.shortListed',
            function () {
                if (vm.candidate) {
                    vm.candidate.shortListed = $rootScope.candidateDetails.shortListed;
                }
            });


        function getCandidateDetails(id) {
            vm.prevLoad = true;
            candidateHttpService.getCandidate(id).then(function (response) {
                vm.candidate = response.data;        
            });

            longlistHttpService.getAppResults(id).then(function (response) {
                vm.appResults = response;
            });

            specialNoteHttpService.getCandidateSpecialNote(id).then(function (response) {
                vm.specialNotes = response.data;
                vm.prevLoad = false;
            });

        }

        function updateResolution() {
            candidateHttpService.updateCandidateResolution(vm.candidate.id, vm.candidate.resolution);
            angular.forEach($scope.$parent.candidateListCtrl.candidateList, function (item) {
                if (item.id == vm.candidate.id) {
                    item.resolution = vm.candidate.resolution;
                }
            });
        };

        function changeTemplate(tab) {
            vm.currentTabName = tab.name;
            vm.currentTabEmpty = candidatePartialProfileService.isCurrentTabEmpty(tab.route, vm.specialNotes, vm.appResults);
            vm.templateToShow = candidatePartialProfileService.changeTemplate(tab.route);
        }

        function showResume() {
            window.open(vm.candidate.lastResumeUrl);
        }
    }
})();