(function () {
    "use strict";

    angular
        .module("hunter-app")
        .controller("PoolListController", PoolListController);

    PoolListController.$inject = [
        "$location",
        "AuthService",
        "HttpHandler",
        '$scope'
    ];

    function PoolListController($location, AuthService, HttpHandler, $scope) {
        var vm = this;

        vm.editPool = editPool;
        vm.choosePool = choosePool;
        vm.close = close;

        (function() {
            HttpHandler.sendRequest({
                url: "/api/pool",
                verb: "GET",
                successCallback: function(result) {
                    console.log(result.data);
                    vm.poolsList = result.data;

                    var names = $scope.generalCtrl.getCandidatePools();
                    vm.poolsList.forEach(function (pool) {
                        if (names.indexOf(pool.name) != -1) {
                            pool.isChecked = true;
                        } else {
                            pool.isChecked = false;
                        }
                    });
                },
                errorCallback: function(result) { console.log(result); }
            });
        })();

        function editPool(pool) {
            $scope.generalCtrl.link = 'Views/pool/addEdit/addEdit.html';
            $scope.generalCtrl.selectedPool = pool;
        }

        function choosePool(pool) {
            if ($scope.generalCtrl.hasRestriction()) {
                $scope.generalCtrl.changePool(pool, pool.isChecked);
                pool.isChecked = !pool.isChecked;
                vm.poolsList.forEach(function(p) {
                    if (p.id != pool.id)
                        p.isChecked = false;
                });
            } else {
                $scope.generalCtrl.selectPool(pool);
                pool.isChecked = !pool.isChecked;
            }
        }

        function close() {
            $scope.generalCtrl.closeChooser();
        }
    }
})();