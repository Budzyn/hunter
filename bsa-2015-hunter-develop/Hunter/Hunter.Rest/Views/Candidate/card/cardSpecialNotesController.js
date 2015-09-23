(function () {
    'use strict';

    angular
        .module('hunter-app')
        .controller('CardSpecialNotesController', CardSpecialNotesController);

    CardSpecialNotesController.$inject = [
        'SpecialNoteHttpService',
        'VacancyHttpService',
        '$routeParams',
        '$scope'
    ];

    function CardSpecialNotesController(specialNoteHttpService, VacancyHttpService, $routeParams, $scope) {
        var vm = this;
        vm.templateName = 'Special Notes';

        vm.saveOldNote = saveOldNote;
        vm.saveNewNote = saveNewSpecialNote;
        vm.loadCardNotes = loadCardNotes;
        vm.loadAllNotes = loadAllNotes;
        vm.loadMyNotes = loadMyNotes;

        vm.vacancy ={};
        vm.notes = [];
        vm.newNoteText = '';
        vm.specialNote ={};
        vm.newNoteText ='';

        (function () {

            if ($scope.$parent.generalCardCtrl.isLLM) {
                loadCardNotes();


        VacancyHttpService.getLongList($routeParams.vid).then(function(result) {
            vm.vacancy = result;
        });
            } else {
                loadAllNotes();
            }
        })();
        function saveNewSpecialNote() {
            var note = {
                text: vm.newNoteText,
                vacancyId: $routeParams.vid || null,
                candidateId: $routeParams.cid
            };
            specialNoteHttpService.addSpecialNote(note)
            .then(function (data) {
                    note.id = data.id;
                    note.lastEdited = data.update;
                note.userLogin = data.userName;
                    vm.notes.unshift(note);
                    vm.newNoteText = '';
                });
        }

        function saveOldNote(note) {
            specialNoteHttpService.updateSpecialNote(note, note.id)
                .then(function (data) {
                    note.id = data.id;
                    note.lastEdited = data.update;
                    note.userLogin = data.userName;
                });
        }

        function loadCardNotes() {
            specialNoteHttpService.getCardSpecialNote($routeParams.vid, $routeParams.cid)
                .then(function (result) {
                    console.log(result.data);
                    vm.notes = result.data;
                });
        };

        function loadAllNotes() {
            specialNoteHttpService.getCandidateSpecialNote($routeParams.cid).then(function (result) {
                console.log(result.data);
                vm.notes = result.data;
            });
        }

        function loadMyNotes() {
            specialNoteHttpService.getUserSpecialNote($routeParams.cid).then(function (result) {
                console.log(result.data);
                vm.notes = result.data;
            });
        }


        function toggleReadOnly(note) {
            note.noteConfig.readOnly = note.text != '' ? !note.noteConfig.readOnly : note.noteConfig.readOnly;

            if (note.noteConfig.readOnly) {
                note.noteConfig.buttonName = 'Edit';
                SaveOldSpecialNote({
                    'id': note.id,
                    'userAlias': note.login,
                    'text': note.text,
                    'lastEdited': note.lastEdited,
                    'cardId': note.cardId
                });

            } else {
                note.noteConfig.buttonName = 'Save';
            }
        }
    }
})();