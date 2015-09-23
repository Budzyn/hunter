(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('FileListController', FileListController);

    FileListController.$inject = [
        '$location',
        'AuthService',
        'FileHttpService'
    ];

    function FileListController($location, authService, fileHttpService) {
        var vm = this;
        //Here we should write all vm variables default values. For Example:
        vm.controllerName = 'List of files';

        vm.files = [];
        fileHttpService.getFiles().then(function (data) {
            vm.files = data;
        });
        //(function() {
        //    // This is function for initialization actions, for example checking auth
        //    if (authService.isLoggedIn()) {
        //    // Can Make Here Any Actions For Data Initialization, for example, http queries, etc.
        //    } else {
        //        $location.url('/login');
        //    }
        //})();
        // Here we should write any functions we need, for example, body of user actions methods.

    }
})();