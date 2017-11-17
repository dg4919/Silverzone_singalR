(function (app) {

    var fn = function ($http, $q, globalConfig) {

        var apiUrl = globalConfig.apiEndPoint + globalConfig.version.Admin,
             fac = {};

        fac.upload_quizImage = function (files) {
            var form_Data = new FormData();

            for (var i = 0; i < files.length; i++) {
                form_Data.append("file", files[i]);
            }

            var defer = $q.defer();

            $http({
                method: 'POST',
                url: apiUrl + '/Quiz/upload_quizImage',
                data: form_Data,
                headers: {
                    'Content-Type': undefined
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('file upload failed');
                });
            return defer.promise;
        }


        fac.createQuiz = function (_model) {
            debugger;

            // we r going to return a httprespone + data of list -> (here angular will automatically serialise of list into the Json format)
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Quiz/create_quiz',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.updateQuiz = function (_model) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Quiz/update_quiz',
                method: 'POST',
                data: _model
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_AllQuiz = function () {
            var defer = $q.defer();

            $http({
                url: apiUrl + '/Quiz/getAll_quiz',
                method: 'GET'
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        fac.get_QuizbyId = function (quizId) {
            debugger;

            var defer = $q.defer();

            $http({
                url: apiUrl + '/Quiz/get_quizbyId',
                method: 'GET',
                params: {
                    quizId: quizId
                }
            })
                .success(function (d) {
                    defer.resolve(d);
                })
                .error(function (e) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }


        return fac;
    }

    angular.module('Silverzone_admin_app')
        .factory('admin_quiz_Service', ['$http', '$q', 'globalConfig', fn]);

})();

