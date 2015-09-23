(function() {
    angular.module('hunter-app')
        .factory('PoolsHttpService', PoolsHttpService);
    
    PoolsHttpService.$inject = [
    'HttpHandler',
    '$q'
    ];
    function PoolsHttpService(httpHandler, $q) {
        var service = {
            getAllPools : getAllPools
        }

        function getAllPools() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/pool',
                successCallback: function(result) {
                    deferred.resolve(result.data);
                },
                errorCallback:function(status) {
                    console.log("get pools error ");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        return service;
    }
})();