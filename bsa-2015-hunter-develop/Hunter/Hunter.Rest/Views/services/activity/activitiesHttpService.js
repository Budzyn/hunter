(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('ActivityHttpService', ActivityHttpService);

    ActivityHttpService.$inject = [
        'HttpHandler',
        '$q',
        '$odata',
        '$odataresource'
    ];

    function ActivityHttpService(httpHandler, $q, $odata, $odataresource) {
        var service = {
            getActivityList: getActivityList,
            saveLastActivityId: saveActivityId,
            getFilterOptions: getFilterOptions,
            getActivitiesByFilter: getActivitiesByFilter
        };

        function getActivityList(successCallback, body) {
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/activities/',
                //                body: body,
                successCallback: successCallback,
                errorMessageToDev: 'GET ACTIVITY INFO ERROR: ',
                errorMessageToUser: 'Failed'
            });
        }

        function saveActivityId(successCallback, body) {
            httpHandler.sendRequest({
                'verb': 'POST',
                'url': 'api/activities/save/lastid',
                'body': body,
                'successCallback': successCallback,
                'errorMessageToDev': 'POST ACTIVITY_ID ERROR',
                'errorMessageToUser': 'Failed'
            });
        }

        function getFilterOptions() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/activities/filters',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get activities filter options error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getActivitiesByFilter(filter) {
            var deferred = $q.defer();
            var activitiesOdata = $odataresource('/api/activities/odata');
            var predicate;

            var filt = [];
            if (filter.users.length > 0) {
                var uPred = [];
                angular.forEach(filter.users, function (value, key) {
                    uPred.push(new $odata.Predicate('UserAlias', value));
                });

                uPred = $odata.Predicate.or(uPred);
                filt.push(uPred);
            }

            if (filter.tags.length > 0) {
                var tPred = [];
                angular.forEach(filter.tags, function (value, key) {
                    tPred.push(new $odata.Predicate('Tag', value));
                });

                tPred = $odata.Predicate.or(tPred);
                filt.push(tPred);
            }

            if (filt.length > 0) {
                predicate = $odata.Predicate.and(filt);
            } else {
                predicate = undefined;
            }

            var skip = (filter.currentPage - 1) * filter.pageSize;

            var acts = activitiesOdata.odata()
                                        .withInlineCount()
                                        .take(filter.pageSize)
                                        .skip(skip)
                                        .filter(predicate)
                                        .orderBy('Time', 'desc')
                                        .query(function () {
                                            deferred.resolve(acts);
                                        });
            return deferred.promise;
        }

        return service;
    }
})();