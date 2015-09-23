(function () {
    "use strict";

    angular
        .module("hunter-app")
        .controller("UserAddEditController", userAddEditController);

    userAddEditController.$inject = [
        "$location",
        "AuthService",
        "UserProfileService",
        "$routeParams"
    ];

    function userAddEditController($location, authService, service, $routeParams) {
        var vm = this;
        vm.editingMode = (typeof $routeParams.id == 'string');
        vm.entityId = 0;
        if (vm.editingMode) {
            vm.entityId = $routeParams.id;
        } else {

        };
        vm.entity = {}; //{ login: "antonv@bs.com", alias: "AV", position: ".Net Developer", added: "01.12.2013" };

        vm.loadUser = function () {
            service.getUserProfile(vm.entityId, function (response) {
                vm.entity = response.data;
            });
        }

        vm.save = function () {
            service.updateUserProfile(vm.entity, function onSuccess(response) {
                if (vm.entityId == 0) {
                    vm.entity = response.data;
                    vm.entityId = response.data.id;
                }
            });
        }

        vm.delete = function () {
            service.deleteUserProfile(vm.entityId, function (response) {
                $location.path("/user");
            });
        }

        if (vm.editingMode) {
            vm.loadUser();
        }
    }
})();