(function () {

    'use strict';

    angular
        .module('hunter-app')
        .factory("UploadResumeService", UploadResumeService);

    UploadResumeService.$inject = [
        'Upload',
        'EnumConstants'
    ];

    function UploadResumeService(upload, enumConstants) {
        var service = {
            onFileSelect: onFileSelect,
            uploadResume: uploadResume,
            //uploadWithLatenc: uploadWithLatenc
        };

        var _files;

        function onFileSelect($files) {
            _files = $files;
        }

        function uploadResume(candidate) {

            angular.forEach(_files, function (file) {
                if (file) {
                    upload.upload({
                        url: "api/fileupload",
                        method: "POST",
                        data: {
                            fileType: enumConstants.fileType.Resume,
                            fileName: file.name,
                            candidateid: candidate.id
                        },
                        file: file
                    })
                    .success(function (data) { console.log(data) });
                }
            });
        }

        //function uploadWithLatenc(candidate) {
        //    setTimeout(function() {
        //        uploadResume(candidate);
        //    }, 1000);
        //}


        return service;
    }
})();