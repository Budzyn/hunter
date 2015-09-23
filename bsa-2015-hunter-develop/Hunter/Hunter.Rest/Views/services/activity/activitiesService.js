(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('ActivityService', ActivityService);

    ActivityService.$inject = [
        'HttpHandler',
        'EnumConstants'
    ];

    function ActivityService(httpHandler, EnumConstants) {
        var vm = this;

        vm.activityTags = EnumConstants.activityTags;

        var service = {
            getFilterUsers: getFilterUsers,
            getFilterTags: getFilterTags,
            convertUrlToFilter: convertUrlToFilter
        };
        
        function getFilterUsers(f) {
            vm.filterUsers = [];
            for (var i = 0; i < f.length; i++) {
                if (f[i].filterId === 1) {
                    vm.filterUsers.push(f[i]);
                }
            }

            return vm.filterUsers;
        }
    
        function getFilterTags(f) {
            vm.filterTags = [];
            for (var i = 0; i < f.length; i++) {
                if (f[i].filterId === 0) {
                    f[i].name = vm.activityTags[f[i].optionId];
                    vm.filterTags.push(f[i]);
                }
            }

            return vm.filterTags;
        }

        function convertUrlToFilter(url) {
            var filter = {
                pageSize: url.pageSize || EnumConstants.itemsOnPage.defaultItem,
                currentPage: url.currentPage || 1,
                users: [],
                tags: [] 
            }
            
            if(url.tags)
            {
                if(angular.isArray(url.tags)){
                    angular.forEach(url.tags, function(item){
                        filter.tags.push(item);
                    });
                }
                else {
                    filter.tags.push(url.tags);
                }
            }
            
            if(url.users)
            {
                if(angular.isArray(url.users)){
                    angular.forEach(url.users, function(item){
                        filter.users.push(item);
                    });
                }
                else {
                    filter.users.push(url.users);
                }
            }
       
            return filter;
        }

        return service;
    }
})();