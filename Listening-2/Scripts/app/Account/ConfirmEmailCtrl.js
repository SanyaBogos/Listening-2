(function () {
    'use strict';

    angular.module('Account')
        .controller('ConfirmEmailCtrl', function ($scope, $state, $location, AccountDataSvc) {
            var self = this;

            $scope.error = '';

            self.checkConfirmation = function () {
                AccountDataSvc.confirmEmail($state.params.userId, $location.search().code)
                    .then(function (data) { })
                    .catch(function (error) {
                        $scope.error = error.data.message;
                    });
            };

            self.checkConfirmation();
        });
})();