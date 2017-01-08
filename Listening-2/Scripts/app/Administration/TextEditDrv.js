(function () {
    'use strict';

    angular.module('Administration')
        .controller('TextEditCtrl', function ($scope, $stateParams, $state, Upload, TextSvcRest,
            FileSvcRest, GenerateIdSvc, focus, growl) {
            var self = this;

            self.init = function () {
                $scope.textDto = {};
                $scope.successMessage = '';
                $scope.errorsFromServer = [];
                $scope.errorText = '';
                $scope.errorTitle = '';

                $scope.textDto.textId = $stateParams.textId || GenerateIdSvc.generateId();
                $scope.isNewElement = !$stateParams.textId;

                focus('focusMe');

                self.getText = function (id) {
                    $scope.textDto = TextSvcRest.get({ id: id });
                };

                self.successFileUpload = function () {
                    growl.success('File uploaded successfully!');
                };

                self.successFileRemoved = function () {
                    growl.success('File removed successfully!');
                };

                self.successTextInsert = function () {
                    growl.success('Text inserted successfully!');
                };

                self.successTextRemove = function () {
                    growl.success('Text deleted successfully!');
                };

                self.successTextUpdated = function () {
                    growl.success('Text updated successfully!');
                };

                self.catchEror = function (resp) {
                    growl.error(resp.data.message.replace('\n', '<br />'));
                };

                if (!$scope.isNewElement) {
                    self.getText($scope.textDto.textId);
                }
            };

            self.addListeners = function () {
                $scope.$watch('file.name', function (newValue, oleValue) {
                    $scope.textDto.audioName = newValue;
                });

                $scope.$watch('textDto.title', function (newValue, oleValue) {
                    $scope.checkTitle();
                });

                $scope.$watch('textDto.text', function (newValue, oleValue) {
                    $scope.checkText();
                });
            };

            $scope.checkText = function () {
                if (!$scope.textDto.text)
                    $scope.errorText = 'Text shouldn`t be empty';
                else if ($scope.textDto.text.length > 4000)
                    $scope.errorText = 'System does not support texts more than 4000 symbols';
                else
                    $scope.errorText = '';
            };

            $scope.checkTitle = function () {
                $scope.errorTitle = !$scope.textDto.title
                    ? 'Title shouldn`t be empty' : '';
            };

            $scope.isSaveDisabled = function () {
                return !!$scope.errorTitle || !!$scope.errorText;
            };

            $scope.save = function () {
                $scope.checkTitle();
                $scope.checkText();
                if ($scope.isSaveDisabled())
                    return;

                $scope.errorsFromServer.length = 0;

                var dto = {
                    textId: $scope.textDto.textId,
                    title: $scope.textDto.title,
                    subTitle: $scope.textDto.subTitle,
                    text: $scope.textDto.text,
                    audioName: $scope.textDto.audioName
                };

                if ($scope.file) {
                    FileSvcRest.upload($scope.textDto.textId, $scope.file).then(self.successFileUpload, self.catchEror);
                    //    function () {
                    //    growl.success('File uploaded successfully!');
                    //}, function (resp) {
                    //    $scope.errorsFromServer.push(resp.data.message);
                    //    growl.error(resp.data.message);
                    //});
                }

                if (!!$scope.errorsFromServer && !$scope.errorsFromServer.length) {
                    if ($scope.isNewElement) {
                        TextSvcRest.insert(dto, self.successTextInsert, self.catchEror);
                        $scope.isNewElement = false;
                    }
                    else {
                        TextSvcRest.update({ id: $scope.textDto.textId },
                            dto, self.successTextUpdated, self.catchEror);
                    }
                }

            };

            $scope.remove = function () {
                TextSvcRest.remove({ id: $scope.textDto.textId }, self.successTextRemove, self.catchEror);
                FileSvcRest.remove($scope.textDto.audioName).then(self.successFileRemoved, self.catchEror);
                $state.go('administration');
            };

            self.init();
            self.addListeners();

        })
        .directive('textEdit', function () {
            return {
                restrict: 'E',
                controller: 'TextEditCtrl',
                templateUrl: 'js/angular/app/Administration/templates/textEdit.html'
            };
        });
})();