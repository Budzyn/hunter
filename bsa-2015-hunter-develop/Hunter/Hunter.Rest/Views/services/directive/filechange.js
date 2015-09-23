(function() {
    'use strict';

    angular
        .module('hunter-app')
        .directive('ngFileChange', FileChange);

    FileChange.$inject = [];

    function FileChange() {
        return {
            'restrict': 'A',
            'link': function(scope, elem, attr) {
                var handler = scope.$eval(attr.ngFileChange);
                elem.bind('change', handler);
            }
        }
    }
})();