(function ($) {

    $('#riskTypeId').focus();

    $('#riskTypeId').attr("disabled", "disabled");
    $('#organizationUnitId').attr("disabled", "disabled");
    //$('#statusId').attr("disabled", "disabled");
    //$('#riskRatingId').attr("disabled", "disabled");
    $('#userId').attr("disabled", "disabled");
    $('#Risk_Summary').attr("disabled", "disabled");
    $('#Risk_ExistingControl').attr("disabled", "disabled");
    $('#Risk_ERMRecommendation').attr("disabled", "disabled");
    $('#Risk_ActionPlan').attr("disabled", "disabled");
    $('#Risk_RiskOwnerComment').attr("disabled", "disabled");
    $('#Risk_TargetDate').attr("disabled", "disabled");
    $('#Risk_ActualClosureDate').attr("disabled", "disabled");
    $('#Risk_AcceptanceDate').attr("disabled", "disabled");
    $('#Risk_RiskAccepted').prop("disabled", "disabled");

    app.modals.ErmUpgradeDowngradeRisk = function () {
        var _risksService = abp.services.app.risks;

        var _modalManager;
        var _$riskInformationForm = null;

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


            var modal = _modalManager.getModal();

            var riskId = modal.find("#riskId").val();
            var _date = new Date();
            var transactionType = "Upgrade/Downgrade";
            var currentValue = modal.find("#currentOwner").val();

            var riskTransferData = {
                'RiskId': riskId,
                'Date': _date,
                'TransactionType': transactionType,
                'CurrentValue': currentValue,
                'NewValue': modal.find('#organizationUnitId option:selected').val(),
                'UserId': modal.find("#riskTransferrer").val()
            };

            //var url = $("#RiskTransferForm").attr('action');
            var url = modal.find('form[name=RiskInformationsForm]').attr('action');

            modal.find('option:not(:selected)').prop('disabled', true);


            var risk = _$riskInformationForm.serializeFormToObject();

            _modalManager.setBusy(true);
            _risksService
                .createOrEdit(risk)
                .done(function () {

                    abp.ajax({
                        url: url,
                        data: JSON.stringify(riskTransferData),
                        abpHandleError: false
                    }).done(function (data) {
                        console.log(data)
                        abp.notify.success('Risk Upgrade/Downgrade Was Successful!');
                        //$(".modal").modal('hide');
                    }).fail(function (error) {
                        //setCaptchaToken();
                        console.log(error)
                        abp.ajax.showError(error);
                    });
                    //abp.notify.info(app.localize('SavedSuccessfully'));
                    _modalManager.close();
                    abp.event.trigger('app.createOrEditRiskModalSaved');
                })
                .always(function () {
                    _modalManager.setBusy(false);
                });
        };
    };
})(jQuery);
