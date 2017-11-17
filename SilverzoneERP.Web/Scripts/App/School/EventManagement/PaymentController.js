(function (app) {

    var Payment_Fun = function ($scope, $rootScope, modalInstance, $filter, Service, SchMngt) {
        $scope.IsPayment = true;
        $scope.IsReturn = false;     
        $scope.IsFavour = true;
        $scope.SelectedEvent = null;
        $scope.SelectedYear = null;
        var AccountDetails;

        function Get_FavourOf() {
            Service.Get('school/eventManagement/Get_FavourOf').then(function (response) {
                $scope.FavourOf = response.FavourOf;
                $scope.DrawnOnBank = response.DrawnOnBank;
                $scope.DrawnOnBank.push({Id:-1,BankName:'Other'})
            });
        }

        Get_FavourOf();

        $scope.Back = function () {
            debugger;
            modalInstance.dismiss();
        }
                
        $scope.getTotal = function (MiniStatement,upTo) {
            debugger;
            var total = 0;
            for (var i = 0; i < upTo; i++) {
                total += MiniStatement[i].Payment;
            }
            return total;
        }

        $scope.Reset = function (IsReset) {
            debugger;
            $scope.Payment = {};
            if (IsReset)
                Service.Reset($scope);

            $scope.Payment.EventmanagementId = SchMngt.EventManagement.Id;
            $scope.Payment.PaymentDate = $filter('dateFormat')(new Date(), 'DD-MMM-YYYY');

            $scope.IsFavour = true;

            $scope.Preserve_Ministatement = {};

            Service.Get('school/feePayment/MiniStatement?SchoolId=' + SchMngt.SchId).then(function (response) {
                $scope.Preserve_Ministatement = response;
                $scope.Event_Year_Change(null,null);
            });
        }

        $scope.Reset(false);

        $scope.Event_Year_Change=function(SelectedEvent, SelectedYear)
        {
            debugger
            $scope.Ministatement = {};
            
            if (SelectedEvent == null && SelectedYear == null)
            {
                $scope.Ministatement.Registration = angular.copy($scope.Preserve_Ministatement.Registration);
                $scope.Ministatement.Book = angular.copy($scope.Preserve_Ministatement.Book);
                $scope.Ministatement.Both = angular.copy($scope.Preserve_Ministatement.Both);
            }
            else if (SelectedEvent != null && SelectedYear != null)
            {
                $scope.Ministatement.Registration = $filter('filter')($scope.Preserve_Ministatement.Registration, { EventManagementYear: $scope.SelectedYear, EventId: $scope.SelectedEvent }, true);
                $scope.Ministatement.Book = $filter('filter')($scope.Preserve_Ministatement.Book, { EventManagementYear: $scope.SelectedYear, EventId: $scope.SelectedEvent }, true);
                $scope.Ministatement.Both = $filter('filter')($scope.Preserve_Ministatement.Both, { EventManagementYear: $scope.SelectedYear, EventId: $scope.SelectedEvent }, true);
            }                
            else if (SelectedYear != null)
            {
                $scope.Ministatement.Registration = $filter('filter')($scope.Preserve_Ministatement.Registration, { EventManagementYear: $scope.SelectedYear }, true);
                $scope.Ministatement.Book = $filter('filter')($scope.Preserve_Ministatement.Book, { EventManagementYear: $scope.SelectedYear }, true);
                $scope.Ministatement.Both = $filter('filter')($scope.Preserve_Ministatement.Both, { EventManagementYear: $scope.SelectedYear }, true);
            }                
            else if (SelectedEvent != null)
            {
                $scope.Ministatement.Registration = $filter('filter')($scope.Preserve_Ministatement.Registration, { EventId: $scope.SelectedEvent }, true);
                $scope.Ministatement.Book = $filter('filter')($scope.Preserve_Ministatement.Book, { EventId: $scope.SelectedEvent }, true);
                $scope.Ministatement.Both = $filter('filter')($scope.Preserve_Ministatement.Both, { EventId: $scope.SelectedEvent }, true);
            }                
            $scope.Total = {Amount:0,Paid:0};

            CalculateTotal($scope.Ministatement.Registration, true);
            CalculateTotal($scope.Ministatement.Book,true);
            CalculateTotal($scope.Ministatement.Both,false);            
        }

        function CalculateTotal(data,IsTotalAmount) {
            for (var i = 0; i < data.length; i++) {
                if (IsTotalAmount)
                    $scope.Total.Amount += data[i].TotalAmount;
                $scope.Total.Paid += data[i].TotalPaid;
            }
        }

        $scope.ModeChange = function () {
            debugger;
            if ($scope.Payment.Mode == "1")
                $scope.Payment.LetterMode = "4";
        }

        $scope.Submit = function (form) {
            debugger;
            if (form.validate() == false)
                return false;

            if (angular.isUndefined($scope.Payment.Type))
                $scope.Payment.Type = 1;

            $scope.Payment.Balance = $scope.Payment.Payment - $scope.Payment.NetBalance;
           // prompt('', JSON.stringify($scope.Payment));

            //return false;
            Service.Create_Update($scope.Payment, 'school/feePayment/Fee').then(function (response) {
                if (response.result == 'Success') {
                    Service.Notification($rootScope, response.message);                    
                    $scope.Reset(true);
                }
                else {
                    prompt('', JSON.stringify(response.message));
                }
            });
        }

        $scope.validationOptions = {
            rules: {
                AccountNo: {
                    required: true,
                    maxlength: 16,
                    minlength: 11
                },
                PaymentDate: {
                    required: true
                },
                MICRCode:{
                    required: true
                },
                PayAgainst: {
                    required: true
                },
                Payment: {
                    required: true,
                    maxlength: 10
                },
                Mode: {
                    required: true
                },
                DipositOnBankId: {
                    required: true
                },
                OtherBank:{
                    required: true
                },
                LatterMode: {
                    required: true
                },
                Cheque_DD_Reference_Receipt_No: {
                    required: true
                },
                FavourOfId: {
                    required: true
                },
                Depositor_Receiver: {
                    required: true
                }
            }
        }

        $scope.FavourOfChange = function (FavourOfId) {
            var DrownOnBank_Filter = $filter("filter")($scope.FavourOf, { Id: FavourOfId }, true);
            if (DrownOnBank_Filter.length != 0)
                $scope.DipositOnBank = DrownOnBank_Filter[0].DipositOnBanks;
        }
     
        $scope.Refund = function (Type,data) {
            debugger;
            if (data.MiniStatement.length != 0)
            {
                $scope.FavourOfChange(data.MiniStatement[0].FavourOfId);
                data.IsFavour = false;
                $scope.Payment.AccountNo = data.MiniStatement[0].AccountNo;
                $scope.Payment.Payment = data.NetBalance;
                $scope.Payment.FavourOfId = data.MiniStatement[0].FavourOfId;
                $scope.Payment.DrownOnBankId = data.MiniStatement[0].DrownOnBankId;
                $scope.Payment.Type = Type;
                if (Type == 3) {
                    $scope.Payment.Mode = "6";
                    $scope.Payment.LetterMode = "5";
                }
            }            
        }

    }
    app.controller('PaymentController', ['$scope', '$rootScope', '$uibModalInstance', '$filter', 'Service', 'SchMngt', Payment_Fun])

})(angular.module('SilverzoneERP_App'))