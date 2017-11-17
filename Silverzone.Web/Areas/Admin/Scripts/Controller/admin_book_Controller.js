
(function () {

    function getCategoriesId(book_category_listId) {
        var array = [];
        if (book_category_listId.length > 0) {
            angular.forEach(book_category_listId, function (data, key) {
                array.push(parseInt(data.id));
            });
        }
        return array;
    }

    var book_createfn = function ($sc, $rsc, svc, sharedSvc, $modal, $stateParams, $state, $timeout) {

        // form will not disable if user edit a record of book list
        $sc.disableForm = $stateParams.bookId !== undefined ? false : true;
        $sc.disableSelection = $stateParams.bookId !== undefined ? true : false;

        $sc.bookModel = {
            bookInfo: {},
            bookDetail: {},
            bookContent: []
        };

        // contain array of list / a Modal
        $sc.classList = [];
        $sc.subjectList = [];
        $sc.book_categorys = [];
        $sc.book_publisherList = ["Silverzone", "3gen"];

        // HERE in all services we r not using error()
        // class list load at runtime
        sharedSvc.getAll_subject().then(function (data) {
            $sc.subjectList = data.result;
        });

        // get subjects
        $sc.get_class = function (subjectId) {
            sharedSvc.get_class_bySubjctId(subjectId).then(function (d) {
                $sc.classList = d.result;
            }, function () {
                console.log("error occured");
            });
        }

        // Book categories list load at runtime
        sharedSvc.get_bookCategorys().then(function (data) {
            $sc.book_categorys = data.result;
        });

        $sc.uploadImage = function (element) {
            console.log("I hav Changed");

            if (element.files.length > 0) {
                svc.upload_bookImage(element.files).then(function (data) {
                    //console.log('file saved sucesfully..   ' + data.result);

                    $sc.bookModel.bookInfo.bookImage = data.result[0];
                },
                function (e) {
                    console.log('in fail.. ' + e);
                });
            }
        }

        $sc.check_isBookexist = function (categoryId) {
            $sc.disableForm = true;

            var _model = {
                classId: $sc.bookModel.bookInfo.classId,
                subjectId: $sc.bookModel.bookInfo.subjectId,
                bookCategoryId: categoryId
            }

            svc.book_isExist(_model).then(function (data) {
                if (data.result === 'exist')
                    show_modal(data.entity);
                else
                    $sc.disableForm = false;
            });

        }

        function show_modal(_model) {
            var template = ' <div class="modal-header">                                                                  '
                   + ' <h4 class="box-title">Book Exist</h4>                                                       '
                   + ' <button type="button" class="close" style="margin-top: -30px !important;">                   '
                   + ' <span aria-hidden="true" ng-click="cancel()">×</span></button> </div>                        '
                   + ' <div class="modal-body" style="padding: 30px !important;">                                  '
                   + ' <div class="row"> <h4> Books already Exist, Do you want to Edit ? </h4> </div> </div>       '
                   + ' <div class="modal-footer">                                                                  '
                   + ' <button class="btn btn-info pull-left" ng-click="ok()">OK</button>     '
                   + ' <button class="btn btn-warning" ng-click="cancel(country_type)">Cancel</button> </div> '

            var modalInstance = $modal.open({
                template: template,
                controller: 'book_editModal',
                size: 'md',
                resolve: {
                    bookModal: function () {
                        return _model;
                    }
                }
            });

            modalInstance.result.then(function (entity) {     // like success fx
                $sc.bookModel = entity;
                $sc.disableForm = false;
                $sc.disableSelection = true;
            }, function () {            // like error fx
                console.log('modal-component dismissed at: ' + new Date());
            });
        }

        $sc.submitForm = function (form) {
            //alert('page is submitted.. !');
            var entity = angular.copy($sc.bookModel);       // changes in entity will not reflect to scope

            entity.bookContent = getBook_contentModel();

            var book_Id = entity.bookInfo.bookId;

            if (form.validate()) {
                if (book_Id === undefined || book_Id === '') {
                    svc.createBook(entity).then(function (data) {
                        //alert('book is created !')                            // create Book
                        $rsc.notify_fx('Book is created !', 'success');

                        $timeout(function () {
                            $state.go('book_create', {}, { reload: true });    // 2nd use if want to send params
                        }, 2000);
                    });
                } else {
                    svc.updateBook(entity).then(function (data) {        // update Book
                        $rsc.notify_fx('Book is updated !', 'success');

                        $timeout(function () {
                            $state.go('book_list');
                        }, 2000);
                    });
                }
            }
        }

        function getBook_contentModel() {
            var arr = new Array();

            var nameList = angular.element('#book_ContentDv')
                           .find('.bookContent_name');              // find all record by class Name

            var description_List = angular.element('#book_ContentDv')
                          .find('.bookContent_description');

            angular.forEach(nameList, function (elem, key) {
                var name = angular.element(elem).val();         // got element
                var description = angular.element(description_List[key]).val();           // value fetch from same cnt :)

                arr.push({ Name: name, Description: description });
            });

            return arr;
        }

        $sc.validationOptions = {
            rules: {
                bookTitle: {            // use with name attribute in control
                    required: true
                },
                bookISBN: {
                    required: true
                },
                publisher: {            // use with name attribute in control
                    required: true
                },
                bookPage: {            // use with name attribute in control
                    required: true
                },
                bookWheight: {            // use with name attribute in control
                    required: true
                },
                bookPrice: {            // use with name attribute in control
                    required: true
                },
                bookDescription: {            // use with name attribute in control
                    required: true
                },
            }
        }



        // ************* Load Book to edit :) ******************

        if ($stateParams.bookId !== undefined) {
            svc.get_bookInfo($stateParams.bookId).then(function (d) {
                $sc.bookModel = d.result;
                $sc.get_class($sc.bookModel.bookInfo.subjectId);
            });
        }


    }

    var book_editModalfn = function ($sc, $modalInstance, bookModal) {
        $sc.ok = function () {
            // close use to preform an operation or Share Data > go to success of modalInstance.result.then
            $modalInstance.close(bookModal);
        }

        $sc.cancel = function () {
            $modalInstance.dismiss('cancel');
        }
    }

    var book_listfn = function ($sc, $rsc, svc, sharedSvc, $modal, modalService) {
        $sc.bookList = [];
        $sc.book_searchModel = {};

        // contain array of list / a Modal
        $sc.classList = [];
        $sc.subjectList = [];
        $sc.book_categorys = [];

        // HERE in all services we r not using error()
        // class list load at runtime
        sharedSvc.getAll_subject().then(function (data) {
            $sc.subjectList = data.result;
        });

        // get subjects
        $sc.get_class = function (subjectId) {
            sharedSvc.get_class_bySubjctId(subjectId).then(function (d) {
                $sc.classList = d.result;
                $sc.book_searchModel.classId = '';      // set -- select option --
            }, function () {
                console.log("error occured");
            });
        }

        // Book categories list load at runtime
        sharedSvc.get_bookCategorys().then(function (data) {
            $sc.book_categorys = data.result;
        });

        $sc.searchBook = function () {
            $sc.book_searchModel.bookCategoryId = getCategoriesId($sc.book_searchModel.bookCategoryId);

            svc.get_books($sc.book_searchModel).then(function (data) {
                $sc.bookList = data.result;
            });
        }

        $sc.Show_deleteModal = function (id) {
            var bookId = id;    // to preserve value 

            modalService.deleteModal('Book').then(function () {
                //on ok button press 
                //console.log('perform Action ' + bookId);

                svc.deleteBook(bookId).then(function (data) {
                    $rsc.notify_fx('Book is deleted !', 'danger');
                    $sc.searchBook();
                });

            }, function () {
                //on cancel button press >  means dissmiss
                console.log("Modal Closed");
            });
        }

        $sc.changeCallback = function (entity) {
            svc.setBook_outOfStock(entity.bookId, entity.inStock).then(function (data) {
                $rsc.notify_fx('Status is changed !', 'info');
            });
        }

    }

    //************************   Book Discount Bundle  *******************************************

    var bundle_createfn = function ($sc, $rsc, svc, $stateParams, $state, $timeout) {
        var that = this;

        that.book_bundle = {
            booksId: [],
            totalPrice: 0
        };
        that.Isbook_selected = [];

        that.callfun = function (book, isChecked) {
            var selectedBook = that.book_bundle.booksId;
            var totalAmt = that.book_bundle.totalPrice;

            if (isChecked) {
                selectedBook.push(book.bookId);
                totalAmt += book.bookPrice;
            }
            else {
                var index = selectedBook.indexOf(book.bookId);
                selectedBook.splice(index, 1);
                totalAmt -= book.bookPrice;
            }

            that.book_bundle.booksId = selectedBook;
            that.book_bundle.totalPrice = totalAmt;
        }

        $sc.$on('onSelect_Class', function ($event, classId) {
            that.book_bundle.ClassId = classId;
        });

        that.submit_data = function (form) {
            if (form.validate()) {

                if (that.book_bundle.booksId != undefined && that.book_bundle.booksId.length >= 2) {

                    var bundleInfo = angular.copy(that.book_bundle);
                    var bundle_id = bundleInfo.BundleId;

                    if (bundle_id === undefined || bundle_id === '') {   // create bundle
                        svc.createBundle(bundleInfo).then(function (data) {
                            $rsc.notify_fx('Discount bundle created succesfully !', 'success');

                            $timeout(function () {
                                $state.go('bundle_list');
                            }, 2000);
                        });
                    }
                    else {           // udpate bundle
                        svc.updateBundle(bundleInfo).then(function (data) {
                            $rsc.notify_fx('Discount bundle created succesfully !', 'success');

                            $timeout(function () {
                                $state.go('bundle_list');
                            }, 2000);
                        });
                    }
                }
                else
                    $rsc.notify_fx('Please select atleast 2 books !', 'error');
            }
        }

        that.validationOptions = {
            rules: {
                bundle_name: {            // use with name attribute in control
                    required: true
                },
                bundle_description: {
                    required: true
                },
                bundle_price: {            // use with name attribute in control
                    required: true
                }
            }
        }

        // ************* Load Bundles to edit :) ******************
        that.is_Edit = false;

        if ($stateParams.bundleId !== undefined) {
            svc.get_Bundle_byId($stateParams.bundleId).then(function (d) {
                that.book_bundle = d.result.bundleInfo;

                that.bookInfo = d.result.bookInfo;

                // to pass value to child ctrolers
                $sc.$broadcast('onGet_Class', that.book_bundle.ClassId);


                // to show by default checked to book list
                that.book_bundle.booksId = that.bookInfo.map(function (book) {

                    // bcoz we r setting array index value by our own >
                    // So if we assing Isbook_selected[10] = "xyz" then 0 to 9 index value would be NULL
                    that.Isbook_selected[book.bookId] = true;
                    return book.bookId;         // return array after loop through items
                });

                that.is_Edit = true;
            });
        }

        that.searchBtn_isCliked = false;
        that.changeStatus = function () {
            that.searchBtn_isCliked = true;
        }
    }

    var bundle_listfn = function ($sc, $rsc, svc) {
        $sc.bundles_List = {};

        svc.get_Bundles().then(function (data) {
            $sc.bundles_List = data.result;
        });

        $sc.changeCallback = function (entity) {
            svc.deleteBundle(entity).then(function (data) {
                $rsc.notify_fx('Status is changed !', 'info');
            });
        }

    }

    var bundle_bookSearchfn = function ($sc, svc, sharedSvc) {

        $sc.bookList = [];
        $sc.book_searchModel = {};

        $sc.classList = [];
        $sc.subjectList = [];
        $sc.book_categorys = [];


        // Book categories list load at runtime
        sharedSvc.get_bookCategorys().then(function (data) {
            $sc.book_categorys = data.result;
        });

        sharedSvc.getAll_class().then(function (data) {
            $sc.classList = data.result;

            // set default class to 1
            $sc.book_searchModel.selected_class = $sc.classList[0].Id;

            // default load subjet of class 1
            $sc.get_subjects(data.result[0].Id);

        });

        $sc.$on('onGet_Class', function ($event, classId) {
            $sc.book_searchModel.selected_class = classId;

            // default load subjet of class 1
            $sc.get_subjects(classId);
        });

        $sc.get_subjects = function (classId) {

            // to pass value to parent ctrolers
            $sc.$emit('onSelect_Class', classId);

            sharedSvc.get_subject_byClassId(classId).then(function (d) {
                $sc.subjectList = d.result;
            }, function () {
                console.log("error occured");
            });
        }

        $sc.searchBook = function () {
            $sc.book_searchModel.bookCategoryId = getCategoriesId($sc.book_searchModel.bookCategoryId);

            svc.get_books($sc.book_searchModel).then(function (data) {
                $sc.bookList = data.result;
            });
        }

    }


    angular.module('Silverzone_app')
        .controller('book_create', ['$scope', '$rootScope', 'admin_book_Service', 'sharedService', '$uibModal', '$stateParams', '$state', '$timeout', book_createfn])
        .controller('book_editModal', ['$scope', '$uibModalInstance', 'bookModal', book_editModalfn])
        .controller('book_list', ['$scope', '$rootScope', 'admin_book_Service', 'sharedService', '$uibModal', 'modalService', book_listfn])
        .controller('bundle_create', ['$scope', '$rootScope', 'admin_book_Service', '$stateParams', '$state', '$timeout', bundle_createfn])
        .controller('bundle_list', ['$scope', '$rootScope', 'admin_book_Service', bundle_listfn])
        .controller('bundle_bookSearch', ['$scope', 'admin_book_Service', 'sharedService', bundle_bookSearchfn])

    ;

})();

