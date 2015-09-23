(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('LongListPreviewController', LongListPreviewController);

    LongListPreviewController.$inject = [
        '$scope',
        '$location',
        '$route',
        '$rootScope',
        'CandidateHttpService',
        'LonglistHttpService',
        'EnumConstants',
        'LonglistService',
        'FeedbackHttpService',
        'CardTestHttpService',
        'SpecialNoteHttpService',
        '$timeout'
    ];

    function LongListPreviewController($scope, $location, $route, $rootScope, candidateHttpService, longlistHttpService, EnumConstants, longlistService, feedbackHttpService, cardTestHttpService, specialNoteHttpService, $timeout) {
        var vm = this;

        vm.hideCandidatePreview = hideCandidatePreview;
        vm.getCandidateDetails = getCandidateDetails;
        vm.removeCard = removeCard;
        vm.changeTemplate = changeTemplate;
        vm.showResume = showResume;

        vm.isPreviewShown = false;
        vm.stages = EnumConstants.cardStages;
        vm.resolutions = EnumConstants.resolutions;
        vm.feedbackTypes = EnumConstants.feedbackTypes;

        vm.tabs = [
            { name: 'Overview', route: 'overview' },
            { name: 'Notes', route: 'specialnotes' },
            { name: 'App results', route: 'appresults' }
        ];

        vm.templateToShow = '';

       
        $rootScope.$watch(
            '$root.candidatePreview.cid',
            function () {
                vm.vacancyId = $rootScope.candidatePreview.vid;

                if ($rootScope.candidatePreview.cid === 0) {
                    vm.isPreviewShown = false;
                } else {
                    vm.getCandidateDetails($rootScope.candidatePreview.cid);
                    vm.isPreviewShown = true;
                    vm.changeTemplate(vm.tabs[2]);
                }
            });

        function hideCandidatePreview() {
            $scope.$emit('hideCandidatePreview');
            $rootScope.candidatePreview.cid = 0;
            vm.isPreviewShown = false;
        }

        vm.overviews = [];
        vm.notes = [];
        vm.appResults = [];

        function getCandidateDetails(cid) {
            vm.prevLoad = true;
            (function () {
                candidateHttpService.getLongListDetails(vm.vacancyId, cid).then(function (result) {
                    vm.candidateDetails = result;
                });
            })();

            (function() {
                feedbackHttpService.getLastFeedbacks(vm.vacancyId, cid)
                    .then(function(result) {
                        vm.overviews = result;
                    });
            })();

            (function () {
                specialNoteHttpService.getCardSpecialNote(vm.vacancyId, cid)
                    .then(function (result) {
                        vm.notes = result.data;
                    });
            })();

            (function() {
                longlistHttpService.getAppResults(cid).then(function (result) {
                    vm.appResults = result;
                    vm.prevLoad = false;
                });
            })();
        }

        function removeCard(cid) {
            longlistHttpService.removeCard(vm.vacancyId, cid);
            
            $timeout(function () {
                vm.isPreviewShown = false;
                $scope.$emit('getCardsAfterCardDeleting');
            }, 1200);

            //$route.reload();
            //$scope.$emit('getCardsAfterCardDeleting');
        }

        function changeTemplate(tab) {
            vm.currentTabName = tab.name;
            vm.currentTabEmpty = longlistService.isCurrentTabEmpty(tab.route, vm.overviews, vm.notes);
            vm.templateToShow = longlistService.changeTemplate(tab.route);
        }

        function showResume() {
            window.open(vm.candidateDetails.lastResumeUrl);
        }
    }
})();