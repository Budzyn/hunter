(function () {
    'use strict';

    angular.module('hunter-app')
        .factory('LonglistHttpService', LonglistHttpService);

    LonglistHttpService.$inject = [
    'HttpHandler',
    '$q',
	'$location',
    '$odataresource',
    '$odata',
    '$routeParams'
    ];
    function LonglistHttpService(httpHandler, $q, $location, $odataresource, $odata, $routeParams) {
        var service = {
            addCards: addCards,
            removeCard: removeCard,
            getAppResults: getAppResults,
            getCardInfo: getCardInfo,
            getOdataLongList: getOdataLongList
        }

        function addCards(body, newLocation) {
            httpHandler.sendRequest({
                verb: 'POST',
                url: '/api/card',
                body: JSON.stringify(body),
                successCallback: function (result) {
                    $location.url(newLocation);
                },
                successMessageToUser: 'Cards were added',
                errorMessageToUser: 'Cards were not added',
                errorCallback: function (status) {
                    console.log("Add cards error");
                    console.log(status);
                }
            });
        }

        function removeCard(vid, cid) {
            httpHandler.sendRequest({
                url: 'api/card/isUsed/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    if (result.data) {
                        var clickResult = confirm('Would you like to remove card from Long List and delete all its feedbacks and notes?');
                        if (clickResult === true) {
                            (function () {
                                httpHandler.sendRequest({
                                    url: 'api/card/deleteallinfo/' + vid + '/' + cid,
                                    verb: "DELETE",
                                    successCallback: function (result) {
                                        //console.log(result);
                                    },
                                    successMessageToUser: 'Card and all feedbacks and notes were removed',
                                    errorMessageToUser: 'Card was not removed. Card is used',
                                    errorCallback: function (result) { console.log(result); }
                                });
                            })();
                            //alert('user click true');
                        } else {
                            //alert('user click cancel');
                        }
                    } else {
                        //alert('is Card used result (else): ' + result.data);
                        (function () {
                            httpHandler.sendRequest({
                                url: 'api/card/' + vid + '/' + cid,
                                verb: "DELETE",
                                successCallback: function (result) {
                                    //console.log(result);
                                },
                                successMessageToUser: 'Card was removed',
                                errorMessageToUser: 'Card was not removed. Card is used',
                                errorCallback: function (result) { console.log(result); }
                            });
                        })();
                    }
                    //console.log(result);
                    //$location.url("/vacancy/1/longlist");
                },
                //successMessageToUser: 'Card was removed',
                //errorMessageToUser: 'Card was not removed. Card is used',
                errorCallback: function (result) { console.log(result); }
            });




            /**
             
             function getCardInfo(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/card/info/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get application results error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }
             * /
             */




        }

        function getAppResults(cid) {

            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/card/appResults/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get application results error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getCardInfo(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/card/info/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get application results error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getOdataLongList(filter) {
            var deferred = $q.defer();
            var predicate;
            var filt = [];

            if (filter.search.length > 0) {
                var pred = $odata.Predicate.or([
                    new $odata.Func('substringof', new $odata.Property('tolower(\'' + filter.search + '\')'), new $odata.Property('tolower(FirstName)')),
                    new $odata.Func('substringof', new $odata.Property('tolower(\'' + filter.search + '\')'), new $odata.Property('tolower(LastName)')),
                    new $odata.Func('substringof', new $odata.Property('tolower(\'' + filter.search + '\')'), new $odata.Property('tolower(Company)')),
                    new $odata.Func('substringof', new $odata.Property('tolower(\'' + filter.search + '\')'), new $odata.Property('tolower(Location)'))
                ]);

                filt.push(pred);
            }

            if (filter.hr.length > 0) {
                var hrPred = [];
                angular.forEach(filter.hr, function (value, key) {
                    hrPred.push(new $odata.Predicate('AddedBy', value));
                });

                hrPred = $odata.Predicate.or(hrPred);
                filt.push(hrPred);
            }

            if (filter.stages.length > 0) {
                var stPred = [];
                angular.forEach(filter.stages, function (value, key) {
                    stPred.push(new $odata.Predicate('Stage', value));
                });

                stPred = $odata.Predicate.or(stPred);
                filt.push(stPred);
            }

            if (filter.shortlisted) {
                filt.push(new $odata.Predicate('Shortlisted', filter.shortlisted));
            }

            if (filt.length > 0) {
                predicate = $odata.Predicate.and(filt);
            } else {
                predicate = undefined;
            }

            var skip = (filter.currentPage - 1) * filter.pageSize;

            var CandidatesForLongList = $odataresource('/api/candidates/longlist/' + $routeParams.id + '/odata');

            var cands = CandidatesForLongList.odata()
                                           .withInlineCount()
                                           .take(filter.pageSize)
                                           .skip(skip)
                                           .filter(predicate)
                                           .orderBy(filter.order.split('_')[0], filter.order.split('_')[1])
                                           .query(function () {
                                               deferred.resolve(cands);
                                           });

            return deferred.promise;
        }

        return service;
    }
})();