(function () {
    'use strict';

    angular.module('Administration')
        .config(function (growlProvider) {
            growlProvider.globalPosition('bottom-right');
            growlProvider.onlyUniqueMessages(false);
            growlProvider.globalTimeToLive({ success: 1000, error: 10000, warning: 2000, info: 4000 });
        });
})();