(function (app) {

    var BookOrder_fun = function ($scope, $rootScope, modalInstance, $filter, Service, data) {
        debugger;
        $scope.SchoolDetail = data;
        $scope.SelectedClassId = "";
        $scope.BookOrderList = [];
        $scope.ClassList = data.ClassList;
        var resetCopy = { "From": 6, "To": 7, "srcFrom": $scope.SchoolDetail.EventManagementId };

        if (!angular.isUndefined(data.PO_masterId))
        {
            resetCopy.PO_masterId = data.PO_masterId;
            $scope.BookOrderList = data.Order;            
        }

        function Get_Book() {
            Service.Get('school/EventManagement/GetAllBook?SubjectId=' + $rootScope.SelectedEvent.EventId).then(function (response) {
                debugger;
                $scope.AllBook = response.result;
                resetCopy.srcTo = response.CompanyId;
                $scope.BookOrder = angular.copy(resetCopy);
            });
        }
        Get_Book();
      
        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }

        function GetPurchaseOrder() {
            debugger;
            Service.Get('school/EventManagement/GetPurchaseOrder?Id='+$scope.BookOrder.PO_masterId).then(function (response) {
                debugger;
                $scope.BookOrderList = response.result;
            });
        }

      //  GetPurchaseOrder();
        
        $scope.Submit = function (form) {
            debugger;
            if ($scope.BookOrder.srcTo == null)
            {
                Service.Notification($rootScope, 'Inventory scource not exists !');
                return false;
            }

            if (form.validate() == false)
                return false;
            if (angular.isUndefined(data.EventManagementId)) {
                var obj = {
                    SchId: data.SchId,
                    EventId: $rootScope.SelectedEvent.EventId
                };

                Service.Create_Update(obj, 'school/EventManagement/Create_Update')
                   .then(function (response) {
                       //Service.Notification($rootScope, response.message);

                       if (angular.lowercase(response.result) == 'success') {
                           $scope.BookOrder.srcFrom = response.EventManagementId;
                           $scope.SchoolDetail.EventManagementId = response.EventManagementId;
                           resetCopy.srcFrom = response.EventManagementId;
                           SaveBookOrder();
                       }
                   });
            }
            else
                SaveBookOrder();
            
           // return false;
            
        }

        function SaveBookOrder() {
            Service.Create_Update($scope.BookOrder, 'Inventory/Stock/save_purchaseOrder').then(function (response) {
                debugger;
                if (angular.lowercase(response.result) == 'success') {
                    var BookId = $scope.BookOrder.BookId;
                    $scope.BookOrder = angular.copy(resetCopy);
                    $scope.BookOrder.PO_masterId = response.PO_masterId;
                    $scope.BookOrder.BookId = BookId;
                    Service.Notification($rootScope, 'Successfully added !');
                    //Change class
                    var index = $rootScope.SelectedEvent.Class.map(function (item) {
                        return item.ClassId;
                    }).indexOf($scope.SelectedClassId);

                    $scope.SelectedClassId = $rootScope.SelectedEvent.Class[index + 1].ClassId;
                    $scope.Class_Selected_IndexChanged($scope.SelectedClassId);
                    GetPurchaseOrder();
                }
                else if (angular.lowercase(response.result) == 'exist')
                    Service.Notification($rootScope, 'Already exists !');

            });
        }

        $scope.validationOptions = {
            rules: {
                SelectedClassId: {
                    required: true
                },
                BookId: {
                    required: true
                },
                Quantity: {
                    required: true
                }
            }
        }

        $scope.Class_Selected_IndexChanged = function (ClassId) {
            debugger;
            var CategoryName = "";
            if (!angular.isUndefined($scope.Book))
            {
                var Book_filter = $filter("filter")($scope.Book, { BookId: $scope.BookOrder.BookId }, true);
                if (Book_filter.length != 0)
                    CategoryName = Book_filter[0].CategoryName;
            }
                

            $scope.Book = $filter("filter")($scope.AllBook, { 'ClassId': ClassId == '' ? 0 : ClassId }, true);
            //Change class
            if (CategoryName != "")
            {
                var Book_filter = $filter("filter")($scope.Book, { CategoryName: CategoryName }, true);
                if (Book_filter.length != 0)
                    $scope.BookOrder.BookId = Book_filter[0].BookId;
            }                       
        }

        $scope.Edit = function (data) {
            debugger;
            $scope.BookOrder = angular.copy(resetCopy);
            $scope.SelectedClassId = data.ClassId;
            $scope.Class_Selected_IndexChanged($scope.SelectedClassId);
            $scope.BookOrder.BookId = data.BookId;
            $scope.BookOrder.Quantity = data.Quantity;
            $scope.BookOrder.POId = data.Id;
        }
    }
  
    app.controller('BookOrderController', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'data', BookOrder_fun]);

})(angular.module('SilverzoneERP_App'));