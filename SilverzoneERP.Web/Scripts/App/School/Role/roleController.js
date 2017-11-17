(function (app) {
    
    var role_fun = function ($scope, Service, $filter, roleService) {

        $scope.Role = {};
        $scope.resetCopy = angular.copy($scope.Role);
        var IsActiveList = [];
        var RoleList_All;
        $scope.Changed = false;
        $scope.selectedIndex = "1";
        $scope.isEdit = false;

        function Reset() {
            $scope.Role = angular.copy($scope.resetCopy);
            $scope.Role.FormDetails = JSON.parse(forms);
            $scope.isEdit = false;
            $scope.selectedIndex = "1";
            $scope.Changed = false;
            IsActiveList = [];
            RoleList_All = null;
            
           // GetRolePermission();
            //GetFormGroupWise();
        }
        $scope.Add = function () {
            debugger;
            $scope.isEdit = true;
            //$scope.Role = { "RoleId": 0, "FormDetails": JSON.parse(forms) };
        }
        $scope.Back = function () {
            debugger;
            Reset();
            // $scope.Role = { "RoleId": 0, "FormDetails": JSON.parse(forms) };
            //$scope.isAdd = false;
        }

        $scope.Submit = function () {
           
         
            //return false;
            roleService.Submit($scope).then(function (response) {
                if (response.result == 'Success') {
                    alert(response.message);
                    $scope.Cancel();
                    GetRolePermission();
                    $scope.Role = { "RoleId": 0, "FormDetails": JSON.parse(forms) };
                    $scope.isAdd = false;
                }
                else
                    alert(response.message);
            });
        }

        function GetRolePermission() {
            Service.Get('school/role/GetRolePermission').then(function (response) {
                debugger;
                $scope.RolePermissionList = response.result;
            });
        }
        var forms;
        function GetFormGroupWise() {
            Service.Get('school/formManagement/GetFormGroupWise').then(function (response) {
                $scope.Role.FormDetails = response.result;
                forms = JSON.stringify(response.result);
            });
        }
        GetFormGroupWise();
        GetRolePermission();

        $scope.Edit = function (index) {
            $scope.Role = { "RoleId": 0, "FormDetails": JSON.parse(forms) };
            roleService.Edit(index, $scope);
            $scope.isAdd = true;
            $scope.isCheckedPermission = true;
        }

     

       

        var firstTime = true;
        //$scope.Edit = function () {
        //    $scope.isEdit = !$scope.isEdit;
        //    if (firstTime)
        //    {
        //        Service.Get('role/Get').then(function (response) {
        //            debugger;
        //            $scope.RoleList = response.result;
        //            $scope.RoleList.splice(0, 0, { "RoleId": 0, "RoleName": "------------Select Role------------" });
                                   
        //            firstTime = false;
        //        });                
        //    }
        //    $scope.SelectedRoleId = 0;
        //    //roleService.Edit(index, $scope);          
        //}
        $scope.SelectedIndexChanged = function () {
            debugger;
            $scope.Role = { "RoleId": 0 };
            //Clear selected Permission
            $scope.Role.FormDetails = JSON.parse(forms);
            $scope.isEdit = !$scope.isEdit;
            $scope.RolePermissionList = [];

            if ($scope.SelectedRoleId != 0) {
                var getData = $filter("filter")($scope.RoleList, { RoleId: $scope.SelectedRoleId }, true);
                if (getData.length != 0) {
                    Service.Get('school/role/GetRolePermissionByRoleId?RoleId=' + $scope.SelectedRoleId).then(function (response) {
                        $scope.RolePermissionList = response.result;
                        $scope.Role.RoleId = $scope.SelectedRoleId;
                        $scope.Role.RoleName = getData[0].RoleName;
                        $scope.Role.RoleDescription = getData[0].RoleDescription;
                        
                       
                        if ($scope.RolePermissionList.length != 0) {
                            angular.forEach($scope.Role.FormDetails, function (item) {
                                angular.forEach(item.Forms, function (frm) {
                                    var getForm = $filter("filter")($scope.RolePermissionList[0].Forms, { FormId: frm.FormId }, true);
                                    if (getForm.length != 0) {
                                        frm.Permission.Add = getForm[0].Permission.Add;
                                        frm.Permission.Edit = getForm[0].Permission.Edit;
                                        frm.Permission.Read = getForm[0].Permission.Read;
                                        frm.Permission.Print = getForm[0].Permission.Print;
                                        frm.Permission.Delete = getForm[0].Permission.Delete;
                                    }                                    
                                    frm.isChecked = roleService.Get_isChecked(frm);
                                });
                                item.isAll = roleService.Get_isAll(item.Forms);
                            });
                        }                                                 
                    });
                }
            }            
        }
        $scope.Delete = function (index) {
            if (confirm("Are you want to delete ?") == true) {
                Service.Delete($scope.RolePermissionList[index].Id, 'school/user/DeleteRole').then(function (response) {
                    if (response.result == 'Success') {
                        alert(response.message);
                        Reset();
                        GetRole();
                    }
                    else
                        alert(response.message);
                });
            }
        }

        $scope.Cancel = function () {
            debugger;
            $scope.isEdit = !$scope.isEdit;
            $scope.Role = { "RoleId": 0 };
            

            //Clear selected Permission
            $scope.Role.FormDetails =JSON.parse(forms);

            //angular.forEach($scope.Role.FormDetails, function (item) {
            //    angular.forEach(item.Forms, function (frm) {
            //        frm.isChecked = false;
            //        frm.Permission.Add = false;
            //        frm.Permission.Edit = false;
            //        frm.Permission.Read = false;
            //        frm.Permission.Print = false;
            //        frm.Permission.Delete = false;                    
            //    });
            //    item.isAll = false;
            //});

           // $scope.isEdit = false;

        }
        
        $scope.Checked_Unchecked_GroupWise = function (data, index, isDirect) {
            debugger;
            isPermission();
            roleService.Checked_Unchecked_GroupWise(data, index, isDirect);
        }

        $scope.Checked_Unchecked = function (data, index)
        {
            isPermission();
            roleService.Checked_Unchecked(data, index);                  
        }

        $scope.All = function (data) {
            isPermission();
            roleService.All(data)           
        }
        $scope.All_Add = function (data) {
            debugger;
            isPermission();
            roleService.All_Add(data);
        }

        $scope.All_Edit = function (data) {
            debugger;
            isPermission();
            roleService.All_Edit(data);
        }

        $scope.All_Delete = function (data) {
            debugger;
            isPermission();
            roleService.All_Delete(data);
        }

        $scope.All_Read = function (data) {
            debugger;
            isPermission();
            roleService.All_Read(data);
        }

        $scope.All_Print = function (data) {
            debugger;
            isPermission();
            roleService.All_Print(data);
        }
        $scope.isCheckedPermission = false;
        function isPermission() {//Check here atleast one permission is checked or not
           // debugger;
            $scope.isCheckedPermission = false;
           // debugger;
            if (angular.isUndefined($scope.Role.FormDetails)) {
                $scope.isCheckedPermission = false;
                return false;
            }
            $.each($scope.Role.FormDetails, function (i, item) {
                $.each(item.Forms, function (i, frm) {
                    //debugger;
                    if (frm.Permission.Add == true || frm.Permission.Edit == true || frm.Permission.Read == true || frm.Permission.Print == true || frm.Permission.Delete == true) {
                        {
                            $scope.isCheckedPermission = true;
                            return false;
                        }
                    }
                });
            });
        }
    }

  
    app.controller('roleController', ['$scope', 'Service', '$filter', 'roleService', role_fun]);
   
})(angular.module('SilverzoneERP_App'));