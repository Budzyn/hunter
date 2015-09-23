(function () {
    "use strict";
    angular
        .module("hunter-app")
        .directive("autofocus", autofocus);

    function autofocus($timeout) {
        return {
            restrict: "A",
            link: function (element) {
                $timeout(function () {
                    element[0].focus();
                });
            }
        }
    }
})();
