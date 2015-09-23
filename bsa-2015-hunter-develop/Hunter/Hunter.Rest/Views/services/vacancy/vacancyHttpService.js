(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('VacancyHttpService', VacancyHttpService);

    VacancyHttpService.$inject = [
        '$q',
        'HttpHandler'
    ];

    function VacancyHttpService($q, httpHandler) {
        var service = {
            getFilteredVacancies : getFilteredVacancies,
            getVacancy: getVacancy,
            addVacancy: addVacancy,
            validateVacancy: validateVacancy,
            updateVacancy: updateVacancy,
            deleteVacancy: deleteVacancy,
            getLongList: getLongList,
            getLongListAddedBy: getLongListAddedBy,
            getFilterInfo: getFilterInfo,
            getAddedByList: getAddedByList,
            getVacancyByState: getVacancyByState
        }

        function getFilteredVacancies(filter) {
            var requestUrl = '/api/vacancy/?' +
                'page=' + filter.page + '&' +
                'pageSize=' + filter.pageSize + '&' +
                'sortColumn=' + filter.sortColumn + '&' +
                'reverceSort=' + filter.reverceSort + '&' +
                'filter="' + filter.filter + '"&' +
                'pool="' + filter.pool.toString() + '"&' +
                'status="' + filter.status.toString() + '"&' +
                'addedBy="' + filter.addedBy.toString() + '"';
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: requestUrl,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get vacancies error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getVacancy(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/vacancy/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get vacancy error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function addVacancy(data) {
            httpHandler.sendRequest({
                verb: 'POST',
                url: '/api/vacancy',
                body: data,
                successMessageToUser: 'Vacancy added',
                errorMessageToUser: 'Vacancy not added',
                errorCallback: function (status) {
                    console.log("Add vacancy error");
                    console.log(status);
                }
            });
        }

        function updateVacancy(data, id) {
            console.log(data);
            httpHandler.sendRequest({
                verb: 'PUT',
                url: '/api/vacancy/' + id,
                body: data,
                successMessageToUser: 'Vacancy updated',
                errorMessageToUser: 'Vacancy not updated',
                errorCallback: function (status) {
                    console.log("Update vacancy error");
                    console.log(status);
                }
            });
        }

        function deleteVacancy(id) {
            httpHandler.sendRequest({
                verb: 'DELETE',
                url: '/api/vacancy/' + id,
                successMessageToUser: 'Vacancy deleted',
                errorMessageToUser: 'Vacancy not deleted',
                errorCallback: function (status) {
                    console.log("Delete vacancy error");
                    console.log(status);
                }
            });
        }

        function validateVacancy(data) {
            //var noError = true;
            if (!data) {
                return false;
            }
            //if (!data.someField) {
            //    errorObject.someField = true;
            //    noErrors = false;
            //} else {
            //    errorObject.someField = false;
            //}
            //return noErrors;
            return true;
        }

        function getLongList(id)
        {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/vacancy/longlist/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get vacancy long list error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getLongListAddedBy(vid) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/vacancy/longlist/' + vid + '/addedby',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get vacancy long list Added by filter data error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getFilterInfo(roleName) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/vacancy/filterInfo/' + roleName,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get filter data error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getAddedByList() {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/vacancy/addedby',
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get vacancies Added by filter data error");
                    console.log(status);
                }
            });
            return deferred.promise;
        }

        function getVacancyByState(id) {
            var deferred = $q.defer();
            httpHandler.sendRequest({
                url: '/api/vacancy/state/' + id,
                verb: 'GET',
                successCallback: function (result) {
                    deferred.resolve(result.data);
                },
                errorCallback: function (status) {
                    console.log("Get vacancies Added by filter data error");
                    console.log(status);
                }
            });

            return deferred.promise;
        }

        return service;
    }
})();