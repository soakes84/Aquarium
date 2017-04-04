(function () {
    var application = angular.module('Application');

    application.controller('TankController', TankController);

    TankController.$inject = ['$http'];

    function TankController($http) {
        var vm = this;          //vm stands for view model, standard practice

        vm.Tanks = [];

        activate();

        function activate() {
            var promise = $http.get('/api/tanks');
            promise.then(function (result) {
                vm.Tanks = result.data;
            });
        };

        vm.Add = function (tank) {
            var copy = angular.copy(tank);
            tank.name = '';

            var promise = $http.post('/api/tanks' , copy);
            promise.then(function (result) {
                //success
                vm.Tanks.push(result.data);
            }, function (result) {
                //failure
            });

        };

        vm.Remove = function (tank) {
            var url = '/api/tanks/{id}'.replace('{id}', tank.id);
            var promise = $http.delete(url);
            promise.then(function (result) {
                var index = vm.Tanks.indexOf(tank);
                vm.Tanks.splice(index, 1);
            });

        };

        vm.Clear = function (tank) {
           
            var promise = $http.clear('/api/tanks');
            promise.then(function (result) {
                var index = vm.Tanks.indexOf(fish);
                vm.Tanks.splice(index, 1);
            });
        };
    }

})();