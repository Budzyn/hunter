(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('RegisterController', RegisterController);

    RegisterController.$inject = [
        '$scope',
        'AuthService',
        'UserRoleHttpService'
    ];


    function RegisterController($scope, AuthService, UserRoleHttpService) {
        var vm = this;
        // $scope.roles = [];

        vm.states = [
            { id: 0, name: 'State1' },
            { id: 1, name: 'State2' }
        ];

        UserRoleHttpService.getUserRoles().then(function (result) {
            vm.roles = result;
        });


        vm.newUser = {
            email: '',
            password: '',
            confirmPassword: '',
            state: 0,
            roleId: 0
        }

        vm.register = function () {
            console.log('asada');
            AuthService.saveRegistration(vm.newUser);
        }

    };


})();
