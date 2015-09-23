(function () {
    "use strict";

    angular
        .module("hunter-app")
        .controller("AddEditNotificationController", AddEditNotificationController);

    AddEditNotificationController.$inject = [
        "$location",
        "AuthService",
        "NotificationHttpService",
        '$routeParams',
        'EnumConstants'
    ];

    function AddEditNotificationController($location, authService, notificationHttpService, $routeParams, enumConstants) {
        var vm = this;
        vm.controllerName = 'Add / Edit notification';
        vm.addNotification = addNotification;
        vm.notificationTypes = enumConstants.notificationTypes;
        vm.notificationType = vm.notificationTypes[0];
        vm.newNotification = {
            id: 0,
            candidateId: $routeParams.cid,
            notificationDate: new Date(),
            notificationType: 0,
            message: '',
            isSent: false,
            isShown: false,
            userLogin: authService.login
        };

        function addNotification() {
            vm.newNotification.notificationType = vm.notificationType.id;
            notificationHttpService.addNotification(vm.newNotification);
        };
    };
})();