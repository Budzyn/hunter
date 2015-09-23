(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('ProfileTestsController', ProfileTestsController);

    ProfileTestsController.$inject = [];

    function ProfileTestsController() {
        var vm = this;
        vm.templateName = 'Tests';
    }
})();