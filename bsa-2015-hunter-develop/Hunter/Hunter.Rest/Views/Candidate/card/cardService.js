(function() {
    'use strict';

    angular
        .module('hunter-app')
        .factory('CardService', CardService);

    CardService.$inject = [
        '$q',
        'HttpHandler'
    ];

    function CardService($q, httpHandler) {
        var service = {
            changeTemplate: changeTemplate,
            updateCardStage: updateCardStage,
            getCardStage: getCardStage
            },
            viewsPath = '/Views/candidate/card/';

        function changeTemplate(templateRoute) {
            switch (templateRoute.toLowerCase()) {
                case 'overview':
                    return viewsPath + 'cardOverview.html';
                case 'specialnotes':
                    return  viewsPath + 'cardSpecialNotes.html';
                case 'hrinterview':
                    return  viewsPath + 'cardHrInterview.html';
                case 'technicalinterview':
                    return  viewsPath + 'cardTechnicalInterview.html';
                case 'test':
                    return viewsPath + 'cardTest.html';
                case 'summary':
                    return viewsPath + 'cardSummary.html';
                case 'application':
                    return viewsPath + 'cardApplications.html';
                default:
                    return '';
            }
        }

        function updateCardStage(vid, cid, stage) {
            httpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/card/stage/?vid=' + vid + '&cid=' + cid,
                body: stage,
                successMessageToUser: 'Card stage was updated',
                errorMessageToUser: 'Card stage update failed',
                errorCallback: function (status) {
                    console.log("Update card stage error:" + status);
                }
            });
        }

        function getCardStage(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/card/stage/?vid=' + vid + '&cid=' + cid,
                successCallback: function (response) {
                    deferred.resolve(response);
                },
                errorCallback: function (status) {
                    console.log("getting card stage error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        return service;
    }
})();