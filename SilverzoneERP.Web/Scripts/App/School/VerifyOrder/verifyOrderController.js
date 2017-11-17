(function (app) {
    var verifyOrder_fun = function ($scope, $rootScope, Service, $filter, $state, $uibModal, Order) {

       
        Order.GetOrder($scope);

        $scope.verifyOrder = function (data) {
            Order.BookVerify($scope, data);
        }
    }

    var verify_Book_Order_fun = function ($scope, $rootScope, modalInstance, $filter, Service, data) {

        debugger;

        $scope.VerifyBookOrder = data;
        $scope.SelectedClassId = "";       
        $scope.ClassList = JSON.parse(data.School.EnrollmentOrderSummary);
        var resetCopy = { "PO_masterId": data.PO_mId, "From": 6, "To": 7, "srcFrom": data.School.EventManagementId };
        function Get_Book() {
            Service.Get('school/EventManagement/GetAllBook?SubjectId=' + data.School.EventId).then(function (response) {
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

        function GetClassName(ClassId) {
            var Class_Filter = $filter("filter")($scope.ClassList, { 'ClassId': ClassId }, true);
            if(Class_Filter.length!=0)
            {
                return Class_Filter[0].ClassName;
            }
            return '';
        }
        function GetBookName(BookId) {
            var Book_Filter = $filter("filter")($scope.AllBook, { 'BookId': BookId }, true);
            if (Book_Filter.length != 0) {
                return Book_Filter[0].CategoryName;
            }
            return '';
        }

        $scope.Add = function (form) {
            if (form.validate() == false)
                return false;
            debugger;
            if (angular.isUndefined($scope.BookOrder.Guid)) {
                var item_filter = $filter("filter")($scope.VerifyBookOrder.Order, { BookId: $scope.BookOrder.BookId, ClassId: $scope.SelectedClassId }, true);
                if (item_filter.length != 0) {
                    Service.Notification($rootScope, "Already exists !");
                    return false;
                }
               

                $scope.BookOrder.ClassId = $scope.SelectedClassId;
                $scope.BookOrder.ClassName = GetClassName($scope.SelectedClassId);
                $scope.BookOrder.BookName = GetBookName($scope.BookOrder.BookId);
                $scope.BookOrder.Guid = Service.guid();

                $scope.VerifyBookOrder.Order.push($scope.BookOrder);                

                Service.Notification($rootScope, "Order added !");
            }
            else
            {
                var item_filter = $filter("filter")($scope.VerifyBookOrder.Order, { Guid: $scope.BookOrder.Guid }, true);
                if (item_filter.length != 0) {
                    item_filter[0].Quantity = $scope.BookOrder.Quantity;
                    Service.Notification($rootScope, "Quantity updated !");                    
                }
            }

            var BookId = $scope.BookOrder.BookId;
            $scope.BookOrder = angular.copy(resetCopy);
            $scope.BookOrder.BookId = BookId;

            var index = $scope.ClassList.map(function (item) {
                return item.ClassId;
            }).indexOf($scope.SelectedClassId);

            $scope.SelectedClassId = $scope.ClassList[index + 1].ClassId;
            $scope.Class_Selected_IndexChanged($scope.SelectedClassId);
        }

        $scope.Edit = function (data) {
            debugger;
            $scope.BookOrder = angular.copy(resetCopy);
            $scope.SelectedClassId = data.ClassId;
            $scope.Class_Selected_IndexChanged($scope.SelectedClassId);
            $scope.BookOrder.BookId = data.BookId;
            $scope.BookOrder.Quantity = data.Quantity;
            $scope.BookOrder.POId = data.Id;
            $scope.BookOrder.Guid = data.Guid;
        }

        $scope.Class_Selected_IndexChanged = function (ClassId) {
            debugger;
            var CategoryName = "";
            if (!angular.isUndefined($scope.Book)) {
                var Book_filter = $filter("filter")($scope.Book, { BookId: $scope.BookOrder.BookId }, true);
                if (Book_filter.length != 0)
                    CategoryName = Book_filter[0].CategoryName;
            }


            $scope.Book = $filter("filter")($scope.AllBook, { 'ClassId': ClassId == '' ? 0 : ClassId }, true);
            //Change class
            if (CategoryName != "") {
                var Book_filter = $filter("filter")($scope.Book, { CategoryName: CategoryName }, true);
                if (Book_filter.length != 0)
                    $scope.BookOrder.BookId = Book_filter[0].BookId;
            }
        }

        $scope.Verify = function () {
            debugger;
            var Order_Filter = $filter("filter")($scope.VerifyBookOrder.Order, function (data) {
                if (data.Quantity == '' || data.Quantity == null)
                    return data;
            });
                
                //{ {Quantity: ''} || {Quantity: null} }, true);
            if (Order_Filter.length != 0)
            {
                Service.Notification($rootScope, "Quantity can't be empty !");
                return false;
            }

            Service.Create_Update($scope.VerifyBookOrder.Order, 'School/verifyOrder/Verify?PO_MId=' + $scope.VerifyBookOrder.PO_mId).then(function (response) {
                debugger;
                if (angular.lowercase(response.result) == 'success') {
                    Service.Notification($rootScope, response.message);
                    modalInstance.close(null);
                }               
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
            if (!angular.isUndefined($scope.Book)) {
                var Book_filter = $filter("filter")($scope.Book, { BookId: $scope.BookOrder.BookId }, true);
                if (Book_filter.length != 0)
                    CategoryName = Book_filter[0].CategoryName;
            }


            $scope.Book = $filter("filter")($scope.AllBook, { 'ClassId': ClassId == '' ? 0 : ClassId }, true);
            //Change class
            if (CategoryName != "") {
                var Book_filter = $filter("filter")($scope.Book, { CategoryName: CategoryName }, true);
                if (Book_filter.length != 0)
                    $scope.BookOrder.BookId = Book_filter[0].BookId;
            }
        }
    }

    app.controller('verifyOrderController', ['$scope', '$rootScope', 'Service', '$filter', '$state', '$uibModal', 'OrderService', verifyOrder_fun])
    .controller('verifyBookOrderController', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'data', verify_Book_Order_fun]);
})(angular.module('SilverzoneERP_App'));