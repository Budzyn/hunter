(function () {
    'use strict';

    angular.module('hunter-app')
        .factory('LonglistService', LonglistService);

    LonglistService.$inject = [
        'HttpHandler',
        '$q',
	    '$location',
        '$rootScope'
    ];

    function LonglistService(httpHandler, $q, $location, $rootScope) {
        var service = {
            changeTemplate: changeTemplate,
            isCurrentTabEmpty: isCurrentTabEmpty,
            convertRouteParamsToFilter: convertRouteParamsToFilter
        }

        // click on candidate item shows candidates preview

        var viewsPath = '/Views/vacancy/longlist/';

        function changeTemplate(templateRoute) {
            switch (templateRoute.toLowerCase()) {
                case 'overview':
                    return viewsPath + 'previewOverview.html';
                case 'specialnotes':
                    return viewsPath + 'previewSpecialNotes.html';
                case 'appresults':
                    return viewsPath + 'previewAppResults.html';

                default:
                    return '';
            }
        }

        function isCurrentTabEmpty(route, overviews, notes) {
            switch (route.toLowerCase()) {
                case 'overview':
                    return overviews.length > 0 ? true : false;
                case 'specialnotes':
                    return notes.length > 0 ? true : false;
                case 'appresults':
                    return true;               
            default:
                return false;
            }
        }

        function convertRouteParamsToFilter(routeParams) {
            var filter = {
                search: routeParams.search || '',
                shortlisted: routeParams.shortlisted || false,
                stages: [],
                salary: [],
                hr: [],
                currentPage: routeParams.currentPage || 1,
                pageSize: parseInt(routeParams.pageSize) || 10,
                order: routeParams.order || 'AddDate_desc'

            };


            if (routeParams.stages) {
                if (angular.isArray(routeParams.stages)) {
                    angular.forEach(routeParams.stages, function(item) {
                        filter.stages.push(parseInt(item));
                    });
                } else {
                    filter.stages.push(parseInt(routeParams.stages));
                }
            }

            if (routeParams.hr) {
                if (angular.isArray(routeParams.hr)) {
                    angular.forEach(routeParams.hr, function(item) {
                        filter.hr.push(item);
                    });
                } else {
                    filter.hr.push(routeParams.hr);
                }
            }

            return filter;
        }

        return service;
    }
})();