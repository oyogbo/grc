(function ($) {
  app.modals.CreateOrEditRiskModal = function () {
    var _risksService = abp.services.app.risks;

    var _modalManager;
      var _$riskInformationForm = null;

      $('#statusId').val(1);

      $('#riskTypeId').focus();

      $('#Risk_TargetDate').blur();

      console.log('Raise New Risk');
      var loggedInUserIsErm = false;

      _risksService
          .isERM()
          .done(function (data) {
              var isERM = data;
              loggedInUserIsErm = isERM;
              if (!isERM) {
                  $('#statusId').attr("disabled", true);
                  $('#riskTypeId').attr("disabled", true);
                  $('#riskRatingId').attr("disabled", true);
                  $('#userId').attr("disabled", true);
                  $('#organizationUnitId').attr("disabled", true);

                  $(".save-button").prop("disabled", true);

                  $("#Risk_ActionPlan, #Risk_RiskOwnerComment").keyup(function (e) {

                      var alltxt = $("#Risk_ActionPlan, #Risk_RiskOwnerComment").length;
                      var empty = true;
                      $("#Risk_ActionPlan, #Risk_RiskOwnerComment").each(function (i) {
                          if ($(this).val() == '' || $(this).val().length < 3) {
                              empty = true;
                              $(".save-button").prop("disabled", true);
                              return false;
                          }
                          else {
                              empty = false;
                          }
                      });
                      if (!empty) $(".save-button").prop("disabled", false);
                  });
              }
          });

      var riskId = $('#riskId').val();

      _risksService
          .isRiskAccepted(riskId)
          .done(function (data) {
              var isRiskAccepted = data;
              
              if (isRiskAccepted) {
                  $('#riskAccepted').attr('readonly', true);
              }
          });


      //_risksService
      //    .getUserId()
      //    .done(function (data) {
      //        $('#get_id').val(data);
      //        console.log(data)
      //    });


    var _RiskriskTypeLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/RiskTypeLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_RiskRiskTypeLookupTableModal.js',
      modalClass: 'RiskTypeLookupTableModal',
    });
    var _RiskorganizationUnitLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/OrganizationUnitLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_RiskOrganizationUnitLookupTableModal.js',
      modalClass: 'OrganizationUnitLookupTableModal',
    });
    var _RiskstatusLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/StatusLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_RiskStatusLookupTableModal.js',
      modalClass: 'StatusLookupTableModal',
    });
    var _RiskriskRatingLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/RiskRatingLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_RiskRiskRatingLookupTableModal.js',
      modalClass: 'RiskRatingLookupTableModal',
    });
    var _RiskuserLookupTableModal = new app.ModalManager({
      viewUrl: abp.appPath + 'App/Risks/UserLookupTableModal',
      scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Risks/_RiskUserLookupTableModal.js',
      modalClass: 'UserLookupTableModal',
    });

    this.init = function (modalManager) {
      _modalManager = modalManager;

      var modal = _modalManager.getModal();
        modal.find('.date-picker').flatpickr({
         defaultDate: null,
        locale: abp.localization.currentLanguage.name,
        format: 'L',
      });

      _$riskInformationForm = _modalManager.getModal().find('form[name=RiskInformationsForm]');
      _$riskInformationForm.validate();
      };


      $('#organizationUnitId').change(function () {
          var ouId = $(this).val();

          console.log("OU Id: " + ouId);

          var usersEmailIdsData = null;

          _risksService
              .usersInOrganizationalUnit(ouId)
              .done(function (data) {

                  console.log(data);

                  var users = data.items;

                  //console.log(users)

                  var usersSelect = $('#userId');
                  usersSelect.html('');

                  var usersOptions = '';

                  _risksService
                      .usersEmailsIdsDict()
                      .done(function (data) {

                          var arr = data.split(',');

                          if (users.length > 0) {
                              var userId;
                              users.forEach(row => {
                                  arr.forEach((r, i) => {
                                      //console.log(i)
                                      var innerArr = r.split(':');
                                      var email = innerArr[0].replace('{', '').replace('"', '');
                                      //console.log("SPLIT====")
                                      console.log(email)
                                      email = email.replace(/.$/, "");
                                      console.log("THE EMAIL")
                                      console.log(row.emailAddress)

                                      if (row.emailAddress.toString() === email.toString()) {
                                          console.log("EQUALS")
                                          userId = innerArr[1].toString().replace('}', '');
                                          userId = parseInt(userId);
                                          console.log("USER ID: " + userId);
                                      }
                                  });

                                  usersOptions += `<option value="${userId}">${row.name} &nbsp; ${row.surname}(${row.emailAddress})</option>`;
                              });
                          usersSelect.append(usersOptions);
                          }
                      });

              });

          console.log(ouId);
      });


    $('#OpenRiskTypeLookupTableButton').click(function () {
      var risk = _$riskInformationForm.serializeFormToObject();

      _RiskriskTypeLookupTableModal.open({ id: risk.riskTypeId, displayName: risk.riskTypeName }, function (data) {
        _$riskInformationForm.find('input[name=riskTypeName]').val(data.displayName);
        _$riskInformationForm.find('input[name=riskTypeId]').val(data.id);
      });
    });

    $('#ClearRiskTypeNameButton').click(function () {
      _$riskInformationForm.find('input[name=riskTypeName]').val('');
      _$riskInformationForm.find('input[name=riskTypeId]').val('');
    });

    $('#OpenOrganizationUnitLookupTableButton').click(function () {
      var risk = _$riskInformationForm.serializeFormToObject();

      _RiskorganizationUnitLookupTableModal.open(
        { id: risk.organizationUnitId, displayName: risk.organizationUnitDisplayName },
        function (data) {
          _$riskInformationForm.find('input[name=organizationUnitDisplayName]').val(data.displayName);
          _$riskInformationForm.find('input[name=organizationUnitId]').val(data.id);
        }
      );
    });

    $('#ClearOrganizationUnitDisplayNameButton').click(function () {
      _$riskInformationForm.find('input[name=organizationUnitDisplayName]').val('');
      _$riskInformationForm.find('input[name=organizationUnitId]').val('');
    });

    $('#OpenStatusLookupTableButton').click(function () {
      var risk = _$riskInformationForm.serializeFormToObject();

      _RiskstatusLookupTableModal.open({ id: risk.statusId, displayName: risk.statusName }, function (data) {
        _$riskInformationForm.find('input[name=statusName]').val(data.displayName);
        _$riskInformationForm.find('input[name=statusId]').val(data.id);
      });
    });

    $('#ClearStatusNameButton').click(function () {
      _$riskInformationForm.find('input[name=statusName]').val('');
      _$riskInformationForm.find('input[name=statusId]').val('');
    });

    $('#OpenRiskRatingLookupTableButton').click(function () {
      var risk = _$riskInformationForm.serializeFormToObject();

      _RiskriskRatingLookupTableModal.open(
        { id: risk.riskRatingId, displayName: risk.riskRatingName },
        function (data) {
          _$riskInformationForm.find('input[name=riskRatingName]').val(data.displayName);
          _$riskInformationForm.find('input[name=riskRatingId]').val(data.id);
        }
      );
    });

    $('#ClearRiskRatingNameButton').click(function () {
      _$riskInformationForm.find('input[name=riskRatingName]').val('');
      _$riskInformationForm.find('input[name=riskRatingId]').val('');
    });

    $('#OpenUserLookupTableButton').click(function () {
      var risk = _$riskInformationForm.serializeFormToObject();

      _RiskuserLookupTableModal.open({ id: risk.userId, displayName: risk.userName }, function (data) {
        _$riskInformationForm.find('input[name=userName]').val(data.displayName);
        _$riskInformationForm.find('input[name=userId]').val(data.id);
      });
    });

    $('#ClearUserNameButton').click(function () {
      _$riskInformationForm.find('input[name=userName]').val('');
      _$riskInformationForm.find('input[name=userId]').val('');
    });

      $('.reset-acceptance').click(function () {

          var risk = _$riskInformationForm.serializeFormToObject();

          _modalManager.setBusy(true);
          _risksService
              .resetAcceptance(risk)
              .done(function () {
                  abp.notify.info(app.localize('Acceptance Reset Was Successful!'));
                  _modalManager.close();
                  abp.event.trigger('app.createOrEditRiskModalSaved');
              })
              .always(function () {
                  _modalManager.setBusy(false);
              });
      });


      // Enable/Disable Save Button based on Risk Owner's Input
      


      //if (!loggedInUserIsErm) {
      //    $(".save-button").prop("disabled", true);
      //    if ($('#Risk_ActionPlan').val() == '') {
      //        abp.message.error(app.localize('{0}IsRequired', app.localize('ActionPlan')));
      //        return;
      //    }
      //    if ($('#Risk_RiskOwnerComment').val() == '') {
      //        abp.message.error(app.localize('{0}IsRequired', app.localize('RiskOwnerComment')));
      //        return;
      //    }
      //}

    this.save = function () {
      if (!_$riskInformationForm.valid()) {
        return;
      }
        if ($('#riskTypeId').prop('required') && $('#riskTypeId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('RiskType')));
        return;
      }
        if ($('#organizationUnitId').prop('required') && $('#organizationUnitId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('OrganizationUnit')));
        return;
      }
        if ($('#statusId').prop('required') && $('#statusId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('Status')));
        return;
      }
        if ($('#riskRatingId').prop('required') && $('#riskRatingId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('RiskRating')));
        return;
      }
        if ($('#userId').prop('required') && $('#userId').val() == '') {
        abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
        return;
      }

        if (!loggedInUserIsErm) {
            if ($('#Risk_ActionPlan').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ActionPlan')));
                return;
            }
            if ($('#Risk_RiskOwnerComment').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('RiskOwnerComment')));
                return;
            }
        }

        var risk = _$riskInformationForm.serializeFormToObject();

        console.log(risk)

        var userInfo;

        $('#userId').change(function () {
            userInfo = $(this).text();
        });

      _modalManager.setBusy(true);
      _risksService
        .createOrEdit(risk)
        .done(function () {
            abp.notify.info(app.localize('SavedSuccessfully'));
            

            _risksService
                .isERM()
                .done(function (data) {
                    var isERM = data;
                    if (isERM) {

                        var userInfo = $("#userId option:selected").text();
                        console.log('UserInfo')
                        console.log(userInfo)
                        const regExp = /\(([^)]+)\)/g;
                        const matches = userInfo.match(regExp).toString();
                        const userEmail = matches.replace('(', '').replace(')', '');
                        console.log('Email To');
                        console.log(userEmail);

                        _risksService
                            .sendRiskEmail({
                                emailAddress: userEmail,
                                subject: "A New Risk From ERM | GRC Portal",
                                body: "A new Risk has been raised your behalf. Please, login to the GRC portal for details!"
                            })
                            .done(function () {
                                abp.notify.info("Email Sent Successfully!");
                            });
                    } else {

                        var id = $('#riskId').val();

                        _risksService
                            .getERMEmail(id)
                            .done(function (data) {
                                var erMEmail = data;
                                console.log('Email From Server');
                                console.log(erMEmail)
                                _risksService
                                    .sendRiskEmail({
                                        emailAddress: erMEmail,
                                        subject: "Email from Risk Ownr",
                                        body: "A Risk Owner has responded to a risk. Please, login to the portal to confirm!"
                                    })
                                    .done(function () {
                                        abp.notify.info("Email Sent Successfully!");
                                    });
                            });
                    }
                });
          _modalManager.close();
          abp.event.trigger('app.createOrEditRiskModalSaved');
        })
        .always(function () {
          _modalManager.setBusy(false);
        });
    };
  };
})(jQuery);
