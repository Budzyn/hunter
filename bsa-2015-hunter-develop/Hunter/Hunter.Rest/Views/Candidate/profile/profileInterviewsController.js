(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('ProfileInterviewsController', ProfileInterviewsController);

    ProfileInterviewsController.$inject = [];

    function ProfileInterviewsController() {
        var vm = this;
        vm.templateName = 'Interviews';
    }
})();