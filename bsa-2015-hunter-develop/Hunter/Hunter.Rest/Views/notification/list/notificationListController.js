(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('NotificationListController', NotificationListController);

    NotificationListController.$inject = [
        '$location',
        'AuthService',
        'NotificationHttpService',
        '$routeParams',
        'EnumConstants',
        '$scope'
    ];

    function NotificationListController($location, authService, notificationHttpService, $routeParams, enumConstants, $scope) {
        var vm = this;
        vm.controllerName = 'Notification List';
        vm.notificationTypes = enumConstants.notificationTypes;
        vm.filter = notificationHttpService.convertRouteParamsToFilter($routeParams);
        vm.isModal = false;
        vm.deleteNotification = deleteNotification;
        vm.notify = notify;
        vm.updateData = updateData;
        vm.showSpinner = false;
        vm.totalCount = 0;
        vm.sortBy = [
            { name: 'Notification date (old first)', orderField: 'notificationDate', invertOrder: false },
            { name: 'Notification date (new first)', orderField: 'notificationDate', invertOrder: true }
        ];
        vm.sortAction = vm.sortBy[0];
        var candidateId = $routeParams["cid"];
        vm.notifications = null;
        if (candidateId) {
            vm.isModal = true;
            loadNotifications(candidateId);
        } else {
            loadNotifications();
            $scope.$on('$routeUpdate', loadNotifications);
        }


        $scope.$watch('notificationListCtrl.sortAction', function () {
            console.log(vm.sortAction);
            vm.filter.orderField = vm.sortAction.orderField;
            vm.filter.invertOrder = vm.sortAction.invertOrder;
            updateData();
            loadNotifications();
        }, true);

        vm.pageChangeHandler = function (pageIndex) {
            vm.filter.page = pageIndex;
            loadNotifications();
        };

        function loadNotifications(candidateId) {
            vm.showSpinner = true;
            if (candidateId == undefined) {
                var filter = notificationHttpService.convertRouteParamsToFilter($routeParams);
                notificationHttpService.getNotificationsByFilter(filter).then(function (result) {
                    vm.totalCount = result.totalCount;
                    vm.notifications = result.rows;
                });
            } else {
                notificationHttpService.getCandidateNotifications(candidateId).then(function (result) {
                    vm.notifications = result;
                });
            }
            vm.showSpinner = false;
        }

        function deleteNotification(index) {
            alertify.confirm('Delete notification?', function () {
                if (vm.notifications == null) return;
                if (index > -1) {
                    var id = vm.notifications[index].id;
                    vm.notifications.splice(index, 1);
                    notificationHttpService.deleteNotification(id);
                }
            });
        }

        function notify()
        {
            notificationHttpService.notify();
        }

        function updateData() {
            if (vm.isModal) return;
            $location.search(vm.filter);
        }
    }
})();