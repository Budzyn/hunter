(function() {
    angular
        .module('hunter-app')
        .controller('PoolGeneralController', PoolGeneralController);

    PoolGeneralController.$inject = [
        '$scope'
    ];

    function PoolGeneralController($scope) {
        var vm = this;
        vm.link = 'Views/pool/list/list.html';
        vm.selectedPool = null;

        vm.selectPool = selectPool;
        vm.closeChooser = closeChooser;
        vm.poolChanged = poolChanged;
        vm.poolRemoved = poolRemoved;
        vm.getCandidatePools = getCandidatePools;
        vm.hasRestriction = hasRestriction;
        vm.changePool = changePool;

        function selectPool(pool) {
            if ($scope.poolSelectorCtrl != undefined) {
                $scope.poolSelectorCtrl.addPoolToCandidate(pool);
            }
        }

        function closeChooser() {
            if ($scope.poolSelectorCtrl != undefined) {
                $scope.poolSelectorCtrl.toggleShow();
            } else {
                vm.link = '';
            }
        }

        function poolChanged(pool) {
            if ($scope.poolSelectorCtrl != undefined) {
                $scope.poolSelectorCtrl.syncPools(pool, vm.selectedPool);
            }
        }

        function poolRemoved(pool) {
            if ($scope.poolSelectorCtrl != undefined) {
                $scope.poolSelectorCtrl.removePoolFromCandidate(pool);
            }
        }

        function getCandidatePools() {
            if ($scope.poolSelectorCtrl != undefined) {
                return $scope.poolSelectorCtrl.candidatePools();
            } else {
                return [];
            }
        }

        function hasRestriction() {
            if ($scope.poolSelectorCtrl != undefined) {
                return $scope.poolSelectorCtrl.isPoolOneRestrict();
            } else {
                return false;
            }
        }

        function changePool(pool, isChecked) {
            if ($scope.poolSelectorCtrl != undefined) {
                $scope.poolSelectorCtrl.poolOneSelect(pool, isChecked);
            }
        }
    }
})()