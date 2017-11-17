(function () {
    'use strict';
    var configFn = function ($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise('/TeacherApp/Login');
        $stateProvider
             .state('/TeacherApp', {
                 url: '/Login',
                 templateUrl: '/Areas/TeacherApp/templates/views/Home/teacher_profile.html',
                 controller: 'Teacher_Login'
             })
            .state('Teacher_Dashboard', {
                url: '/TeacherApp/Dashboard',
                templateUrl: '/Areas/TeacherApp/templates/views/Home/teacher_profile.html',
                controller: 'teacherApp_Profile'
            })
        .state('Teacher_Profile', {
            url: '/TeacherApp/Profile',
            templateUrl: '/Areas/TeacherApp/templates/views/Profile/teacher_profile.html',
            controller: 'teacherApp_Profile'
        });
    };

    var moduleDependencies =
        [
            'datatables',
            'uiSwitch',             // https://github.com/xpepermint/angular-ui-switch
            'Silverzone_app.Common'
        ];

    angular.module("TeacherApp", moduleDependencies)
    .config(['$stateProvider', '$urlRouteProvider', configFn]);
})();