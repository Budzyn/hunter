(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('CandidateAddEditService', CandidateAddEditService);

    CandidateAddEditService.$inject = [];

    function CandidateAddEditService() {
        var service = {
            validateData: validateData
        };

        function validateData(data, errorObject) {
            var noErrors = true;
            if (!data) {
                return false;
            }

//            if (!data.someField) {
//                errorObject.someField = true;
//                noErrors = false;
//            } else {
//                errorObject.someField = false;
//            }
            if (!data.FirstName) {
                errorObject.message += ' First name; ';
                noErrors = false;
            }
            if (!data.LastName) {
                errorObject.message += ' Last name; ';
                noErrors = false;
            }

            if (!data.DateOfBirth) {            
                    errorObject.message += ' Date of birth; ';
                    noErrors = false;
            }
            return noErrors;
        }

        return service;
    }
})();