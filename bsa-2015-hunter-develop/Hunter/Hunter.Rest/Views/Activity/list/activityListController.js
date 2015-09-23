(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('ActivityListController', ActivityListController);

    ActivityListController.$inject = [
        '$location',
        '$scope',
        'AuthService',
        'ActivityHttpService',
        'localStorageService',
        '$odataresource',
        '$odata',
        'ActivityService',
        'EnumConstants'
    ];

    function ActivityListController($location, $scope, authService, activityHttpService, localStorageService, $odataresource, $odata, activityService, EnumConstants) {
        var vm = this;
        //Here we should write all vm variables default values. For Example:
        vm.name = "Activity";
        vm.itemsOnPage = EnumConstants.itemsOnPage;

        // TODO: Use camelCase
        $scope.IndexCtrl.amount = 0;

        

        //(function() {
        //    // This is function for initialization actions, for example checking auth
        //    if (authService.isLoggedIn()) {
        //    // Can Make Here Any Actions For Data Initialization, for example, http queries, etc.
        //    } else {
        //        $location.url('/login');
        //    }
        //})();

        vm.getActivitiesOdata = getActivitiesOdata;
        vm.update = update;
        vm.activitiesList = [];

        vm.filterSpinner = false;        

        vm.filter = {
            users: [],
            tags: [],
            currentPage: 1,
            pageSize: vm.itemsOnPage.defaultItem
        };
        vm.skip = 0;
        var predicate;        

        // get filter options 
        (function () {
            vm.filterSpinner = true;
            vm.filterUsers = [];
            vm.filterTags = [];
            activityHttpService.getFilterOptions().then(function (data) {
                vm.filterUsers = activityService.getFilterUsers(data);
                vm.filterTags = activityService.getFilterTags(data);
                vm.filterSpinner = false;
            });
            vm.filter = activityService.convertUrlToFilter($location.search());
            //console.log(vm.filter);
            vm.getActivitiesOdata(vm.filter);
            //activityService.convertUrlToFilter($location.search());
        })();

        vm.listSpinner = false;

        function getActivitiesOdata(filter) {
            vm.listSpinner = true;
            //console.log(vm.filter);
           /* var acts = activitiesOdata.odata()
                                        .withInlineCount()
                                        .take(vm.filter.pageSize)
                                        .skip(vm.skip)
                                        .filter(predicate)
                                        .orderBy('Time', 'desc')
                                        .query(function () {
                                            vm.activitiesList = acts.items;
                                            vm.totalItems = acts.count;
                                            vm.listSpinner = false; 
                                        });*/
            activityHttpService.getActivitiesByFilter(filter).then(function (result) {
                vm.activitiesList = result.items;
                vm.totalItems = result.count;
                vm.listSpinner = false;
            });
        };
        
        function update() {
            $location.search(vm.filter);
        }

        $scope.$on('$routeUpdate', function () {
            vm.getActivitiesOdata(vm.filter);
        });
        // filters
        //$scope.$watch('ActivityListCtrl.filter', function () {
            
        //}, true);    

        vm.activityList;
        // Here we should write any functions we need, for example, body of user actions methods.
        // TODO: Initialization Should Be covered with self invoke function
        activityHttpService.getActivityList(function (data) {
            vm.activityList = data.data;
            //console.log(data.data);

            if(isNewActivityPresent(vm.activityList)){
                saveLastViewdActivityId(vm.activityList);
            }

        });

        function saveLastViewdActivityId(activityList) {
            if (activityList == null || activityList.length == 0) {
                return;
            }

            activityList.sort(sortFunc);

            var lastViewdId = activityList[0].id;

            localStorageService.set('lastViewdActivityId', lastViewdId);

            activityHttpService.saveLastActivityId(function(response) {
                //console.log(response.data);
            }, 
            lastViewdId);
        }

        // TODO: Data Functions (not user event functions) Should Be In Services
        function isNewActivityPresent(activityList) {
            if (activityList == null || activityList.length == 0) {
                return;
            }

            activityList.sort(sortFunc);

            var lastId = localStorageService.get('lastViewdActivityId');

            if (lastId != activityList[0].id) {
                return true;
            } else {
                return false;
            }
        }
    }

    // TODO: Data Functions (not user event functions) Should Be In Services
    function sortFunc(a, b) {
        var aDate = new Date(a.time);
        var bDate = new Date(b.time);

        if (aDate > bDate) {
            return -1;
        } else if (aDate < bDate) {
            return 1;
        } else {
            return 0;
        }
    }

})();