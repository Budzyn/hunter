(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CardTechnicalInterviewController', CardTechnicalInterviewController);

    CardTechnicalInterviewController.$inject = [
        'FeedbackHttpService',
        '$routeParams',
        'VacancyHttpService',
        'EnumConstants',
        '$scope'
    ];

    function CardTechnicalInterviewController(FeedbackHttpService, $routeParams, VacancyHttpService, EnumConstants, $scope) {
        var vm = this;
        vm.templateName = 'Technical Interview';
        vm.techFeedbacks = {};
        vm.newTechFeedbackText = '';
        vm.vacancy = {};
        vm.saveFeedback = saveFeedback;
        vm.updateFeedback = updateFeedback;
        vm.getFeedbacks = getFeedbacks;
        vm.getAllFeedbacks = getAllFeedbacks;
        vm.getMyFeedbacks = getMyFeedbacks;
        vm.voteColors = [];

        (function () {
            VacancyHttpService.getVacancy($routeParams.vid).then(function (result) {
                vm.vacancy = result;
            });

            vm.getFeedbacks();

            for (var x in EnumConstants.voteColors) {
                if (EnumConstants.voteColors.hasOwnProperty(x)) {
                    vm.voteColors.push(EnumConstants.voteColors[x]);
                }
            }

        })();

        function saveFeedback(feedback, isNew) {
            if (!isNew) {
                var body = {
                    id: feedback.id,
                    cardId: $scope.$parent.generalCardCtrl.cardInfo.cardId,
                    text: feedback.text,
                    type: 2,
                    successStatus: feedback.successStatus
                }
            }
            else {
                var body = {
                    cardId: $scope.$parent.generalCardCtrl.cardInfo.cardId,
                    text: feedback,
                    type: 2,
                }
            }

            FeedbackHttpService.saveFeedback(body, $routeParams.vid, $routeParams.cid).then(function (result) {
                if (!isNew) {
                    feedback.id = result.id;
                    feedback.date = result.date;
                    feedback.userAlias = result.userAlias;
                }              
                else {
                    setFeedbackConfig(result);
                    vm.techFeedbacks.push(result);
                    vm.newTechFeedbackText = '';
                }
            });
        }



        function updateFeedback(feedback) {
            if (!feedback.editMode) {
                feedback.editMode = !feedback.editMode;
            } else {
                feedback.editMode = !feedback.editMode;
                vm.saveFeedback(feedback, false);
            }
        }

        function getAllFeedbacks() {
            FeedbackHttpService.getTechFeedback(0, $routeParams.cid).then(function (result) {
                vm.techFeedbacks = result;
                vm.techFeedbacks.forEach(function(feedback) {
                    setFeedbackConfig(feedback);
                });
            });
        }
        function getMyFeedbacks() {
            FeedbackHttpService.getTechFeedback(-1, $routeParams.cid).then(function (result) {
                vm.techFeedbacks = result;
                vm.techFeedbacks.forEach(function (feedback) {
                    setFeedbackConfig(feedback);
                });
            });
        }
        function getFeedbacks() {
            FeedbackHttpService.getTechFeedback($routeParams.vid, $routeParams.cid).then(function (result) {
                vm.techFeedbacks = result;
                vm.techFeedbacks.forEach(function (feedback) {
                    setFeedbackConfig(feedback);
                });
            });
        }

        function setFeedbackConfig(feedback) {
            feedback.feedbackConfig = {};
            feedback.feedbackConfig.style = {
                "border-color": feedback.successStatus == 0 ? EnumConstants.voteColors['None']
                    : feedback.successStatus == 1 ? EnumConstants.voteColors['Like']
                    : EnumConstants.voteColors['Dislike']
            }
        }

    }
})();