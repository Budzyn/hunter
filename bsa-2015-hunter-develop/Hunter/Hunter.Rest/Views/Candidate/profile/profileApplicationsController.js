(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('ProfileApplicationsController', ProfileApplicationsController);

    ProfileApplicationsController.$inject = [];

    function ProfileApplicationsController() {
        var vm = this;
        vm.templateName = 'Applications';
    }
})();