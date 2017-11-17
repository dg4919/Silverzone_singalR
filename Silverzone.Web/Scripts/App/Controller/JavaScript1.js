
var fn = function ($sc, $rsc, $modalInstance, svc) {
    //$sc.categries = [];          // contain array of categry list, by default set as empty

    $sc.categries = { first_categry: [], second_categry: [], third_categry: [] };
    $rsc.categries_name = { first_categry_name: null, second_categry_name: null, third_categry_name: null };
    $rsc.selected_categryImg = null;
    $rsc.isVisible = false;
    var vehicle_Id;
    //debugger;

    // $event, $log, $index -> angular predefine service inject by ng-click on page
    $sc.get_categories = function (categry) {
        var categry_id = categry == null ? null : categry.id;
        var selected_cat_level = categry == null ? null : categry.categry_level;

        svc.sub_categyList(categry_id).then(function (result) {
            // console.log('service has run');

            switch (selected_cat_level) {
                case null:
                    $sc.categries.first_categry = result;
                    break;
                case 1:
                    $sc.selected_categry = categry;

                    $sc.categries.second_categry = chunk(result, 3);
                    $rsc.categries_name.first_categry_name = categry.category_name;

                    $sc.categries.third_categry = $rsc.categries_name.second_categry_name = null;
                    break;
                case 2:
                    $sc.selected_subCategry = categry;                     // use to apply active class on whcich categry is choosen
                    $rsc.selected_categryImg = categry.image_url;         // bind image aftr selection of categry

                    if (result.length !== 0) {
                        // for level 2 => click on sub-category
                        $sc.categries.third_categry = result;
                        $rsc.categries_name.second_categry_name = categry;
                        //$rsc.categries_name.second_categry_name = (result.length !== 0) ? categry.category_name : null;
                    }
                    else {
                        $sc.submit_data(categry);
                    }
                    break;
            }
        },
        function (error) {
            console.log('in error..  ' + error.data.Message);
        }).finally(function () {
            console.log('i will always execute either sucess/fail');
        });
    }

    $sc.get_categories(null);               // call 1st time to show categries

    $sc.submit_data = function (categry) {

        if (categry.categry_level === 3) {
            $rsc.categries_name.third_categry_name = ' | ' + categry.category_name;
        }
        else {
            $rsc.categries_name.second_categry_name = categry;
        }

        // for cars, Motorcycles, Scooters
        if ([68, 110, 111].indexOf($rsc.categries_name.second_categry_name.id) > -1) {
            console.log('categry is caught to show vehicle model...');
            vehicle_Id = categry.id;
            get_vehicleModels();
        }

        $rsc.isVisible = true;

        $modalInstance.close();
        console.log('close popup');
    };

    $sc.cancel = function () {
        $modalInstance.dismiss();
        console.log('cancel popup');
    }

    // this fx use by controller, not by view so no need to define with $scope
    function chunk(arr, size) {
        var newArr = [];
        for (var i = 0; i < arr.length; i += size) {
            newArr.push(arr.slice(i, i + size));
        }
        return newArr;
    }

    function get_vehicleModels() {
        svc.find_vehicleModels(vehicle_Id).then(function (Data) {
            console.log('In suces.....  ');
            $rsc.models = Data;
        },
       function (error) {
           console.log('in error..  ' + error.data.Message);
       })
    }

}

app.controller('catgoryController', ['$scope', '$rootScope', '$modalInstance', 'cateoryList_Service', fn]);

//***************************************************************


(function () {
    'use strict';

    // if i forget to write var before fn below : then gives error bcoz of 'use strict' in console, othwerwise it run
    var fn = function ($scope, $modal, adPosting_svc, city_svc) {
        $scope.model = {};
        $scope.countryList = [];
        $scope.localityyList = [];
        $scope.ticked = false;

        $scope.saveData = function (data) {
            $scope.model = data;

            adPosting_svc.savePosting($scope.model).then(function (result) {
                console.log('suces fx..');
            },
            function (error) {
                console.log('in error..  ' + error.data.Message);
            }).finally(function () {
                console.log('i will always execute either sucess/fail');
            });
        }

        // modal pop up will bind at the end of the above controller page
        $scope.ShowModal = function () {
            var modalInstance = $modal.open({
                templateUrl: '/HTML/Category_poupup.html',
                controller: 'catgoryController',            // No need to specify this controller on page explicitly    
                size: 'lg',
                resolve: {
                    //menu: function () {
                    //    return {};
                    //}
                }
            });
            modalInstance.result.then(function () {
                //on ok button press 
            }, function () {
                //on cancel button press
                console.log("Modal Closed");
            });
        }

        $scope.getCities_list = function (cityData) {
            var cityId = cityData == null ? null : cityData.id;

            // debugger;
            city_svc.getCityList(cityId).then(function (data) {
                //console.log('data is  ' + data);
                if (cityId === null)
                    $scope.countryList = data;
                else
                    $scope.localityyList = data;
            },
            function (error) {
                console.log('in error..  ' + error.data.Message);
            }).finally(function () {
                console.log('i will always execute either sucess/fail');
            });
        }

        $scope.getCities_list(null);

        function random_UID() {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            for (var i = 0; i < 15; i++)
                text += possible.charAt(Math.floor(Math.random() * possible.length));

            return text;
        }

        $scope.temp_imageId = random_UID();

        // to disable console & debug output in browser..
        function disable_logs() {
            if (!window.console) window.console = {};
            var methods = ["log", "debug", "warn", "info"];
            for (var i = 0; i < methods.length; i++) {
                console[methods[i]] = function () { };
            }
        }

        disable_logs();

    }

    // app is global module
    // change name of module on 1 place will reflect every where
    app.controller('ad_postController', ['$scope', '$modal', 'adsPostingService', 'cityListServie', fn]);
})();

