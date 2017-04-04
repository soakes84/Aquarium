(function () {
    'use strict';

    var application = angular.module('Application');

    application.controller('AccountController', AccountController);

    AccountController.$inject = ['$http', '$window'];

    function AccountController($http, $window) {
        var vm = this;          

        vm.Register = function (model) {
            var promise = $http.post('/api/accounts/register', model);
            promise.then(function (result) {
                $window.location.href='login';
            });
        };

        vm.Login = function (model) {
            var promise = $http.post('/api/accounts/login', model);
            promise.then(function (result) {
                $window.location.href='/home/tanks';
            });
        };
        
    }

})();