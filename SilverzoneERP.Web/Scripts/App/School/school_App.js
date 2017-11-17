/// <reference path="../../Lib/angular-1.5.8.js" />

(function () {
    var configFn = function ($stateProvider) {

        $stateProvider.state("ERP_Login", {
            url: '/ERP/Login',
            templateUrl: 'Templates/Common/Login/Login.html',
            controller: 'accountController'
        })
        .state("ERP_Home", {
            url: '/ERP/Home',
            templateUrl: 'Templates/Common/Home/Index.html',
            controller: 'homeController'
        })
        .state("Registration", {
            url: '/ERP/User',
            templateUrl: 'Templates/School/User/Create.html',
            controller: 'registrationController'
        })

        .state('Role', {
            url: '/ERP/Role',
            templateUrl: 'Templates/Common/Role/Create.html',
            controller: 'roleController'
        })
        .state('UserRole', {
            url: '/ERP/UserRole',
            templateUrl: 'Templates/Common/Role/UserRole.html',
            controller: 'userRoleController'
        })
        .state('UserSummary', {
            url: '/ERP/UserSummary',
            templateUrl: 'Templates/School/User/Summary.html',
            controller: 'userSummaryController'
        })
        .state('UserPermission', {
            url: '/ERP/UserPermission',
            templateUrl: 'Templates/School/User/UserPermission.html',
            controller: 'userPermissionController'
        })
        .state('Location', {
            url: '/ERP/Location',
            templateUrl: 'Templates/School/Location/Index.html',
            controller: 'locationController'
        })
         .state('Designtion', {
             url: '/ERP/Designation',
             templateUrl: 'Templates/School/Designation/Create.html',
             controller: 'designationController'
         })
      .state('Title', {
          url: '/ERP/Title',
          templateUrl: 'Templates/School/Title/Create.html',
          controller: 'titleController'
      })
       .state('Event', {
           url: '/ERP/Event',
           templateUrl: 'Templates/School/Event/Create.html',
           controller: 'eventController'
       })
      .state('EventYear', {
          url: '/ERP/EventYear',
          templateUrl: 'Templates/School/Event/YearManage.html',
          controller: 'eventYearController'
      })
       .state('SchoolManagement', {
           url: '/ERP/SchoolManagement',
           templateUrl: 'Templates/School/SchoolManagement/Index.html',
           controller: 'schoolManagementController',
           params: {
               SchCode: null
           }
       })
       .state('Category', {
           url: '/ERP/Category',
           templateUrl: 'Templates/School/Category/Create.html',
           controller: 'categoryController'
       })
        .state('Class', {
            url: '/ERP/Class',
            templateUrl: 'Templates/School/Class/Create.html',
            controller: 'classController'
        })
       .state('SchoolGroup', {
           url: '/ERP/SchoolGroup',
           templateUrl: 'Templates/School/SchoolGroup/Create.html',
           controller: 'schoolGroupController'
       })
       .state('ExamDate', {
           url: '/ERP/ExamDate',
           templateUrl: 'Templates/School/ExamDate/Index.html',
           controller: 'examDateController'
       })
       .state('EventManagement', {
           url: '/ERP/EventManagement',
           templateUrl: 'Templates/School/EventManagement/Index.html',
           controller: 'eventManagementController',
           params: {
               SchCode: null
           }
       })
      .state('StudentEntry', {
          url: '/ERP/StudentEntry',
          templateUrl: 'Templates/School/StudentEntry/Index.html',
          controller: 'studentEntryController',
          params: {
              SchCode: null
          }
      })
        .state('Courier', {
            url: '/ERP/Courier',
            templateUrl: 'Templates/School/Courier/Create.html',
            controller: 'courierController'
        })
        .state('VerifyOrder', {
            url: '/ERP/VerifyOrder',
            templateUrl: 'Templates/School/VerifyOrder/Index.html',
            controller: 'verifyOrderController'
        })
         .state('WebCamra', {
             url: '/ERP/WebCamra',
             templateUrl: 'Templates/School/WebCamra.html',
             controller: 'mainController'
         })
        
        ;
    }

    angular.module('Silverzone_school_App', [])
  .config(['$stateProvider', configFn]);

})();