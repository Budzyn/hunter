(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('TestService', TestService);

    TestService.$inject = [
       
    ];

    function TestService() {
        var service = {
            getUrlOnTest: getUrlOnTest
        }

        function getUrlOnTest(vacancyId,candidateId) {
            var urlOnTest = '#/vacancy/' + vacancyId + '/candidate/' + candidateId + '?tab=test';
            return urlOnTest;
        }

        return service;

    }
})();