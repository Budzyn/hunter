(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CandidateProfileController', CandidateProfileController);

    CandidateProfileController.$inject = [
        '$location',
        '$routeParams',
        'AuthService',
        'CandidateHttpService',
        'CandidateAddEditService',
        'CandidateService',
        'EnumConstants'

    ];

    function CandidateProfileController($location, $routeParams, authService, candidateHttpService, candidateAddEditService, candidateService, EnumConstants) {
        var vm = this;
        vm.isScinner = true;
        vm.subMenuItems = [
            {
                name: 'Applications',
                isActive: true
            }, {
                name: 'Special Notes',
                isActive: false
            }, {
                name: 'Tests',
                isActive: false
            }, {
                name: 'Interviews',
                isActive: false
            }
        ];

        vm.resolutions = [];
        vm.candidate = {};

        vm.updateResolution = updateResolution;
        vm.changeTemplate = changeTemplate;
        vm.showResume = showResume;

        (function() {
            // This is function for initialization actions
            if (!$routeParams.cid) {
                $location.url('/login');
            }
        })();

        (function () {
            vm.templateToShow = candidateService.changeTemplate(vm.subMenuItems[0].name);
            vm.resolutions = EnumConstants.resolutions;
        })();


        function changeTemplate(templateName) {
            vm.templateToShow = candidateService.changeTemplate(templateName);
            vm.subMenuItems.forEach(function (item, i, arr) {
                if (item === templateName) {
                    item.isActive = true;
                } else {
                    item.isActive = false;
                }
            });

        }

        function updateResolution() {
            candidateHttpService.updateCandidateResolution($routeParams.cid, vm.candidate.resolution);
        };

        candidateHttpService.getCandidate($routeParams.cid).then(function (response) {
            vm.candidate = response.data;
            vm.isScinner = false;
            console.log(response.data);
        });

        function showResume() {
            window.open(vm.candidate.lastResumeUrl);
        }

        function getCandidateID(url) {
            return url.split('/').pop();
        }
    }
})();