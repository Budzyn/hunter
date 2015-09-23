(function() {
    'use strict';

    angular
        .module('hunter-app')
        .directive('thumbButton', ThumbButton);

    ThumbButton.$inject = [
//        'EnumConstants'
    ];

    function ThumbButton() {
        return {
            template:
                    '<div id="likeBtn" ng-click="click()" class="circleButton">' +
                        '<span id="likeIcon" class="fa circleButton-icon"></span>' +
                    '</div>',
            scope: {
                btnType: '@btnType',
                btnIcon: '@btnIcon',
                feedback: '=feedback',
                key: '@key'
            },
            restrict: 'EA',
            link: function (scope, elem, attr) {
                var button = $(elem).find('.circleButton')[0];
                var icon = $(button).find('.circleButton-icon')[0];

                $(icon).addClass(scope.btnIcon);
                $(button).addClass(scope.btnType);

                scope.click = function() {

                    if (scope.feedback.feedbackConfig.style == undefined || scope.feedback.feedbackConfig.style["border-color"] != scope.getColor(scope.key)) {
                        scope.feedback.feedbackConfig.style = { 'border-color': scope.getColor(scope.key) };
                        scope.feedback.successStatus = scope.getSuccess(scope.key);
                    } else {
                        scope.feedback.feedbackConfig.style = { 'border-color': scope.getColor('None') }
                        scope.feedback.successStatus = scope.getSuccess('None');
                    }

                    scope.saveSuccessStatus(scope.feedback);
                }
            },
            controller: ['$scope', 'EnumConstants', 'FeedbackHttpService', function ($scope, EnumConstants, FeedbackHttpService) {
                    $scope.getSuccess = function(key) {
                        return EnumConstants.successStatus[key];
                    }

                    $scope.getColor = function(key) {
                        return EnumConstants.voteColors[key];
                    }

                    $scope.saveSuccessStatus = function(feedback) {
                        FeedbackHttpService.updateSuccessStatus(feedback.id, feedback.successStatus);
                    }
            }]
        }
    }
})()