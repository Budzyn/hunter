(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('FeedbackHttpService', FeedbackHttpService);

    FeedbackHttpService.$inject = [
        '$q',
        'HttpHandler'
    ];

    function FeedbackHttpService($q, httpHandler) {
        var service = {
            getHrFeedback: getHrFeedback,
            saveHrFeedback: saveHrFeedback,
            saveTestFeedback: saveTestFeedback,
            getTechFeedback: getTechFeedback,
            saveFeedback: saveFeedback,
            getSummary: getSummary,
            saveSummary: saveSummary,
            updateSuccessStatus: updateSuccessStatus,
            getLastFeedbacks: getLastFeedbacks,
            getFeedbacksHistory: getFeedbacksHistory
        }

        function getHrFeedback(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/hr/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                    //console.log("getHrFeedback " + result.data);
                },
                errorCallback: function (status) {
                    console.log("Get vacancies error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function saveHrFeedback(feedback, vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/save',
                verb: 'POST',
                body: JSON.stringify(feedback),
                successCallback: function (result) {
                    var saveResult = result.data;
                    deferred.resolve(saveResult);
                },
                errorCallback: function (result) { console.log(result); }
            });
            return deferred.promise;
        };

        function saveTestFeedback(feedbackObj) {

            console.log(feedbackObj);
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/test/update',
                verb: 'PUT',
                body: feedbackObj,
                successCallback: function(response) {
                    console.log('test feedback was successfuly updated');
                    deferred.resolve(response.data);
                },
                errorCallback: function(response) { console.log('Updating was failed') }
            });

            return deferred.promise;
        }

        function getTechFeedback(vacancyId, candidateId) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/tech/' + vacancyId + '/' + candidateId,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get feedback error");
                }
            });
            return deferred.promise;
        }

        function saveFeedback(feedback, vacancyId, candidateId) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/save',
                verb: 'POST',
                body: JSON.stringify(feedback),
                successCallback: function (result) {
                    //                    var newFeedback = getTechFeedback(vacancyId, candidateId);
                    deferred.resolve(result.data);
                },
                errorCallback: function (result) {
                    console.log("tech feedback update - error");
                    console.log(result);
                }
            });
            return deferred.promise;
        };

        function getSummary(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/summary/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get summary error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function saveSummary(feedback, vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/save',
                verb: 'POST',
                body: JSON.stringify(feedback),
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (result) { console.log(result); }
            });
            return deferred.promise;
        };

        function updateSuccessStatus(feedbackId, status) {
            httpHandler.sendRequest({
                'url': '/api/feedback/' + feedbackId + '/success/update',
                'verb': 'PUT',
                body: status,
                'successCallback': function (response) { console.log(response); },
                'errorCallback': function (response){console.log(response)}
            });
        }

        function getLastFeedbacks(vid,cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/overview/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get feedbacks error");
                    console.log(status);
                }
            });
            return deferred.promise;
        };

        function getFeedbacksHistory(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/feedback/history/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get history feedbacks error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        return service;
    }
})();