(function () {
    'use strict';

    angular
        .module('hunter-app')
        .filter('VacanciesFilter', VacanciesFilter);

    function filterVacanciesByPool(vacancies, poolFilters) {
        var filtered = [];

        if (vacancies == undefined) {
            return filtered;
        }

        vacancies.forEach(function (vacancy) {
            var id = vacancy.poolId;

                if (poolFilters[id]) {
                    filtered.push(vacancy);
                }
        });

        return filtered;
    }

    function filterVacanciesByStatus(vacancies, statusFilters) {
        var filtered = [];

        if (vacancies == undefined) {
            return filtered;
        }

        vacancies.forEach(function (vacancy) {
            var id = vacancy.status;

            if (statusFilters[id]) {
                filtered.push(vacancy);
            }
        });

        return filtered;
    }

    function filterVacanciesByAdder(vacancies, adderFilters) {
        var filtered = [];

        if (vacancies == undefined) {
            return filtered;
        }

        vacancies.forEach(function (vacancy) {
            adderFilters.forEach(function (adder) {
                if (adder.name == vacancy.addedByName && adder.isChecked) {
                    filtered.push(vacancy);
                }
            });
        });

        return filtered;
    }

    function filterVacanciesBySearch(vacancies, poolFilters) {
        var filtered = [];

        if (vacancies == undefined) {
            return filtered;
        }

        vacancies.forEach(function (vacancy) {
            var name = vacancy.name;

            if (name.toLowerCase().includes(poolFilters.toLowerCase())) {
                filtered.push(vacancy);
            }
        });

        return filtered;
    }

    function VacanciesFilter() {

        return function (vacancies, options) {

            if (!checkOptions(options.poolFilters) &&
                !checkOptions(options.statusFilters) &&
                !checkOptions(options.adderFilters) &&
                !checkOptions(options.searchText)) {
                return vacancies;
            }

            var res = [];

            if (checkOptions(options.poolFilters)) {
                res = filterVacanciesByPool(vacancies, options.poolFilters);
            } else {
                res = vacancies;
            }

            if (checkOptions(options.adderFilters)) {
                res = filterVacanciesByAdder(res, options.adderFilters);
            }
            if (checkOptions(options.statusFilters)) {
                res = filterVacanciesByStatus(res, options.statusFilters);
            }
            if (checkOptions(options.searchText)) {
                res = filterVacanciesBySearch(res, options.searchText);
            }
            return res;
        }
    }

    function checkOptions(options) {
        var anyChecked = false;
        if (typeof options === 'string' && options) {
            return true;
        }
        if (options instanceof Array) {
            for (var i in options) {
                if (options[i].isChecked) {
                    return true;
                }
            }
        } else {
            for (var key in options) {
                if (options[key]) {
                    return true;
                }
            }
        }

        return anyChecked;
    }

})();