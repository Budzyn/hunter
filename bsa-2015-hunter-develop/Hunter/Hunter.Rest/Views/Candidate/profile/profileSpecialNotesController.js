(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('ProfileSpecialNotesController', ProfileSpecialNotesController);

    ProfileSpecialNotesController.$inject = [];

    function ProfileSpecialNotesController() {
        var vm = this;
        vm.templateName = 'Special Notes';
    }
})();