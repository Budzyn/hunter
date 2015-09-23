(function () {
    'use strict';
    angular
        .module('hunter-app')
        .directive('activityType', function () {
            return function (scope, element, attrs) {
                    var icon = '';
                    switch (attrs.activityType) {
                        case '0':
                            icon = 'fa-users';
                            break;
                        case '1':
                            icon = 'fa-suitcase';
                            break;
                        case '2':
                            icon = 'fa-comment';
                            break;
                        case '3':
                            icon = 'fa-user-plus';
                            break;
                        case '4':
                            icon = 'fa-list';
                            break;
                        case '5':
                            icon = 'fa-bookmark-o';
                            break;
                        case '6':
                            icon = 'fa-file-text';
                            break;
                        case '7':
                            icon = 'fa-file';
                            break;
                        case '8':
                            icon = 'fa-file-image-o';
                            break;
                        default:
                            icon = 'fa-question';
                    }

                    element.html("<i class=\"fa " + icon + " \"></i>");
            }
        });

})();