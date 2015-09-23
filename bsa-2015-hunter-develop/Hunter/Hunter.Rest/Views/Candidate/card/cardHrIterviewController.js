(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CardHrInterviewController', CardHrInterviewController);

    CardHrInterviewController.$inject = [
        'VacancyHttpService',
        'FeedbackHttpService',
        'CardHrInterviewService',
        '$routeParams',
        'EnumConstants',
        '$scope'
    ];

    function CardHrInterviewController(VacancyHttpService, FeedbackHttpService, CardHrInterviewService,
        $routeParams, EnumConstants, $scope) {
        var vm = this;

        vm.templateName = 'HR Interview';
        vm.vacancy;
        vm.feedbacks;
        vm.newFeedbackText = '';
        vm.newFeedbackType = null;
        vm.saveFeedback = saveFeedback;
        vm.updateFeedback = updateFeedback;
        vm.getFeedbacks = getFeedbacks;
        vm.getAllFeedbacks = getAllFeedbacks;
        vm.getMyFeedbacks = getMyFeedbacks;
        vm.voteColors = [];
        vm.HrFeedbackTypes = [];

        (function () {
            if ($scope.$parent.generalCardCtrl.isLLM) {
                VacancyHttpService.getVacancy($routeParams.vid).then(function (result) {
                    vm.vacancy = result;
                });
                vm.getFeedbacks();
            } else {
                vm.getAllFeedbacks();
            }

            for (var x in EnumConstants.voteColors) {
                if (EnumConstants.voteColors.hasOwnProperty(x)) {
                    vm.voteColors.push(EnumConstants.voteColors[x]);
                }
            }

            vm.HrFeedbackTypes = EnumConstants.feedbackTypes.slice(0, 2);

        })();


        function saveFeedback(feedback, isNew) {
            if (!isNew) {
                var body = {
                    id: feedback.id,
                    cardId: $scope.$parent.generalCardCtrl.cardInfo.cardId,
                    text: feedback.text,
                    type: feedback.type,
                    successStatus: feedback.successStatus
                }
            }
            else {
                var body = {
                    cardId: $scope.$parent.generalCardCtrl.cardInfo.cardId,
                    text: feedback,
                    type: vm.newFeedbackType,
                }
            }


 //           feedback.cardId = $scope.$parent.generalCardCtrl.cardInfo.cardId;

            FeedbackHttpService.saveFeedback(body, $routeParams.vid, $routeParams.cid).then(function (result) {
                if(!isNew){
                    feedback.id = result.id;
                    feedback.date = result.date;
                    feedback.userAlias = result.userAlias;
                }
                else {
                    setFeedbackConfig(result);
                    vm.feedbacks.push(result);
                    vm.newFeedbackText = '';
                }
            });
        }

        function updateFeedback(feedback) {
            if (!feedback.editMode) {
                feedback.editMode = !feedback.editMode;
            } else {
                feedback.editMode = !feedback.editMode;
                vm.saveFeedback(feedback,false);
            }
        }

        function getAllFeedbacks() {
            FeedbackHttpService.getHrFeedback(0, $routeParams.cid).then(function (result) {
                vm.feedbacks = result;
                vm.feedbacks.forEach(function(feedback) {
                    setFeedbackConfig(feedback);
                });
            });
        }
        function getMyFeedbacks() {
            FeedbackHttpService.getHrFeedback(-1, $routeParams.cid).then(function (result) {
                vm.feedbacks = result;
                vm.feedbacks.forEach(function(feedback) {
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

        function getFeedbacks() {
            FeedbackHttpService.getHrFeedback($routeParams.vid, $routeParams.cid).then(function (result) {
                vm.feedbacks = result;
                vm.feedbacks.forEach(function (feedback) {
                    setFeedbackConfig(feedback);
                });
            });
        }


    }
})();