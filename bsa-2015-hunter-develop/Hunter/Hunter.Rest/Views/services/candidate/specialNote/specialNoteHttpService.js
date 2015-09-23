(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('SpecialNoteHttpService', SpecialNoteHttpService);

    SpecialNoteHttpService.$inject = [
        'HttpHandler',
        '$q'
    ];

    function SpecialNoteHttpService(httpHandler, $q) {
        var service = {
            updateSpecialNote: updateSpecialNote,
            getAllSpecialNote: getAllSpecialNote,
            getCandidateSpecialNote: getCandidateSpecialNote,
            getCardSpecialNote: getCardSpecialNote,
            getUserSpecialNote: getUserSpecialNote,
            getSpecialNote: getSpecialNote,
            addSpecialNote: addSpecialNote
            //deleteSpecialNote: deleteSpecialNote
        };

        function updateSpecialNote(body, id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/specialnote/' + id,
                body: body,
                successCallback: function (response) {
                    deferred.resolve(response.data);
                }
            });

            return deferred.promise;
        }

        function addSpecialNote(body) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'POST',
                url: '/api/specialnote/create',
                body: body,
                successCallback: function (response) {
                     deferred.resolve(response.data);
                }
            });

            return deferred.promise;
        }

        function getSpecialNote(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/specialnote/' + id,
                successCallback: function (response) {
                    deferred.resolve(response);
                },
                errorCallback: function (status) {
                    console.log("getting special note error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getAllSpecialNote() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/specialnote/',
                successCallback: function (response) {
                    deferred.resolve(response);
                },
                errorCallback: function (status) {
                    console.log("getting special note error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getCandidateSpecialNote(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/specialnote/candidate/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result);
                },
                errorCallback: function (status) {
                    console.log("Get candidates long list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getCardSpecialNote(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/specialnote/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result);
                },
                errorCallback: function (status) {
                    console.log("Get candidates long list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getUserSpecialNote(cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/specialnote/user/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result);
                },
                errorCallback: function (status) {
                    console.log("Get candidates long list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        //function deleteSpecialNote(id) {
        //    var deferred = $q.defer();
        //    httpHandler.sendRequest({
        //        url: '/api/specialnote/' + id,
        //        verb: 'DELETE',
        //        successCallback: function (result) {
        //            deferred.resolve(result.data);
        //        },
        //        errorCallback: function (status) {
        //            console.log("Get candidates long list error");
        //            console.log(status);
        //        }
        //    });
        //    return deferred.promise;
        //}

        return service;
    }
})();