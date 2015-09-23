
﻿(function () {

    'use strict';

    angular
        .module('hunter-app')
        .controller('VacancyListController', vacancyListController);

    vacancyListController.$inject = [
        '$scope',
        '$filter',
        'VacancyHttpService',
        'PoolsHttpService',
        'EnumConstants',
        '$location'
    ];

    function vacancyListController($scope, $filter, vacancyHttpService, poolsHttpService, enumConstants, $location) {
        var vm = this;

        vm.filterParams = {
            page: 1,
            pageSize: 25,
            sortColumn: 'startDate',
            reverceSort: true,
            filter: '',
            pool: [],
            status: [],
            addedBy: []
        };

        vm.totalCount = 0;
        vm.pools = [];
        poolsHttpService.getAllPools().then(function (data) {
            vm.pools = data;
        });

        vm.statuses = enumConstants.vacancyStates;

        vm.sortBy = [
            { name: "Add Date (new first)", reverseSort: true, sortColumn: "startDate" },
            { name: "Add Date (old first)", reverseSort: false, sortColumn: "startDate" },
            { name: "Name (A-Z)", reverseSort: false, sortColumn: "name" },
            { name: "Name (Z-A)", reverseSort: true, sortColumn: "name" }
        ];
        vm.sortAction = {};

        vm.vacancies = [];

        vm.pageChangeHandler = function (pageIndex) {
            vm.filterParams.page = pageIndex;
            vm.loadDataByParams(vm.filterParams);
        };

        vm.loadDataByParams = function () {
            vm.filterParams.sortColumn = vm.sortAction.sortColumn;
            vm.filterParams.reverceSort = vm.sortAction.reverseSort;
            $location.search(vm.filterParams);
            vacancyHttpService.getFilteredVacancies(vm.filterParams).then(function (result) {
                vm.vacancies = result.rows;
                console.log(vm.vacancies);
                vm.totalCount = result.totalCount;
            });
        }

        $scope.$watch('vacancyListCtrl.filterParams', function () {
            vm.loadDataByParams();
        }, true);

        $scope.$watch('vacancyListCtrl.sortAction', function () {
            vm.loadDataByParams();
        }, true);

       
        vacancyHttpService.getAddedByList().then(function (result) {
            vm.adders = result;
        });

        if (typeof $location.search().page === 'undefined') {
            //vm.pushPopItem(vm.statuses[1].id, vm.filterParams.status);
            vm.filterParams.status.push(vm.statuses[1].id);
            vm.sortAction = vm.sortBy[0];
            console.log(true);
        } else {
            convertRouteParamsToFilter($location.search());
            console.log(vm.filterParams);
            console.log(false);
        }

        vm.loadDataByParams();



        function convertRouteParamsToFilter(routeParams) {
            var filter = {
                pool: [],
                addedBy: [],
                status: [],
                //shortListed: routeParams.shortListed == 'true' ? true : false,
                filter: routeParams.filter || '',
                pageSize: parseInt(routeParams.pageSize) || 25,
                page: routeParams.page || 1,
                sortColumn: routeParams.sortColumn || 'startDate',
                reverceSort: routeParams.reverceSort || true
            };

            if (routeParams.pool) {
                if (angular.isArray(routeParams.pool)) {
                    angular.forEach(routeParams.pool, function (item) {
                        filter.pool.push(parseInt(item));
                    });
                } else {
                    filter.pool.push(parseInt(routeParams.pool));
                }
            };
            if (routeParams.addedBy) {
                if (angular.isArray(routeParams.addedBy)) {
                    angular.forEach(routeParams.addedBy, function (item) {
                        filter.addedBy.push(parseInt(item));
                    });
                } else {
                    filter.addedBy.push(parseInt(routeParams.addedBy));
                }
            };
            if (routeParams.status) {
                if (angular.isArray(routeParams.status)) {
                    angular.forEach(routeParams.status, function (item) {
                        filter.status.push(parseInt(item));
                    });
                } else {
                    filter.status.push(parseInt(routeParams.status));
                }
            };
            vm.filterParams = filter;
            if (filter.sortColumn === 'startDate') {
                if (filter.reverceSort === true) {
                    vm.sortAction = vm.sortBy[0];
                } else {
                    vm.sortAction = vm.sortBy[1];
                }
            }
            if (filter.sortColumn === 'name') {
                if (filter.reverceSort === true) {
                    vm.sortAction = vm.sortBy[3];
                } else {
                    vm.sortAction = vm.sortBy[2];
                }
            }
        }
    }
})();