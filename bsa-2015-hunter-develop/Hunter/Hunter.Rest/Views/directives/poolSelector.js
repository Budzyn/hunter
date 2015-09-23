(function() {
    angular
        .module('hunter-app')
        .directive('poolSelector', PoolSelector);

    PoolSelector.$inject = [];

    function PoolSelector() {
        return {
            restrict: 'EA',
            link: function (scope, elem, attr, ctrl) {
                if(scope.poolReadonly === 'false'){
                    $(document).click(function (event) {
                        if ($(event.target).closest('#addPoolBtn').length != 0) {
                            return;
                        }

                        if ($(event.target).hasClass('pool-label')) {
                            return;
                        }

                        //edit-link-search
                        //back-search
                        //add-pool-btn-search
                        if (hasClass(event.target) != '' && $(hasClass(event.target)).length == 0) {
                            return;
                        }

                        if ($(event.target).closest('#selectPoolMain').length != 0 && scope.poolSelectorCtrl.show)
                            return;

                        scope.poolSelectorCtrl.close();
                    });
                }

                function hasClass(elem) {
                    var search = ['edit-link-search', 'back-search', 'add-pool-btn-search'];
                    var res = '';

                    search.forEach(function(s) {
                        if ($(elem).hasClass(s)) {
                            res = s;
                        }
                    });

                    return res;
                }

            },
            scope: {
                'candidate': '=candidate',
                'poolReadonly': '@poolReadonly',
                'poolShort': '@poolShort',
                'poolLazy': '@poolLazy',
                'poolOne': '@poolOne'
            },
            controllerAs: 'poolSelectorCtrl',
            controller: ['$scope', 'CandidateHttpService', function ($scope, CandidateHttpService) {
                var vm = this;
                vm.show = false;
                $scope.wid = $scope.poolReadonly === 'true' ? '100%' : 'auto';
                vm.firstUse = true;
//                vm.poolSpinner = false;

                vm.toggleShow = function () {
                    vm.show = !vm.show;
                    vm.firstUse = false;
                }

                //close selector
                vm.close = function() {
                    vm.show = false;
                    $scope.$apply();
                }
                //Start check pool
                vm.addPoolToCandidate = function (pool) {
//                    vm.poolSpinner = true;

                    if ($scope.candidate.poolNames.indexOf(pool.name) == -1) {
                        if ($scope.poolLazy === 'true') {
                            selectPool(pool);
                        }else{
                            CandidateHttpService.addCandidatePool($scope.candidate.id, pool.id)
                                .then(function(data) {
                                    selectPool(pool);
                                });
                        }
                    } else {
                        if ($scope.poolLazy === 'true') {
                            unselectPool(pool);
                        }else{
                            CandidateHttpService.removeCandidatePool($scope.candidate.id, pool.id)
                                .then(function(data) {
                                    unselectPool(pool);
                                });
                        }
                    }
//                    vm.poolSpinner = false;
                }
                //add pool from candidate poolList
                function selectPool(pool) {
                    $scope.candidate.poolNames.push(pool.name);

                    if (!(pool.name in $scope.candidate.poolColors)) {
                        $scope.candidate.poolColors[pool.name.toLowerCase()] = pool.color;
                    }
                }
                //delete pool from candidate poolList
                function unselectPool(pool) {
                    var index = $scope.candidate.poolNames.indexOf(pool.name);
                    $scope.candidate.poolNames.splice(index, 1);
                    delete $scope.candidate.poolColors[pool.name.toLowerCase()];
                }
                //End check label

                //apply changes from selector
                vm.syncPools = function(pool, oldPool) {
                    var index = $scope.candidate.poolNames.indexOf(oldPool.name);
                    if(index != -1){
                        $scope.candidate.poolNames[index] = pool.name;
                        delete $scope.candidate.poolColors[oldPool.name.toLowerCase()];
                        $scope.candidate.poolColors[pool.name.toLowerCase()] = pool.color;
                    }
                }

                //pool was deleted in checker
                vm.removePoolFromCandidate = function(pool) {
                    var index = $scope.candidate.poolNames.indexOf(pool.name);
                    if (index != -1) {
                        $scope.candidate.poolNames.splice(index, 1);
                        delete $scope.candidate.poolColors[pool.name];
                    }

                    if (vm.labelClicked != '') {
                        vm.labelClicked = '';
                        vm.show = false;
                    }
                }

                //get all pooof candidate
                vm.candidatePools = function() {
                    return $scope.candidate.poolNames;
                }
                
                vm.onLabelClick = function(pool) {
                    vm.show = true;
                    vm.firstUse = false;
                }

                vm.isPoolOneRestrict = function() {
                    return $scope.poolOne === 'true';
                }

                vm.poolOneSelect = function (pool, isChecked) {
                    if (isChecked) {
                        $scope.candidate.poolNames = [];
                        $scope.candidate.poolColors = {};
                        $scope.candidate.id = 0;
                    }else{
                        $scope.candidate.poolNames = [pool.name];
                        $scope.candidate.poolColors = {};
                        $scope.candidate.poolColors[pool.name.toLowerCase()] = pool.color;
                        $scope.candidate.id = pool.id;
                    }
                }
            }],
            template: 
                '<div style="width: auto; display: block;">' +
                    '<div style="width: {{wid}};" class="pool-label-container">' +
                        '<div ng-if="poolShort === \'false\' && poolReadonly === \'false\'">' +
                            '<div ng-repeat="pool in candidate.poolNames" class="pool-label" ng-click="poolSelectorCtrl.onLabelClick(pool)" style="background-color: {{candidate.poolColors[pool.toLowerCase()]}};">' +
                            '{{pool}}</div>' +

                            '<button ng-if="poolReadonly === \'false\'" style="margin-left: 5px;" id="addPoolBtn" ng-click="poolSelectorCtrl.toggleShow()" class="pool-selector-btn btn btn-default"><i class="fa fa-plus pool-selector-btn-icon"></i></button>' +
                            '<div ng-if="!poolSelectorCtrl.firstUse && poolReadonly === \'false\'" id="selectPoolMain" ng-show="poolSelectorCtrl.show" class="pool-widget-container" ng-controller="PoolGeneralController as generalCtrl">' +
                                '<div style="width: 380px;" ng-include="generalCtrl.link"></div>' +
                            '</div>' +
                        '</div>' +

                        '<div ng-if="poolShort === \'false\' && poolReadonly === \'true\'">' +
                            '<div ng-repeat="pool in candidate.poolNames" class="pool-label" style="background-color: {{candidate.poolColors[pool.toLowerCase()]}};">' +
                            '{{pool}}</div>' +

                            '<button ng-if="poolReadonly === \'false\'" style="margin-left: 5px;" id="addPoolBtn" ng-click="poolSelectorCtrl.toggleShow()" class="pool-selector-btn btn btn-default"><i class="fa fa-plus pool-selector-btn-icon"></i></button>' +
                            '<div ng-if="!poolSelectorCtrl.firstUse && poolReadonly === \'false\'" id="selectPoolMain" ng-show="poolSelectorCtrl.show" class="pool-widget-container" ng-controller="PoolGeneralController as generalCtrl">' +
                                '<div style="width: 380px;" ng-include="generalCtrl.link"></div>' +
                            '</div>' +
                        '</div>' +

                        '<div ng-if="poolShort === \'true\'">' +
                            '<div ng-if="poolShort === \'true\'" ng-repeat="pool in candidate.poolNames" class="pool-label-short" style="background-color: {{candidate.poolColors[pool.toLowerCase()]}};">' +
                            '</div>' +

                            '<button ng-if="poolReadonly === \'false\'" style="margin-left: 5px;" id="addPoolBtn" ng-click="poolSelectorCtrl.toggleShow()" class=" btn btn-default pool-selector-btn"><i class="fa fa-plus pool-selector-btn-icon"></i></button>' +
                            '<div ng-if="!poolSelectorCtrl.firstUse && poolReadonly === \'false\'" id="selectPoolMain" ng-show="poolSelectorCtrl.show" class="pool-widget-container" ng-controller="PoolGeneralController as generalCtrl">' +
                                '<div style="width: 380px;" ng-include="generalCtrl.link"></div>' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
//                    '<button ng-if="poolReadonly === \'false\'" style="margin-left: 5px;" id="addPoolBtn" ng-click="poolSelectorCtrl.toggleShow()" class=" btn btn-default"><i class="fa fa-plus"></i></button>' +
//                    '<div ng-if="!poolSelectorCtrl.firstUse && poolReadonly === \'false\'" id="selectPoolMain" ng-show="poolSelectorCtrl.show" class="pool-widget-container" ng-controller="PoolGeneralController as generalCtrl">' +
//                        '<div style="width: 380px;" ng-include="generalCtrl.link"></div>' +
//                    '</div>' +
                '</div>'
        }
    }
})()