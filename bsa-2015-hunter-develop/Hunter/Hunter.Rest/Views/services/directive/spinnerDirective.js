(function () {
    'use strict';
    angular
        .module('hunter-app')
        .directive('spinner', Spinner);

    Spinner.$inject = ['$animate'];

    function Spinner($animate) {
        return {
            template: '<div class="treasure-overlay-spinner-content">' +
                            '<div class="treasure-overlay-spinner-container">' +
                                '<div class="treasure-overlay-spinner"></div>' +
                            '</div>' +
                            '<ng-transclude></ng-transclude>' +
                        '</div>',

            scope: { active: '=spinner' },
            transclude: true,
            restrict: 'A',
            link: link
        };

        function link(scope, iElement) {
            scope.$watch('active', statusWatcher);
            function statusWatcher(active) {
                $animate[active ? 'addClass' : 'removeClass'](iElement, 'treasure-overlay-spinner-active');
            }
        }
    }



})();