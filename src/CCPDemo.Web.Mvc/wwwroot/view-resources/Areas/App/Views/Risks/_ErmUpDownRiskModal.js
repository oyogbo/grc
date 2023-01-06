$.noConflict;
var $ = jQuery;

$(function () {
    $("#UpgradeDowngradeBtnSave").click(function () {

        console.log('Upgrading/Downgrading Risk');

        var riskId = $("riskId").val();
        var _date = new Date().toLocaleString();
        var transactionType = "Downgrade/Upgrade";
        var currentValue = $("#currentOwner").val();

        var newValue;

        $("#Risk_Rating").change(function () {
            newValue = $(this).val();
        });

        var riskTransferData = {
            'RiskId': $("#riskId").val(),
            'Date': new Date().toLocaleString(),
            'TransactionType': transactionType,
            'CurrentValue': currentValue,
            'NewValue': newValue,
            'UserId': $("#loggedInUserId").val()
        };
        var baseUrl = window.location.origin;
        var url = baseUrl + "Risks/UpDownGradeRisk";

        abp.ajax({
            url: $("#RiskTransferForm").attr('action'),
            data: JSON.stringify(riskTransferData),
            abpHandleError: false,
        }).done(function (data) {
            console.log(data)
            abp.notify.success('Risk Upgrade/Downgrade Was Successful!');
            $(".modal").modal('hide');
        }).fail(function (error) {
            //setCaptchaToken();
            console.log(error)
            abp.ajax.showError(error);
        });


        //$.ajax({
        //    url: "App/VRisks/TransferRisk",
        //    type: "POST",
        //    data: JSON.stringify(data),
        //    dataType: "json",
        //    traditional: true,
        //    contentType: 'application/json; charset=utf-8',
        //    success: function (data) {
        //        console.log('Data Here');
        //        console.log(data)
        //    }
        //});

    });
});