(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('NotificationHttpService', NotificationHttpService);

    NotificationHttpService.$inject = [
        '$q',
        'HttpHandler'
    ];

    function NotificationHttpService($q, httpHandler) {
        var service = {
           // getNotifications: getNotifications,
            getCandidateNotifications: getCandidateNotifications,
            getActiveNotifications: getActiveNotifications,
            addNotification: addNotification,
            notificationShown: notificationShown,
            deleteNotification: deleteNotification,
            notify: notify,
            convertRouteParamsToFilter: convertRouteParamsToFilter,
            getNotificationsByFilter: getNotificationsByFilter
        };

        function getNotifications() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/notifications',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get notification list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getCandidateNotifications(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/notifications/candidate/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get candidate notification list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getActiveNotifications() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/notifications/active',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get notification list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function addNotification(data) {
            httpHandler.sendRequest({
                verb: 'POST',
                url: '/api/notifications',
                body: data,
                successMessageToUser: 'Scheduled notification added',
                errorMessageToUser: 'Scheduled notification not added',
                errorCallback: function (status) {
                    console.log("Add Scheduled notification error");
                    console.log(status);
                }
            });
        }

        function notificationShown(id) {
            httpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/notifications/' + id + '/shown',
                errorCallback: function (status) {
                    console.log("Shown Scheduled notification error");
                    console.log(status);
                }
            });
        }

        function deleteNotification(id) {
            httpHandler.sendRequest({
                verb: 'DELETE',
                url: '/api/notifications/' + id,
                successMessageToUser: 'Scheduled notification deleted',
                errorMessageToUser: 'Scheduled notification not deleted',
                errorCallback: function (status) {
                    console.log("Delete Scheduled notification error");
                    console.log(status);
                }
            });
        }

        function notify() {
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/notifications/notify',
                successMessageToUser: 'Scheduled notifications sent',
                errorMessageToUser: 'Scheduled notifications not sent',
                errorCallback: function (status) {
                    console.log("Scheduled notifications sending error");
                    console.log(status);
                }
            });
        }

        function convertRouteParamsToFilter(routeParams) {
            var filter = {
                notificationTypes: [0, 1, 2],
                pageSize: parseInt(routeParams.pageSize) || 10,
                page: routeParams.page || 1,
                orderField: routeParams.orderField || 'notificationDate',
                invertOrder: routeParams.invertOrder || false
            };
            if (routeParams.notificationTypes) {
                filter.notificationTypes = [];
                if (angular.isArray(routeParams.notificationTypes)) {
                    angular.forEach(routeParams.notificationTypes, function (item) {
                        filter.notificationTypes.push(parseInt(item));
                    });
                } else {
                    filter.notificationTypes.push(parseInt(routeParams.notificationTypes));
                }
            }

            return filter;
        }

        function getNotificationsByFilter(filter) {
            var requestUrl = '/api/notifications/?' +
                'page=' + filter.page + '&' +
                'pageSize=' + filter.pageSize + '&' +
                'orderField=' + filter.orderField + '&' +
                'invertOrder=' + filter.invertOrder + '&' +
                'notificationTypes="' + filter.notificationTypes.toString() + '"';
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: requestUrl,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get filtered notification list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        return service;
    }
})();