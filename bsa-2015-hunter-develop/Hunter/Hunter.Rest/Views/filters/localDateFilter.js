(function () {
    'use strict';

    angular
        .module('hunter-app')
        .filter('localDate', function () {
            return function (utcDate) {
//                var dt = new Date(utcDate + 'Z').getTime();
                //                return new Date(dt);
                return new Date(utcDate);
            }
        });
})();