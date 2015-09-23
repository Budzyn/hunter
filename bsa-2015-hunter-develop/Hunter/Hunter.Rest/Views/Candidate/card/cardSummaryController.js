(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CardSummaryController', cardSummaryController);

    cardSummaryController.$inject = [
        'FeedbackHttpService',
        'VacancyHttpService',
        '$routeParams',
        'EnumConstants'
    ];

    function cardSummaryController(feedbackHttpService, vacancyHttpService, $routeParams, EnumConstants) {
        var vm = this;
        // TODO: Initialization Should Be With Initial Value, at least [], {}, ''
        vm.vacancy;
        vm.summary;

        // TODO: Define event function at the beginning of controller and only then should be implementation vm.saveHrFeedback = saveHrFeedback; function saveHrFeedback() {}
        vm.saveSummary  = function (summary) {
            var body = {
                id: summary.id,
                cardId: summary.cardId,
                text: summary.text,
                type: summary.type,
                successStatus: summary.successStatus
            }
            feedbackHttpService.saveSummary(body, $routeParams.vid, $routeParams.cid).then(function (result) {
                summary.id = result.id;
                summary.date = result.date;
                summary.userAlias = result.userAlias;

                console.log("result after update");
                console.log(result);
            });
        }

        // TODO: Initialization Should Be covered with self invoke function
        vacancyHttpService.getLongList($routeParams.vid).then(function (result) {
            console.log(result);
            vm.vacancy = result;
        });

        feedbackHttpService.getSummary($routeParams.vid, $routeParams.cid).then(function (result) {
            console.log(result);
            vm.summary = result;
            
            if (vm.summary.text == '') {
                vm.summary.feedbackConfig = {
                    'buttonName': 'Save',
                    'readOnly': false
                }
            } else {
                vm.summary.feedbackConfig = {
                    'buttonName': 'Edit',
                    'readOnly': true
                }
            }

            vm.summary.feedbackConfig.style = {
                "border-color": vm.summary.successStatus == 0 ? EnumConstants.voteColors['None']
                    : vm.summary.successStatus == 1 ? EnumConstants.voteColors['Like']
                    : EnumConstants.voteColors['Dislike']
            }
        });

        vm.toggleReadOnly = function() {
            vm.summary.feedbackConfig.readOnly = vm.summary.text == '' ? vm.summary.feedbackConfig.readOnly
                : !vm.summary.feedbackConfig.readOnly;

            if (vm.summary.feedbackConfig.readOnly) {
                vm.summary.feedbackConfig.buttonName = 'Edit';
                vm.saveSummary(vm.summary);
            } else {
                vm.summary.feedbackConfig.buttonName = 'Save';
            }
        }
    }
})();