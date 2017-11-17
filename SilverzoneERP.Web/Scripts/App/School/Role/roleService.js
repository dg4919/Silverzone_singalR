(function () {

    var fun = function ($http, $q, globalConfig, $filter)
    {
        var apiUrl = globalConfig.apiEndPoint,
            fac = {}
        fac.Edit=function(index,sc){
            debugger;
            sc.Role.RoleId = sc.RolePermissionList[index].RoleId;
            sc.Role.RoleName = sc.RolePermissionList[index].RoleName;
            sc.Role.RoleDescription = sc.RolePermissionList[index].RoleDescription;

            var FormList = [];
            // Start Collect all from from (User management,Olympiad ,Other and report)
            angular.forEach(sc.RolePermissionList[index].Forms, function (item) {
                FormList.push(item);
            });
            // End Collect all from from (User management,Olympiad ,Other and report)           

            angular.forEach(sc.Role.FormDetails, function (item) {

                angular.forEach(item.Forms, function (frm) {

                    var getForm = $filter("filter")(FormList, { FormId: frm.FormId }, true);
                    if (getForm.length != 0) {
                        frm.Permission.Add = getForm[0].Permission.Add;
                        frm.Permission.Edit = getForm[0].Permission.Edit;
                        frm.Permission.Read = getForm[0].Permission.Read;
                        frm.Permission.Print = getForm[0].Permission.Print;
                        frm.Permission.Delete = getForm[0].Permission.Delete;                        
                    }
                    else {
                        frm.Permission.Add = false;
                        frm.Permission.Edit = false;
                        frm.Permission.Read = false;
                        frm.Permission.Print = false;
                        frm.Permission.Delete = false;
                    }
                    frm.isChecked = fac.Get_isChecked(frm);
                });

                item.isAll = fac.Get_isAll(item.Forms);

            });           
            sc.isEdit = true;
            $(window).scrollTop(0);
        }

        fac.Submit = function (sc) {
            var defer = $q.defer();

            var obj = { "Id": sc.Role.RoleId, "RoleName": sc.Role.RoleName, "RoleDescription": sc.Role.RoleDescription, "Forms": [] };

            angular.forEach(sc.Role.FormDetails, function (value, key) {
                angular.forEach(value.Forms, function (frm, frmkey) {
                    if (frm.Permission.Add == true || frm.Permission.Edit == true || frm.Permission.Read == true || frm.Permission.Print == true || frm.Permission.Delete == true) {
                        var for_obj = { "FormId": frm.FormId, "Permission": frm.Permission };
                        obj.Forms.push(for_obj);
                    }
                });
            });

            
          
            $http({
                url: apiUrl + 'school/role/Create_Update',
                method: 'POST',
                data:obj
            })
                .success(function (response) {                  
                    defer.resolve(response);
                })
                .error(function (error) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;

            //Service.Create_Update(obj, 'role/Create_Update').then(function (response) {
            //    if (response.result == 'Success') {
            //        alert(response.message);
            //        sc.Cancel();
            //        sc.RolePermissionList = response.data;
            //    }
            //    else
            //        alert(response.message);
            //});
        }

        fac.Get_isChecked = function (data) {
            debugger;
            if (data.Permission.Add == true && data.Permission.Edit == true && data.Permission.Read == true && data.Permission.Print == true && data.Permission.Delete == true)
                return true;
            else
                return false;
        }

        fac.Get_isAll = function (data) {
            debugger;
            var getForm = $filter("filter")(data, { isChecked: true }, true);

            if (data.length == getForm.length)
                return true;
            else
                return false;
        }

        fac.Checked_Unchecked_GroupWise = function (data, index, isDirect) {

            debugger;
            data.Forms[index].Permission.Add = data.Forms[index].isChecked;
            data.Forms[index].Permission.Edit = data.Forms[index].isChecked;
            data.Forms[index].Permission.Read = data.Forms[index].isChecked;
            data.Forms[index].Permission.Print = data.Forms[index].isChecked;
            data.Forms[index].Permission.Delete = data.Forms[index].isChecked;

            if (isDirect) {
                data.isAll = this.Get_isAll(data.Forms);
            }
            
        }

        fac.Checked_Unchecked = function (data, index) {
            data.Forms[index].isChecked = this.Get_isChecked(data.Forms[index]);

            data.isAll = this.Get_isAll(data.Forms);
        }

        fac.All = function (data) {           
            angular.forEach(data.Forms, function (item, index) {
                item.isChecked = data.isAll;
                fac.Checked_Unchecked_GroupWise(data, index, false)
            });
        }
        fac.All_Add = function (data) {
            angular.forEach(data.Forms, function (item, index) {
                item.Permission.Add = data.isAdd;
            });
        }
        fac.All_Edit = function (data) {
            angular.forEach(data.Forms, function (item, index) {
                item.Permission.Edit = data.isEdit;
            });
        }
        fac.All_Delete = function (data) {
            angular.forEach(data.Forms, function (item, index) {
                item.Permission.Delete = data.isDelete;
            });
        }
        fac.All_Read = function (data) {
            angular.forEach(data.Forms, function (item, index) {
                item.Permission.Read = data.isRead;
            });
        }
        fac.All_Print = function (data) {
            angular.forEach(data.Forms, function (item, index) {
                item.Permission.Print = data.isPrint;
            });
        }

        return fac;
    }

    angular.module('SilverzoneERP_App')
    .factory('roleService', ['$http', '$q', 'globalConfig', '$filter', fun]);
})();