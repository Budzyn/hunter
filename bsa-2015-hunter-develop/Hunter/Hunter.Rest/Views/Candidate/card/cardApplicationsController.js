(function() {
    'use strict';

    angular
        .module('hunter-app')
        .controller('cardApplicationsController', cardApplicationsController);

    cardApplicationsController.$inject = [
        'LonglistHttpService',
        'EnumConstants',
        '$routeParams'
      
    ];

    function cardApplicationsController(longlistHttpService, EnumConstants, $routeParams) {
        var vm = this;

        vm.appResults = [];
	    vm.stages = EnumConstants.cardStages;
	    vm.candidateId = $routeParams.cid;

    	(function () {
        	longlistHttpService.getAppResults(vm.candidateId).then(function (result) {
                vm.appResults = result;
            });

        })();


    };

})();