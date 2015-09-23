(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('CandidateHttpService', CandidateHttpService);

    CandidateHttpService.$inject = [
        'HttpHandler',
        '$q',
        '$odataresource',
        '$odata'
    ];

    function CandidateHttpService(httpHandler, $q, $odataresource, $odata) {
        var service = {
            updateCandidate: updateCandidate,
            getCandidate: getCandidate,
            getCandidateList: getCandidateList,
            addCandidate: addCandidate,
            //            getLongList: getLongList,
            getLongListDetails: getLongListDetails,
            parseLinkedIn: parseLinkedIn,
            getAddedByList: getAddedByList,
            setShortListFlag: setShortListFlag,
            updateCandidateResolution: updateCandidateResolution,
            addCandidatePool: addCandidatePool,
            removeCandidatePool: removeCandidatePool,
            changeCandidatePool: changeCandidatePool,
            getOdataCandidateList: getOdataCandidateList
        };

        function updateCandidate(body, successCallback, id) {
            httpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/candidates/' + id,
                body: body,
                successCallback: successCallback
            });
        }

        function addCandidate(body, successCallback) {
            httpHandler.sendRequest({
                verb: 'POST',
                url: 'api/candidates/',
                body: body,
                successCallback: successCallback
            });
        }

        function getCandidate(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/candidates/' + id,
                //                body: body,
                successCallback: function (response) {
                    deferred.resolve(response);
                },
                errorCallback: function (status) {
                    console.log("getting candidate error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getCandidateList() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/candidates/',
                //                body: body,
                successCallback: function (response) {
                    deferred.resolve(response);
                },
                errorCallback: function (status) {
                    console.log("getting candidates error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        //function getLongList(id) {
        //    var deferred = $q.defer();
        //    httpHandler.sendRequest({
        //        url: '/api/candidates/longlist/' + id,
        //        verb: 'GET',
        //        successCallback: function (result) {
        //            deferred.resolve(result.data);
        //        },
        //        errorCallback: function (status) {
        //            console.log("Get candidates long list error");
        //            console.log(status);
        //        }
        //    });
        //    return deferred.promise;
        //}

        function getLongListDetails(vid, cid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/candidates/candidatelonglist/' + vid + '/' + cid,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get candidates long list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function parseLinkedIn(url) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/resume/parse?url=' + url,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get candidates long list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getAddedByList() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/candidates/addedby',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get candidates long list Added by filter data error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function setShortListFlag(cid, isShort) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/candidates/' + cid + '/' + isShort,
                verb: 'PUT',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log(status);
                    deferred.reject(status);
                }
            });
            return deferred.promise;
        }

        function updateCandidateResolution(cid, resolution) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/candidates/' + cid + '/resolution/' + resolution,
                verb: 'PUT',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log(status);
                    deferred.reject(status);
                }
            });
            return deferred.promise;
        }

        function addCandidatePool(candidateId, poolId) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                'url': 'api/candidates/' + candidateId + '/addpool/' + poolId,
                'verb': 'PUT',
                'successCallback': function(response) {
                    console.log('Pool added successful');
                    deferred.resolve(response.data);
                }
            });

            return deferred.promise;
        }

        function removeCandidatePool(candidateId, poolId) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                'url': 'api/candidates/' + candidateId + '/removepool/' + poolId,
                'verb': 'DELETE',
                'successCallback': function(response) {
                    console.log('Pool removed successful');
                    deferred.resolve(response.data);
                }
            });

            return deferred.promise;
        }

        function changeCandidatePool(oldId, newId, candidateId) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                'url': 'api/candidates/' + candidateId + '/pools/update/' + oldId + '/' + newId,
                'verb': 'PUT',
                'successCallback': function(response) {
                    console.log('Pools changed');
                    deferred.resolve(response.data);
                }
            });

            return deferred.promise;
        }

        function getOdataCandidateList(filter) {
            var deferred = $q.defer();

            var predicate;
            //
            var filt = [];

            if (filter.pools != undefined && filter.pools.length > 0) {
                var poolPred = [];
                angular.forEach(filter.pools, function (value, key) {
                    poolPred.push(new $odata.Predicate(new $odata.Property('PoolNames/any(p: p eq \'' + value + '\' )'), true));
                });

                poolPred = $odata.Predicate.or(poolPred);
                filt.push(poolPred);
            }

            if (filter.inviters != undefined && filter.inviters.length > 0) {
                var invPred = [];
                angular.forEach(filter.inviters, function (value, key) {
                    invPred.push(new $odata.Predicate('AddedBy', value));
                });

                invPred = $odata.Predicate.or(invPred);
                filt.push(invPred);
            }

            if (filter.statuses != undefined && filter.statuses.length > 0) {
                var stPred = [];
                angular.forEach(filter.statuses, function (value, key) {
                    stPred.push(new $odata.Predicate('Resolution',parseInt(value)));
                });

                stPred = $odata.Predicate.or(stPred);
                filt.push(stPred);
            }

            if (filter.shortListed) {
                filt.push(new $odata.Predicate('ShortListed', filter.shortListed));
            }

            if (filter.search != undefined && filter.search.length > 0) {
                var pred = $odata.Predicate.or([
                    new $odata.Func('substringof', new $odata.Property('tolower(\'' + filter.search + '\')'), new $odata.Property('tolower(FirstName)')),
                    new $odata.Func('substringof', new $odata.Property('tolower(\'' + filter.search + '\')'), new $odata.Property('tolower(LastName)'))
                ]);

                filt.push(pred);
            }

            if (filt.length > 0) {
                predicate = $odata.Predicate.and(filt);
            } else {
                predicate = undefined;
            }

            var skip = (filter.currentPage - 1) * filter.pageSize;
            //
            var Candidates = $odataresource('/api/Candidates/odata');

            var cands = Candidates.odata()
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