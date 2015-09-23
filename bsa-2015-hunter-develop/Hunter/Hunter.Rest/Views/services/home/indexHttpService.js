(function() {
    'use strict';

    angular
        .module('hunter-app')
        .factory('IndexHttpService', IndexHttpService);

    IndexHttpService.$inject = [
        'HttpHandler',
        '$q'
    ];

    function IndexHttpService(HttpHandler, $q) {
        var service = {
            'getActivityAmount': getActivityAmount,
            'getTasksForCheck': getTasksForCheck
        };

        function getActivityAmount(successCallback) {
            HttpHandler.sendRequest({
                'verb': 'GET',
                'url': 'api/activities/amount',
                'successCallback': successCallback,
                'errorMessageToDev': 'GET ACTIVITY INFO ERROR: ',
                'errorMessageToUser': 'Failed'
            });
        }

        function getTasksForCheck() {
            var deferred = $q.defer();
            HttpHandler.sendRequest({
                url: '/api/test/count',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get test count error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }
        return service;
    }

})();