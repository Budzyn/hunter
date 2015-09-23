(function() {
    'use strict';

    angular
        .module('hunter-app')
        .controller('VacancyAddEditController', VacancyAddEditController);

    VacancyAddEditController.$inject = [
        '$location',
        '$routeParams',
        'AuthService',
        'VacancyHttpService',
        'PoolsHttpService',
        'EnumConstants',
        'localStorageService'
    ];

    function VacancyAddEditController($location, $routeParams, authService, vacancyHttpService, poolsHttpService, enumConstants,
        localStorageService) {
        var vm = this;
        //Here we should write all vm variables default values. For Example:

//        vm.currentPool;
//        vm.color;

        vm.controllerName = 'Add / Edit Vacancy';
        vm.statuses = enumConstants.vacancyStates;
        vm.isNewVacancy = true;
        vm.currentVacancy = {
            id: 0,
            name: '',
            startDate: new Date(),
            endDate: null,
            //location: '',
            status: 0,
            description: '',
            poolNames: [],
            poolColors: {}
        };
        vm.pools = [];
        poolsHttpService.getAllPools().then(function (data) {
            vm.pools = data;
            initializeFields($routeParams);
        });

        vm.submitVacancy = function () {
            if (vacancyHttpService.validateVacancy(vm.currentVacancy)) {
                if (vm.isNewVacancy) {
                    vm.currentVacancy.userLogin = localStorageService.get('authorizationData').userName;
                    vacancyHttpService.addVacancy(vm.currentVacancy);
                } else {
                    vacancyHttpService.updateVacancy(vm.currentVacancy, vm.currentVacancy.id);
                }
                $location.url('/vacancy/list');
            }
        };

        function initializeFields($routeParams) {
            var id = -1;
            if ($routeParams.id) {
                id = $routeParams.id;
            } else {
                return;
            }
            vacancyHttpService.getVacancy(id).then(function (data) {
                vm.isNewVacancy = false;
                vm.currentVacancy = {
                    id: id,
                    name: data.name,
                    startDate: new Date(data.startDate),
                    endDate: data.endDate != null ? new Date(data.endDate) : null,
                    //location: data.location,
                    status: data.status,
                    description: data.description,
                    poolNames: data.poolNames,
                    poolColors: data.poolColors
                };
                
//                //!!!!!
//                for (var i in vm.pools) {
//                    var pool = vm.pools[i];
//                    if (pool.id == vm.currentVacancy.poolId) {
//                        vm.currentPool = pool;
//                        break;
//                    }
//                }
//                vm.color[vm.currentPool.name.toLowerCase()] = vm.currentPool.color;
//                //!!!!!!

                console.log('Original date - ' + data.endDate);
                console.log('Converted data', new Date(data.endDate));
            });
            //if (vacancy != null) {
            //    console.log(vacancy.data);
            //    vm.isNewVacancy = false;
            //    vm.currentVacancy = {
            //        id: id,
            //        Name: vacancy.Name,
            //        StartDate: vacancy.StartDate,
            //        EndDate: vacancy.EndDate,
            //        Location: vacancy.Location,
            //        Status: vacancy.Status,
            //        Description: vacancy.Description,
            //        PoolId: vacancy.PoolId
            //    };
            //};
        }
        //Here we should write all signatures for user actions callback method, for example,

        //(function() {
        //    // This is function for initialization actions, for example checking auth
        //    if (authService.isLoggedIn()) {
        //    // Can Make Here Any Actions For Data Initialization, for example, http queries, etc.
        //    } else {
        //        $location.url('/login');
        //    }
        //})();


        // Here we should write any functions we need, for example, body of user actions methods.

    }
})();