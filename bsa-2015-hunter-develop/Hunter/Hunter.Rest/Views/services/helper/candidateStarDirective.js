(function () {
    'use strict';
    angular
        .module('hunter-app')
        .directive('shorListStar', shorListStar);

    shorListStar.$inject = ['CandidateHttpService', '$rootScope'];

    function shorListStar(CandidateHttpService, $rootScope) {
        return {
            template: '<i class="fa " ' +
                'ng-class="isShort ? \'fa-star\' : \'fa-star-o\' "' +
                'ng-click="update()" ' +
                'style="position: absolute;font-size:{{size}};top: 2px;left: 2px;color: gold;"' +
                '></i>',
            restrict: 'E',
            scope: {
                isShort: "=isShort",
                candidateId: "=candidateId",
                size: "=size"
            },
            link: function (scope, element, attrs, ctrl, transclude) {


                scope.update = function () {
                    CandidateHttpService.setShortListFlag(scope.candidateId, !scope.isShort).then(function (result) {
                        scope.isShort = result;
                    });
                    scope.isShort = !scope.isShort;
                    $rootScope.$broadcast('starChangeEvent', { id: scope.candidateId, isShort: scope.isShort });
                }

                $rootScope.$on('starChangeEvent', function(event, e) {
                    if (e.id == scope.candidateId) {
                        scope.isShort = e.isShort;
                    }
                });

            }
        }
    }

})();