/// <reference path="../../Lib/angular-1.5.8.js" />

(function () {
    var configFn = function ($stateProvider) {

        $stateProvider
          .state('press_Inventory_create', {
              url: '/ERP/Inventory/Press/Create',
              template: '<press:inventory></press:inventory>'
          })
          .state('dealer_Inventory_create', {
              url: '/ERP/Inventory/Dealer/Create',
              template: '<press:inventory></press:inventory>',
          })
          .state('school_Inventory_Create', {
              url: '/ERP/Inventory/School/Create',
              template: '<press:inventory></press:inventory>',
          })
          .state('counter_Inventory_create', {
              url: '/ERP/Inventory/CounterSale/Create',
              template: '<counter:inventory></counter:inventory>',
          })
          .state('EditInventory', {
              url: '/ERP/Inventory/Challan/Edit',
              template: '<edit_inventory></edit_inventory>',
              params: { challanNo: null }
          })
          .state('VerifyInventory', {
              url: '/ERP/Inventory/Verify',
              template: '<verify:inventory></verify:inventory>',
              params: { challanNo: null }
          })
          .state('InventoryInfo', {
              url: '/ERP/Inventory/Detail',
              template: '<inventory:detail></inventory:detail>'
          })
          .state('InvoiceSearch', {
              url: '/ERP/Invoice/Search',
              template: '<invoice-search></invoice-search>'
          })

          .state('SourceDetail_Create', {
              url: '/ERP/Inventory/SourceDetail',
              
              template: '<inventory-source></inventory-source>'
          })
        //.state('Inventory_PO_Create', {
        //      url: '/ERP/Inventory/Purchase_Order/Create',
        //      templateUrl: 'Templates/Inventory/purchaseOrder.html',
        //      controller: 'inventoy_PO_Controller'
        //})
         .state('Inventory_PO_Create', {
             url: '/ERP/Inventory/Purchase_Order/Create',
             template: '<purchase:order-create></purchase:order-create>',
         })
         .state('counter_sale_Create', {
             url: '/ERP/counterSale/Create',
             template: '<counter:sale-create></counter:sale-create>',
         })
         .state('Inventory_PO_Edit', {
             url: '/ERP/Inventory/Purchase_Order/Edit',
             template: '<purchase:order-edit></purchase:order-edit>',
         })

         .state('Inventory_PO_Search', {
             url: '/ERP/Inventory/Purchase_Order/Search',
             template: '<search:pending-po></search:pending-po>',
         })
         .state('Dispatch_Other_Item', {
             url: '/ERP/Inventory/otherItems',
             template: '<dipacth:other-item></dipacth:other-item>',
         })
        .state('RC_register', {
            url: '/ERP/RC/Register',
            template: '<rc:register></rc:register>',
        })
        ;
    }

    var moduleDependencies = [
        'SilverzoneERP_invenotry_component',
        'SilverzoneERP_invenotry_service'
    ];

    angular
        .module('Silverzone_invenotry_App', moduleDependencies)     // inject components of inventory
        .config(['$stateProvider', configFn])

    ;

})();