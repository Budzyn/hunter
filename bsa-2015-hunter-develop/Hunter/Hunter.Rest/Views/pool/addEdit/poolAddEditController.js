(function () {
    "use strict";

    angular
        .module("hunter-app")
        .controller("PoolAddEditController", PoolAddEditController);

    PoolAddEditController.$inject = [
        "$location",
        "AuthService",
        "HttpHandler",
        "$scope"
    ];

    function PoolAddEditController($location, AuthService, HttpHandler, $scope) {
        var vm = this;
        vm.pageConfig = {};
        vm.id = 0;
        vm.pool = {};
        vm.poolColors = {};
        vm.poolUrl = "";
        vm.addEditFlag = $scope.generalCtrl.selectedPool.id;

        vm.goBack = goBack;
        vm.close = close;

        function goBack() {
            $scope.generalCtrl.link = 'Views/pool/list/list.html';
        }

        function close() {
            $scope.generalCtrl.closeChooser();
        }

        if (vm.addEditFlag > 0) {
            vm.pageConfig.deleteButton = true;
            vm.pageConfig.pageTitle = "Edit a pool";
            vm.pageConfig.postPutButtonValue = "Edit Pool";

            //!!!
            vm.poolUrl = "/api/pool/" + $scope.generalCtrl.selectedPool.id;
//            vm.pool = $scope.generalCtrl.selectedPool;
            HttpHandler.sendRequest({
                url: vm.poolUrl,
                verb: "GET",
                successCallback: function (result) {
                    console.log(result.data);
                    vm.pool = result.data;
                    vm.poolColors = result.data.poolbackground;
                },
                errorCallback: function (result) { console.log(result); }
            });

            vm.poolPostPut = function () {
                HttpHandler.sendRequest({
                    url: vm.poolUrl,
                    verb: "PUT",
                    body: JSON.stringify(vm.pool),
                    successCallback: function (result) {
                        //console.log(result);
                        //                        $location.url("/pool");
                        $scope.generalCtrl.link = 'Views/pool/list/list.html';
                        $scope.generalCtrl.poolChanged(vm.pool);
                    },
                    errorCallback: function (result) { console.log(result); }
                });
            };

            vm.poolDelete = function () {
                HttpHandler.sendRequest({
                    url: vm.poolUrl,
                    verb: "DELETE",
                    successCallback: function (result) {
                        //console.log(result);
                        //                        $location.url("/pool");
                        $scope.generalCtrl.link = 'Views/pool/list/list.html';
                        $scope.generalCtrl.poolRemoved(vm.pool);
                    },
                    errorCallback: function (result) { console.log(result); }
                });
            };
        }
        // add - post
        else {
            vm.pageConfig.deleteButton = false;
            vm.pageConfig.pageTitle = "Add new pool";
            vm.pageConfig.postPutButtonValue = "Add Pool";

            vm.poolUrl = "/api/pool/";

            HttpHandler.sendRequest({
                url: "/api/pool",
                verb: "GET",
                successCallback: function (result) {
                    console.log(result.data);
                    vm.poolColors = result.data[0].poolbackground;
                },
                errorCallback: function (result) { console.log(result); }
            });

            vm.poolPostPut = function (pool) {
                HttpHandler.sendRequest({
                    url: vm.poolUrl,
                    verb: "POST",
                    body: JSON.stringify(vm.pool),
                    successCallback: function (result) {
                        console.log(result);
                        //                        $location.url('/pool');
                        $scope.generalCtrl.link = 'Views/pool/list/list.html';
                    },
                    errorCallback: function (result) { console.log(result); }
                });
            }
        }

        vm.setColor = function ($event, code) {
            vm.pool.color = code;

            vm.currentChild = $event.currentTarget;
            vm.parent = $event.currentTarget.parentNode;
            
            for (var i = 0; i < vm.parent.childNodes.length - 1; i++) {
                vm.parent.childNodes[i].className = "pool_colors_inactive";
            }
            
            vm.currentChild.className = "pool_colors_active";
        }
    }
})();