(function() {
    'use strict';

    angular
        .module('hunter-app')
        .factory('CandidateService', CandidateService);

    CandidateService.$inject = [];

    function CandidateService() {
        var service = {
            changeTemplate: changeTemplate,
            convertRouteParamsToFilter: convertRouteParamsToFilter
            },
            candidateProfileViewsUrl = 'Views/candidate/profile/';

        function changeTemplate(templateName) {
            switch (templateName.toLowerCase()) {
                case 'applications':
                    return candidateProfileViewsUrl + 'profileApplications.html';
                case 'special notes':
                    return candidateProfileViewsUrl + 'profileSpecialNotes.html';
                case 'tests':
                    return candidateProfileViewsUrl + 'profileTests.html';
                case 'interviews':
                    return candidateProfileViewsUrl + 'profileInterviews.html';
                default:
                    return '';
            }
        }


        function convertRouteParamsToFilter(routeParams) {
           var filter = {
                pools: routeParams.pools || [],
                inviters: routeParams.inviters || [],
                statuses: [],
                shortListed: routeParams.shortListed=='true' ? true : false,
                search: routeParams.search || '',
                pageSize: parseInt(routeParams.pageSize) || 10,
                currentPage: routeParams.currentPage || 1,
                order: routeParams.order || 'AddDate_asc'
            };


            if (routeParams.statuses) {
                if (angular.isArray(routeParams.statuses)) {
                    angular.forEach(routeParams.statuses, function(item) {
                        filter.statuses.push(parseInt(item));
                    });
                } else {
                    filter.statuses.push(parseInt(routeParams.statuses));
                }
            }

            return filter;
        }

        return service;
    }
})();