'use strict';
app.controller('AcceptJobCtrl', ['$scope', '$state', '$ionicPopup', '$cordovaKeyboard', '$cordovaBarcodeScanner', 'ACCEPTJOB_ORM', 'ApiService',
  function($scope, $state, $ionicPopup, $cordovaKeyboard, $cordovaBarcodeScanner, ACCEPTJOB_ORM, ApiService) {
    var alertPopup = null,
      dataResults = new Array();
    $scope.Search = {
      BookingNo: ''
    };
    /*
    $scope.jobs = [
        {
            action : 'Collect',
            amt : '2 PKG',
            time : '09:00 - 12:00',
            code : 'PC 601234',
            customer : {
                name : 'John Tan',
                address : '150 Jurong East...'
            }
        },
        {
            action : 'Deliver',
            amt : '1 PKG',
            time : '11:00 - 13:00',
            code : 'PC 603234',
            customer : {
                name : 'John Tan',
                address : '32 Jurong East...'
            }
        },
        {
            action : 'Collect',
            amt : '1 PKG',
            time : '12:30 - 15:00',
            code : 'PC 605061',
            customer : {
                name : 'Mary Lim',
                address : '50 Jurong East...'
            }
        },
        {
            action : 'Collect',
            amt : '1 PKG',
            time : '14:00 - 16:00',
            code : 'PC 643456',
            customer : {
                name : 'John Tan',
                address : '165 Jurong North...'
            }
        }
    ];
    */
    var showPopup = function(title, type) {
      if (alertPopup === null) {
        alertPopup = $ionicPopup.alert({
          title: title,
          okType: 'button-' + type
        });
      } else {
        alertPopup.close();
        alertPopup = null;
      }
    };
    var showList = function() {
      if (is.not.empty(ACCEPTJOB_ORM.LIST.Csbk1s)) {
        dataResults = dataResults.concat(ACCEPTJOB_ORM.LIST.Csbk1s);
        $scope.jobs = dataResults;
      }
    };
    var showCsbk = function(bookingNo) {
      if (is.not.empty(bookingNo)) {
        var strUri = '/api/tms/csbk1?BookingNo=' + bookingNo;
        ApiService.GetParam(strUri, true).then(function success(result) {
          var results = result.data.results;
          if (is.not.empty(results)) {
            var reuturnTime = '';
            if (is.equal(results[0].CollectionTimeStart, '') && is.equal(results[0].CollectionTimeEnd, '')) {
              reuturnTime = '';
            } else {
              reuturnTime = results[0].CollectionTimeStart + '-' + results[0].CollectionTimeEnd;
            }
            var DLVReturntime='';
            if (is.equal(results[0].CollectionTimeStart, '') && is.equal(results[0].CollectionTimeEnd, '')) {
              DLVReturntime = '';
            } else {
              DLVReturntime = results[0].TimeFrom + '-' + results[0].TimeTo;
            }
            console.log(reuturnTime);
            var Csbk1 = {
              action: is.equal(results[0].StatusCode, 'DLV') ? 'Deliver' : 'Collect',
              amt: results[0].Pcs + ' PKG',
              //  time: is.equal(results[ 0 ].Pcs.JobType, 'DLV') ? '11:00 - 18:00' : checkDatetime(results[ 0 ].CollectionTimeStart)-checkDatetime(results[ 0 ].CollectionTimeEnd),
              time: is.equal(results[0].StatusCode, 'DLV') ? DLVReturntime : reuturnTime,
              code: results[0].PostalCode,
              customer: {
                name: results[0].BusinessPartyName,
                address: results[0].Address1 + results[0].Address2 + results[0].Address3 + results[0].Address4
              }
            };
            for (var i = 0; i < results.length; i++) {
              db_add_Csbk1_Accept(results[i]);
            }
            dataResults = dataResults.concat(Csbk1);
            $scope.jobs = dataResults;
            ACCEPTJOB_ORM.LIST._setCsbk($scope.jobs);
          }
          $scope.Search.BookingNo = '';
          $('#div-list').focus();
        });
      } else {
        showPopup('Wrong Booking No', 'assertive');
      }
    };
    $scope.returnMain = function() {
      $state.go('index.main', {}, {
        reload: true
      });
    };
    $scope.save = function() {
      if (is.not.empty($scope.jobs)) {
        $state.go('jobListingList', {}, {});
      } else {
        showPopup('No Job Accepted', 'calm');
      }
    };
    $scope.clear = function() {
      dataResults = new Array();
      $scope.jobs = dataResults;
      ACCEPTJOB_ORM.LIST._setCsbk($scope.jobs);
      $scope.Search.BookingNo = '';
    };
    $scope.openCam = function() {
      $cordovaBarcodeScanner.scan().then(function(imageData) {
        $scope.Search.BookingNo = imageData.text;
        showCsbk($scope.Search.BookingNo);
      }, function(error) {
        $cordovaToast.showShortBottom(error);
      });
    };
    $scope.clearInput = function() {
      if (is.not.empty($scope.Search.BookingNo)) {
        $scope.Search.BookingNo = '';
        $('#txt-bookingno').select();
      }
    };
    $('#txt-bookingno').on('keydown', function(e) {
      if (e.which === 9 || e.which === 13) {
        if (window.cordova) {
          $cordovaKeyboard.close();
        }
        if (alertPopup === null) {
          showCsbk($scope.Search.BookingNo);
        } else {
          alertPopup.close();
          alertPopup = null;
        }
      }
    });
    showList();
  }
]);
