(function () {
    'use strict';

    angular.module('Account')
        .factory('AccountDataSvc', function ($http, $resource) {
            var endpoint = 'Account/';

            return {
                login: function (login) {
                    return $http({ method: 'POST', url: endpoint + 'Login', data: login });
                },
                logout: function () {
                    return $http({ method: 'POST', url: endpoint + 'LogOff' });
                },
                registry: function (user) {
                    console.log(user);
                    return $http({ method: 'POST', url: endpoint + 'Register', data: user });
                },
                confirmEmail: function (userId, code) {
                    return $http({ method: 'GET', url: endpoint + 'ConfirmEmail', params: { userId: userId, code: code } });
                }
            };
        });
})();