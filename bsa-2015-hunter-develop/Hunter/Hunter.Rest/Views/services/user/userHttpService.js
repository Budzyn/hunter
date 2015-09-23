(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('UserHttpService', UserHttpService);

    UserHttpService.$inject = [
        '$q',
        'HttpHandler'
    ];

    function UserHttpService($q, httpHandler) {
        var services = {
            getUsersByRole: getUsersByRole,
            getUsersByRole2: getUsersByRole2
        }

        function getUsersByRole(roleName) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/user/' + roleName,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get users by role error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getUsersByRole2(roleName) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/userprofile/users/role/' + roleName,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get users by role error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        return services;
    }

})();