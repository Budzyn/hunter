(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('FileHttpService', FileHttpService);

    FileHttpService.$inject = [
        '$q',
        'HttpHandler',
        'Upload'
    ];

    function FileHttpService($q, httpHandler, upload) {
        var service = {
            getFiles: getFiles,
            getFile: getFile,
            addFile: addFile,
            updateFile: updateFile,
            deleteFile: deleteFile,
            getResume: getResume
        }

        function getFiles() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/file',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get files error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getFile(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/file/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get file error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getResume(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/file/resume/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get resume error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function addFile(data) {
            httpHandler.sendRequest({
                verb: 'POST',
                url: '/api/file',
                body: data,
                errorCallback: function (status) {
                    console.log("Add file error");
                    console.log(status);
                }
            });
        }

        function updateFile(data, id) {
            httpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/file/' + id,
                body: data,
                errorCallback: function (status) {
                    console.log("Update file error");
                    console.log(status);
                }
            });
        }

        function deleteFile(id,success) {
            httpHandler.sendRequest({
                verb: 'DELETE',
                url: '/api/file/' + id,
                successCallback:success,
                errorCallback: function (status) {
                    console.log("Delete file error");
                    console.log(status);
                }
            });
        }

        return service;
    }
})();