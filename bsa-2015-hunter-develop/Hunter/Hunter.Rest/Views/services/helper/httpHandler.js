(function() {
    'use strict';
    angular
        .module('hunter-app')
        .factory('HttpHandler', HttpHandler);

    HttpHandler.$inject = ['$http'];

    function HttpHandler($http) {

        var handler = {
            sendRequest: sendRequest
        };

        /**
         * 
         * @param  httpObject {
            url: STRING,
            params: JSON OBJECT,
            verb: STRING,
            body: JSON OBJECT,
            successCallback: FUNCTION,
            errorCallback: FUNCTION,
            errorMessageToUser: STRING,
            errorMessageToDev: STRING,
            successMessageToUser: STRING
         * }
         * @returns {} 
         */
        function sendRequest(httpObject) {
            if (!checkUrl(httpObject.url)) {
                return;
            }
            if (!checkRequestType(httpObject.url)) {
                return;
            }

            var config = {
                'method': httpObject.verb.toUpperCase(),
                'url': httpObject.url,
                'data': httpObject.body
            };
            if (typeof httpObject.params == 'object') {
                config.params = httpObject.params;
            }

            $http(config)
                .then(successfulRequest)
                .catch(failedRequest);

            function successfulRequest(response) {

                if (httpObject.successMessageToUser) {
                    alertify.success(httpObject.successMessageToUser);
                }

                if (httpObject.successCallback && typeof httpObject.successCallback === 'function') {
                    httpObject.successCallback(response);
                }
            }

            function failedRequest(error) {
                if (httpObject.errorMessageToDev) {
                    console.log(httpObject.errorMessageToDev);
                }

                if (httpObject.errorMessageToUser) {
                    alertify.error(httpObject.errorMessageToUser);
                }

                if (httpObject.errorCallback && typeof httpObject.errorCallback === 'function') {
                    httpObject.errorCallback(error);
                }
            }
        }

        function checkUrl(url) {
            if (!url) {
                console.log('HTTP REQUEST ERROR: URL DOES NOT SPECIFIED');
                alertify.error('Action Failed');
                return false;
            }

            return true;
        }

        function checkRequestType(type) {
            if (typeof type !== 'string' || !type) {
                console.log('HTTP REQUEST ERROR: HTTP REQUEST TYPE DOES NOT SPECIFIED');
                alertify.error('Action Failed');
                return false;
            }
            return true;


        }

        return handler;
    }
})();