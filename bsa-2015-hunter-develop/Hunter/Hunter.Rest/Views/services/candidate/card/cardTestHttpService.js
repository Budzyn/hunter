(function() {
    'use strict';

    angular
        .module('hunter-app')
        .factory('CardTestHttpService', CardTestHttpService);

    CardTestHttpService.$inject = [
        'HttpHandler',
        
    ];

    function CardTestHttpService(HttpHandler) {
        var service = {
            'getTest': getTest,
            'sendTest': uploadTest,
            'updateTestComment': updateTestComment,
            'getAllTests': getAllTests,
            addCheckingToTest: addCheckingToTest
        };

        function getTest(vacancyId, candidateId, success) {
            HttpHandler.sendRequest({
                'verb': 'GET',
                'url': 'api/test?vacancyId=' + vacancyId + '&candidateId=' + candidateId,
                'successCallback': success,
                'errorMessageToDev': 'GET TEST ERROR',
                'errorMessageToUser': 'Failed'
            });
        }

        function getAllTests(vacancyId, candidateId, success) {
            HttpHandler.sendRequest({
                'verb': 'GET',
                'url': 'api/test/all/' + candidateId,
                'successCallback': success,
                'errorMessageToDev': 'GET ALL TESTS ERROR',
                'errorMessageToUser': 'Failed'
            });
        }

        function uploadTest(body, success) {
            HttpHandler.sendRequest({
                'verb': 'POST',
                'url': '/api/test/add',
                'body': body,
                'successCallback': success,
                'errorMessageToDev': 'POST TEST ERROR',
                'errorMessageToUser': 'uploading failed'
            });
        }

        function updateTestComment(body, success) {
            HttpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/test/comment',
                body: body,
                successCallback: success,
                errorMessageToDev: 'POST TEST COMMENT ERROR'
            });
        }

        function addCheckingToTest(userId, testId, success) {
            HttpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/test/addChecking/' + testId + "/" + userId,
                
                successCallback: success,
                errorCallback: function (response) {
                    console.log("error add checking to test");
                    console.log(response);
                }
            });
        };

        return service;
    }
})();