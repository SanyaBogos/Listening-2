(function () {
    'use strict';

    angular.module('Account')
        .controller('RegisterCtrl', function ($scope, $state, AccountDataSvc, CheckDataSvc, growl) {
            var self = this;

            self.catchError = function (resp) {
                growl.error(resp.data.message.replace('\n', '<br />'));
            };

            self.setUserContext = function () {
                $scope.userContext.userName = $scope.user.email;
                $state.go('home');
            };

            $scope.user = {
                email: '',
                password: '',
                confirmPassword: ''
            };

            $scope.emailError = '';
            $scope.passwordError = '';
            $scope.confirmPasswordError = '';

            $scope.$watch('user.email', function () {
                $scope.emailCheck();
            });

            $scope.$watch('user.password', function () {
                $scope.passwordCheck();
            });

            $scope.$watch('user.confirmPassword', function () {
                $scope.confirmPasswordCheck();
            });

            $scope.emailCheck = function () {
                $scope.emailError = CheckDataSvc.emailCheck($scope.user.email);
            };

            $scope.passwordCheck = function () {
                $scope.passwordError = CheckDataSvc.passwordCheck($scope.user.password);
            };

            $scope.confirmPasswordCheck = function () {
                $scope.confirmPasswordError = CheckDataSvc.confirmPasswordCheck($scope.user.confirmPassword, $scope.user.password);
            };

            $scope.isRegisterDisabled = function () {
                return !!$scope.emailError || !!$scope.passwordError || !!$scope.confirmPasswordError;
            };

            $scope.registryClick = function () {
                AccountDataSvc.registry($scope.user).then(self.setUserContext, self.catchError);
            };
        });
})();