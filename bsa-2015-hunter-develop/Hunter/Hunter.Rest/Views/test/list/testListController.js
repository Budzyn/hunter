(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('TestListController', TestListController);

    TestListController.$inject = [
        'TestHttpService',
        'TestService',
        '$scope'
    ];

    function TestListController(TestHttpService, TestService, $scope) {
        var vm = this;
        vm.tests = [];
        vm.getUrlOnTest = getUrlOnTest;
        vm.changeCheckedTest = changeCheckedTest;
        vm.getClassForTr = getClassForTr;

        LoadTests();
        
        function LoadTests() {
            TestHttpService.getTestsNotChecked().then(function (result) {
                console.log(result);
                vm.tests = result;
            });
        }        

        function getUrlOnTest(vacancyId, candidateId) {
            return TestService.getUrlOnTest(vacancyId, candidateId);
        }

        function changeCheckedTest(testId) {
            TestHttpService.changeCheckedTest(testId, function (result) {
                LoadTests();
                $scope.$parent.IndexCtrl.setCountTests($scope.$parent.IndexCtrl.countTests - 1);
            });            
        }

        function getClassForTr(status) {
            var resultClass = '';
            switch (status) {
                case 1:
                    resultClass = 'success';
                    break;
                case 2:
                    resultClass = 'danger'
                    break;
            }
            return resultClass;
        }
     
    }
})();