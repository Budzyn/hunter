(function () {
    'use strict';

    angular.module('hunter-app').factory('CardHttpService', CardHttpService);

    CardHttpService.$inject = [
        'HttpHandler',
        '$q'
    ];

    function CardHttpService(httpHandler,$q) {
        var service = {
            getCardById: getCardById
        }

        function getCardById(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/card/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get card by id error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        return service;
    }
})();