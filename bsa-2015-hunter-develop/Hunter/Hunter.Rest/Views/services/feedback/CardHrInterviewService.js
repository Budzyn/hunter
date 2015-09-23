(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('CardHrInterviewService', CardHrInterviewService);

    CardHrInterviewService.$inject = [];

    function CardHrInterviewService() {
        var service = {
            isDateShow: isDateShow
        };

        function isDateShow(str) {
            if (str === '') {
                return true;
            }

            return false;
        }

        return service;
    }
})();