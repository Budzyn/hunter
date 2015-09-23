(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('UserListController', userListController);

    userListController.$inject = [
        '$scope',
        '$location',
        '$routeParams',
        'AuthService',
        'UserProfileService' //change on your service
    ];

    function userListController($scope, $location, $routeParams, authService, service) {
        var vm = this;
        //Here we should write all vm variables default values. For Example:
        vm.controllerName = 'This is users page';
        vm.profilesList = [{ Alias: "Loading ...", Position:"Please wait" }];

        //(function() {
        //    // This is function for initialization actions, for example checking auth
        //    if (authService.isLoggedIn()) {
        //    // Can Make Here Any Actions For Data Initialization, for example, http queries, etc.
        //    } else {
        //        $location.url('/login');
        //    }
        //})();
       

        // Here we should write any functions we need, for example, body of user actions methods.
        var updatePage = function (newPageNum) {
            if (typeof newPageNum !== "number" || newPageNum <= 0) {
                newPageNum = 1;
            }
           vm.page = newPageNum;
        }

        vm.loadUsers = function () {
            $location.search('page', vm.page);
            service.getUserProfileList(vm.page, function (response) {
                vm.profilesList = response.data;
            });
        }

        $scope.$watch(function() {return vm.page;}, vm.loadUsers);

        //vm.watch('page', vm.loadUsers);
        
        vm.nextPage = function () {
            updatePage(vm.page + 1);
        }

        vm.prevPage = function () {
            updatePage(vm.page - 1);
        }

        updatePage($routeParams.page);
    }
})();