(function () {
    var application = angular.module('Application');

    application.controller('AquariumController', AquariumController);

    AquariumController.$inject = ['$http', '$interval'];

    function AquariumController($http, $interval) {
        var vm = this;          //vm stands for view model, standard practice

        vm.Fishes = [];
        vm.tank = '';
        vm.fish = '';


        vm.GetInfo = function (tankId) {
            var promise = $http.get('/api/tanks/' + tankId);
            promise.then(function (result) {
                console.log(result);
                vm.tank = (result.data);
                vm.GetFishes();
            }, function (result) {
                console.log(result);
            });
        };

        vm.GetFishes = function () {
            var promise = $http.get('/api/tanks/' + vm.tank.id + '/fishes');
            promise.then(function (result) {
                vm.Fishes = result.data;
            });
        };

        vm.GetFishInfo = function (tankId, fishId) {
            var promise = $http.get('/api/tanks/' + tankId + '/fishes/' + fishId);
            promise.then(function (result) {
                console.log(result);
                vm.fish = (result.data);
            }, function (result) {
                console.log(result);
            });
        };

        vm.Add = function (fish) {
            var copy = angular.copy(fish);
            fish.name = '';
            fish.type = '';
            fish.description = '';

            var promise = $http.post('/api/tanks/' + vm.tank + '/fishes', copy);
            promise.then(function (result) {
                //success
                vm.Fishes.push(result.data);
            }, function (result) {
                //failure
            });

        };

        vm.Remove = function (fish) {
            var url = '/api/fishes/{id}'.replace('{id}', fish.id);
            var promise = $http.delete(url);
            promise.then(function (result) {
                var index = vm.Fishes.indexOf(fish);
                vm.Fishes.splice(index, 1);
            });

        };

        vm.Clear = function (fish) {
            var promise = $http.clear('/api/fishes');
            promise.then(function (result) {
                var index = vm.Fishes.indexOf(fish);
                vm.Fishes.splice(index, 1);
            });
        };

        vm.AddShark = function () {
            vm.Fishes.push({ name: 'Steve the Shark', type: 'Great White', description: 'Eats all Fish' });
            $interval(function () {
                var fish = vm.Fishes[0];
                vm.Remove(fish);
            }, 2000, vm.Fishes.length - 1);
            var promise = $http.post('/api/tanks' + vm.tank.id);
            promise.then(function (result) {
                vm.Fishes.push(result.data);

            })

        }
    }

})();