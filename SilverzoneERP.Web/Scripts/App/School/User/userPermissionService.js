(function () {
    var userPermission_fun = function ($http, $q, globalConfig, roleService) {
        var apiUrl = globalConfig.apiEndPoint;
        var fact = {}

        fact.GetUserPermission = function (UserId, RoleId) {

            var data = {};
            data.UserPermission = [];
            data.Preserve_UserPermission = []

            var defer = $q.defer();

            $http({
                url: apiUrl + 'school/account/GetUserPermission?userId=' + UserId + '&RoleId=' + RoleId,
                method: 'GET'
            })
             .success(function (response) {
                 debugger;
                 //Preserve FormList fro compare permission is change or not
                 data.Preserve_UserPermission = JSON.parse(JSON.stringify(response.result));
                 data.UserPermission = response.result;

                 angular.forEach(data.UserPermission, function (item) {

                     angular.forEach(item.Forms, function (frm) {
                         frm.isChecked = roleService.Get_isChecked(frm);
                     });

                     item.isAll = roleService.Get_isAll(item.Forms);
                 });

                 defer.resolve(data);
             })
             .error(function (error) {
                 defer.reject('something went wrong..');
             });
            return defer.promise;
        }

        fact.Submit = function (UserId, UserPermission, Preserve_UserPermission) {
            debugger;
           
            var obj = {};
            obj.UserId = UserId
            obj.Forms = [];

            angular.forEach(UserPermission, function (item, Parentindex) {

                angular.forEach(item.Forms, function (frm, ChildIndex) {
                    debugger;
                    if ((frm.Permission.Add == true || frm.Permission.Edit == true || frm.Permission.Read == true || frm.Permission.Print == true || frm.Permission.Delete == true) || (frm.Permission.Add == false && frm.Permission.Edit == false && frm.Permission.Read == false && frm.Permission.Print == false && frm.Permission.Delete == false)) {
                        if (Preserve_UserPermission[Parentindex].Forms[ChildIndex].Permission.Add != frm.Permission.Add || Preserve_UserPermission[Parentindex].Forms[ChildIndex].Permission.Edit != frm.Permission.Edit || Preserve_UserPermission[Parentindex].Forms[ChildIndex].Permission.Read != frm.Permission.Read || Preserve_UserPermission[Parentindex].Forms[ChildIndex].Permission.Print != frm.Permission.Print || Preserve_UserPermission[Parentindex].Forms[ChildIndex].Permission.Delete != frm.Permission.Delete) {
                            var for_obj = { "FormId": frm.FormId, "Permission": frm.Permission };
                            obj.Forms.push(for_obj);
                        }
                    }
                });
            });
            //prompt('', JSON.stringify(obj));
            //return false;
           
            var defer = $q.defer();
            

            $http({
                url: apiUrl + 'school/user/userPermission',
                method: 'POST',
                data: obj
            })
                .success(function (response) {
                    defer.resolve(response);
                })
                .error(function (error) {
                    defer.reject('something went wrong..');
                });
            return defer.promise;
        }

        return fact;
    }

    angular.module('SilverzoneERP_App')
    .factory('userPermissionService', ['$http', '$q', 'globalConfig', 'roleService', userPermission_fun]);

})();