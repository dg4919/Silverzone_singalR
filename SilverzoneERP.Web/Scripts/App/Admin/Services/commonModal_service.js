(function () 
{
    var commonModal_servicefn = function ($modal) {
        var fac = {};

        fac.deleteModal = function (title) {

            var Template = ' <div class="modal-header">                                                           '
                         + '  <h4 class="box-title">Delete '+ title +'</h4>                                       '
                         + ' </div>                                                                               '
                         + ' <div class="modal-body">                                                             '
                         + ' <p> </p><h3>Are You Sure Want To Delete This Record ? </h3><p></p>                   '
                         + ' </div>                                                                               '
                         + ' <div class="modal-footer">                                                           '
                         + '    <button class="btn btn-primary pull-left" ng-click="ok()">Yes</button>            '
                         + '    <button class="btn btn-warning" ng-click="cancel()">No</button>                   '
                         + ' </div>                                                                               '

            modalInstance = $modal.open({
                template: Template,
                controller: 'modalController',
                size: 'sm'
            });

            // when modal will close
            return modalInstance.result;
        }

        return fac;
    }

    var commonModal_ctrlfn = function($sc, $modalInstance) {

        $sc.ok = function() {
            $modalInstance.close();
        }

        $sc.cancel = function() {
            $modalInstance.dismiss();
        }
    }

    angular.module('Silverzone_admin_app')
        .service('admin_modalService', ['$uibModal', commonModal_servicefn])
        .controller('modalController', ['$scope', '$uibModalInstance', commonModal_ctrlfn])
    ;

})();