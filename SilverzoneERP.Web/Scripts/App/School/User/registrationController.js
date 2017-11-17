
(function (app) {

    var user_fun = function ($scope, $location, $rootScope, Service, $filter, $state, globalConfig) {
        
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.rangeSize = 5;
        $scope.startIndex = 0;        

        var IsActiveList = [];
        var UserList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;

       // $scope.Permission = Service.GetPermission(60, 4);       


        $scope.User = {
            "GenderType": "1",
            "ProfilePic": $rootScope.UserInfo.ImgPrefix + "ProfilePic/placeholderImage.png"
        };
        $scope.resetCopy = angular.copy($scope.User);

        function GetRole() {
            Service.Get('School/role/Get').then(function (response) {                
               $scope.Role=response.result;                
            });
        }

        function Reset() {
            $scope.User = angular.copy($scope.resetCopy);
            $scope.isEdit = false;
            $scope.selectedIndex = "1";
            $scope.Changed = false;
            IsActiveList = [];
            UserList_All = null;           
        }

        GetRole();
                
        $scope.Submit = function (form) {
            debugger
            if (form.validate() == false)
                return false;
            Service.Create_Update($scope.User, 'School/account/registration').then(function (response) {
                debugger;
                if (response.result == 'Success') {
                    $state.reload();
                }
                Service.Notification($rootScope,response.message);
               // Service.Alert(response.message);
            });
        }
        $scope.validationOptions = {
            rules: {
                UserName: {            // field will be come from component
                    required: true,
                    maxlength: 100
                },
                DOB: {
                    required: true                    
                },
                EmailID: {
                    required: true,
                    maxlength: 50
                },
                RoleId: {
                    required: true
                }
                ,
                Password: {
                    required: true
                },
                ConfirmPassword: {
                    required: true
                },
                UserAddress: {
                    maxlength: 200
                },
                MobileNumber: {
                    maxlength: 10,
                    minlength: 10
                }
            }
        }

        $scope.UploadImage = function (file) {            
            Service.readAsDataURL(file, $scope).then(function (result) {
                $scope.User.ProfilePic = result;
            });
        };

        $scope.Edit = function (data) {
            debugger;
            $scope.User.Id = data.Id;
            $scope.User.UserName = data.UserName;
            if (data.ProfilePic == null)
                $scope.User.ProfilePic = $rootScope.UserInfo.ImgPrefix + "ProfilePic/placeholderImage.png";
            else
                $scope.User.ProfilePic = $rootScope.UserInfo.ImgPrefix + data.ProfilePic;

            $scope.User.EmailID = data.EmailID;
            $scope.User.GenderType = ''+data.GenderType;
            $scope.User.Password = data.Password;
            $scope.User.ConfirmPassword = data.Password;
            $scope.User.UserAddress = data.UserAddress;
            $scope.User.MobileNumber = data.MobileNumber;
            $scope.User.RoleId = data.RoleId;
            $scope.User.DOB = $filter('dateFormat')(data.DOB, 'MM/DD/YYYY');;
            
            $scope.isEdit = true;
        }
      
        $scope.Add = function () {
            debugger;
            if ($scope.Changed) {
                var result = confirm('Do you want to save change ?');
                if (result == true) {
                    $scope.Active_Deactive();
                    $scope.Changed = false;
                } else {
                    $scope.SelectedIndexChanged($scope.selectedIndex);
                    $scope.Changed = false;
                }
            }
            $scope.isEdit = true;

        }

        $scope.Back = function () {
            $scope.User = angular.copy($scope.resetCopy);
            $scope.isEdit = false;
            Service.Reset($scope);
        }

        $scope.SelectedIndexChanged = function (selectedIndex) {
            debugger;
            $scope.UserList = Service.SelectedIndexChanged(selectedIndex, UserList_All);
        }
       
        $scope.IsActive = function (Id) {
            debugger;
            IsActiveList = Service.IsActive(Id, IsActiveList, $scope);
        }
        
        $scope.Active_Deactive = function () {
            Service.Create_Update(IsActiveList, 'School/account/Active_Deactive')
             .then(function (response) {
                 alert(response.message);
                 if (response.result == 'Success') {
                     GetUser(preserve_NewValue, preserve_OldValue);
                 }
             });
        }
        var preserve_NewValue, preserve_OldValue;
        $scope.$watch("currentPage", function (newValue, oldValue) {
            debugger;
            GetUser(newValue, oldValue);
            preserve_NewValue = newValue;
            preserve_OldValue = oldValue;
        });

        function GetUser(newValue, oldValue)
        {
            var startIndex = ((newValue - 1) * $scope.itemsPerPage);

            Service.Get('School/account/GetUser?StartIndex=' + startIndex + '&Limit=' + $scope.itemsPerPage)
                .then(function (response) {
               
                debugger;
                UserList_All = response.result;
                $scope.total = response.Count;
                $scope.SelectedIndexChanged($scope.selectedIndex);

                $scope.range = function () {
                    var ret = [];
                    var start = ((Math.ceil($scope.currentPage / $scope.rangeSize) - 1) * $scope.rangeSize) + 1;
                    for (var i = start; i < (start + $scope.rangeSize) && i <= $scope.pageCount() ; i++)
                        ret.push(i);
                    return ret;
                };


                $scope.prevPage = function () {
                    if ($scope.currentPage > 1) {
                        $scope.currentPage--;
                    }
                };

                $scope.prevPageDisabled = function () {
                    return $scope.currentPage === 1 ? "disabled" : "";
                };

                $scope.firstPage = function () {
                    $scope.currentPage = 1;
                }
                $scope.firstPageDisabled = function () {
                    return $scope.currentPage === 1 ? "disabled" : "";
                };

                $scope.lastPage = function () {
                    $scope.currentPage = $scope.pageCount();
                }
                $scope.lastPageDisabled = function () {
                    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                };

                $scope.nextPage = function () {
                    if ($scope.currentPage <= $scope.pageCount()) {
                        $scope.currentPage++;
                    }
                };

                $scope.nextPageDisabled = function () {
                    return $scope.currentPage === $scope.pageCount() ? "disabled" : "";
                };

                $scope.pageCount = function () {
                    return Math.ceil($scope.total / $scope.itemsPerPage);
                };

                $scope.setPage = function (n) {
                    if (n > 0 && n <= $scope.pageCount()) {
                        $scope.currentPage = n;
                    }
                };

            }, function () {

            })
        }
    }

   
    app.controller('registrationController', ['$scope', '$location', '$rootScope', 'Service', '$filter', '$state', 'globalConfig', user_fun]);

})(angular.module('SilverzoneERP_App'));