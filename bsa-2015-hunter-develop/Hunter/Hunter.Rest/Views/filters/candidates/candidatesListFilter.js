(function() {
    'use strict';

    Array.prototype.unique = function() {
        var arr = this.concat();

        if (arr.length == 0) {
            return [];
        }

        for (var i = 0; i < arr.length - 1; i++) {
            for (var j = i+1; j < arr.length; j++) {
                if (arr[i] === arr[j]) {
                    arr.splice(j--, 1);
                }
            }
        }

        return arr;
    }

    angular	
        .module('hunter-app')
        .filter('CandidatesFilter', CandidatesFilter);

    function filterCandidatesByPool(candidates, poolFilters) {
        var filtered = [];

        if (candidates == undefined) {
            return filtered;
        }

        candidates.forEach(function(candidate) {
            var wasPushed = false;

            candidate.poolNames.forEach(function(pool) {
                var name = pool.name;

                if (poolFilters[name] && !wasPushed) {
                    filtered.push(candidate);
                    wasPushed = true;
                }
            });
        });

        return filtered;
    }

    function filterCandidatesByStatus(candidates, statusFilters) {
        var filtered = [];

        if (candidates == undefined) {
            return filtered;
        }

        candidates.forEach(function(candidate) {
            var resolution = candidate.resolutionString;

            if (statusFilters[resolution]) {
                filtered.push(candidate);
            }
        });

        return filtered;
    }

    function filterCandidatesByInviter(candidates, inviterFilters) {
        var filtered = [];

        if (candidates == undefined) {
            return filtered;
        }

        candidates.forEach(function(candidate) {
            inviterFilters.forEach(function(inviter) {
                if (inviter.id == candidate.addedByProfileId && inviter.isChecked) {
                    filtered.push(candidate);
                }
            });
        });

        return filtered;
    }

    function filterCandidatesByName(candidates, nameFilter) {
        var filtered = [];
        var patern = RegExp('^' + nameFilter.toLowerCase());

        candidates.forEach(function(candidate) {
            if (patern.test(candidate.firstName.toLowerCase()) || patern.test(candidate.lastName.toLowerCase()) ||
                patern.test(candidate.firstName.toLowerCase() + ' ' + candidate.lastName.toLowerCase())) {
                filtered.push(candidate);
            }
        });

        return filtered;
    }

    function CandidatesFilter() {

        return function (candidates, options) {
            var result = candidates;

            if (checkOptions(options.poolFilters)) {
                result = filterCandidatesByPool(result, options.poolFilters);
            }

            if (checkOptions(options.statusFilters)) {
                result = filterCandidatesByStatus(result, options.statusFilters);
            }

            if (checkOptions(options.inviterFilters)) {
                result = filterCandidatesByInviter(result, options.inviterFilters);
            }

            if (options.nameFilter != '' && options.nameFilter != undefined) {
                result = filterCandidatesByName(result, options.nameFilter);
            }

            return result;
        }
    }

    function checkOptions(options) {
        var anyChecked = false;

        if (options instanceof Array) {
            for (var i in options) {
                if (options[i].isChecked) {
                    anyChecked = true;
                }
            }
        } else {
            for (var key in options) {
                if (options[key]) {
                    anyChecked = true;
                }
            }
        }

        return anyChecked;
    }

})();