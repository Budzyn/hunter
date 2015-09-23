(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CardOverviewController', CardOverviewController);

    CardOverviewController.$inject = [
        'FeedbackHttpService',
        '$routeParams',
        '$scope',
        'EnumConstants'
    ];

    function CardOverviewController(FeedbackHttpService, $routeParams, $scope, EnumConstants) {
        var vm = this;

        vm.feedbacks = {};
        vm.voteColors = [];
        vm.feedbacksHistory=[];
        vm.showHistoryBlock = false;

        (function () {
            if ($scope.$parent.generalCardCtrl.isLLM) {
                FeedbackHttpService.getLastFeedbacks($routeParams.vid, $routeParams.cid).then(function(result) {
                    vm.feedbacks = result;
                });
            } else {
                FeedbackHttpService.getLastFeedbacks(0, $routeParams.cid).then(function (result) {
                    vm.feedbacks = result;
                });
            }    

            for (var x in EnumConstants.voteColors) {
                if (EnumConstants.voteColors.hasOwnProperty(x)) {
                    vm.voteColors.push(EnumConstants.voteColors[x]);
                }
            }


        })();

        vm.GetFeedbacksHistory= function() {
            vm.showHistoryBlock = !vm.showHistoryBlock;
            if ($scope.$parent.generalCardCtrl.isLLM) {
                FeedbackHttpService.getFeedbacksHistory($routeParams.vid, $routeParams.cid).then(function (result) {
                    vm.feedbacksHistory = result;
                });
            } else {
                FeedbackHttpService.getFeedbacksHistory(0, $routeParams.cid).then(function (result) {
                    vm.feedbacksHistory = result;;
                });
            }
            
        }
    }
})();