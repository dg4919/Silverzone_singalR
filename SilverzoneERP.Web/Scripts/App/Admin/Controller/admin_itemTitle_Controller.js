(function (app) {
    'use strict';

    var itemMater_create_Controllerfn = function ($sc, $rsc, svc) {
        $sc.entity = {};
        $sc.titleModel = {
            entitys: []
        };

        svc.get_items()
        .then(function (d) {
            $sc.entity = d.result;
        });

        $sc.submit_data = function (form) {
            //debugger;

            if (form.validate()) {
                svc.Create_ItemTitle($sc.titleModel)
                .then(function (d) {
                    console.log(d.result);
                });
            }
        }

        $sc.validationOptions = {
            rules: {
                subject: {            // use with name attribute in control
                    required: true
                }
            }
        }

    }

    app
    .controller('itemMater_create', ['$scope', '$rootScope', 'admin_bookCategory_Service', itemMater_create_Controllerfn])

})(angular.module('Silverzone_admin_app'));
