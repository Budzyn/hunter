(function () {
    'use strict';

    angular
        .module('hunter-app')
        .factory('UserProfileService', userProfileService);

    userProfileService.$inject = [
        'HttpHandler'
    ];

    function userProfileService(httpHandler) {
        var service = {
            updateUserProfile: updateUserProfile,
            getUserProfile: getUserProfile,
            getUserProfileList: getUserProfileList,
            deleteUserProfile: deleteUserProfile
        };

        function updateUserProfile(body, onSuccess) {
            httpHandler.sendRequest({
                verb: 'POST', //'PUT',
                url: '/api/userprofile/',
                body: body,
                successMessageToUser: 'User profile updated.',
                successCallback: onSuccess,
                errorCallback: function (error) {
                    alertify.error("Save profile error: " + error.data);
                }
            });
        };

        function deleteUserProfile(id, successCallback) {
            httpHandler.sendRequest({
                verb: 'DELETE',
                url: 'api/userprofile/' + id,
                successCallback: successCallback,
                successMessageToUser: "User with id " + id + " deleted.",
                errorCallback: function (error) {
                    console.log("Delete vacancy error. Status " + error);
                }
            });
        };

        function getUserProfile(id, onSuccess) {
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/userprofile/' + id,
                successCallback: onSuccess,
                errorMessageToDev: 'GET USERPROFILE INFO ERROR: ',
                errorMessageToUser: 'Failed'
            });
        };

        function getUserProfileList(page, successCallback) {
            httpHandler.sendRequest({
                verb: 'GET',
                url: '/api/userprofile/',
                params: {'page': page},
                successCallback: successCallback,
                errorMessageToDev: 'GET USERPROFILE INFO ERROR: ',
                errorMessageToUser: 'Failed'
            });
        };

        return service;
    }
})();