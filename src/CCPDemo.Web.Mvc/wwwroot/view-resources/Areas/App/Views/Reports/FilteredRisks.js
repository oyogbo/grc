(function () {



    $(function () {
        $("#RisksTable1").dataTable({
            "searching": true,
            autoWidth: false,
            paging: true,
            serverSide: true,
            dom: 'Bfrtip',
            buttons: [
                'excel', 'pdf', 'print'
            ]
        });

        var table = $('#RisksTable1').DataTable();

        var risktypeIndex = 0;
        $("#RisksTable1 th").each(function (i) {
            if ($($(this)).html() == "Risk Type") {
                risktypeIndex = i; return false;
            }
        });
        var orgUnitIndex = 0;
        $("#RisksTable1 th").each(function (i) {
            if ($($(this)).html() == "Organization Unit") {
                orgUnitIndex = i; return false;
            }
        });

        var riskRatingIndex = 0;
        $("#RisksTable1 th").each(function (i) {
            if ($($(this)).html() == "Rating") {
                riskRatingIndex = i; return false;
            }
        });

        $.fn.dataTable.ext.search.push(
            function (settings, data, dataIndex) {
                var selectedItem = $('#RiskTypeNameFilterId option:selected').text()
                var riskType = data[risktypeIndex];
                if (selectedItem === "" || riskType.includes(selectedItem)) {
                    return true;
                }

                var selectedItem = $('#OrganizationUnitDisplayNameFilterId option:selected').text()
                var orgUnit = data[orgUnitIndex];
                if (selectedItem === "" || orgUnit.includes(selectedItem)) {
                    return true;
                }

                var selectedItem = $('#RiskRatingNameFilterId option:selected').text()
                var riskRating = data[riskRatingIndex];
                if (selectedItem === "" || riskRating.includes(selectedItem)) {
                    return true;
                }

                return false;
            }
        );

        $("#RisksTable1").change(function (e) {
            table.draw();
        });

        table.draw();


        $('td:first-child').each(function () {
            var span = $(this).attr('colspan');
            if (parseInt(span) > 1) {
                $(this).attr('colspan', 1);
                table.draw();
            }
        });

        //$('#RisksTable1 td').resizable({
        //    handles: 'e',
        //    stop: function (e, ui) {
        //        $(this).width(ui.size.width);
        //    }

        //});

        $('#RiskTypeNameFilterId').change(function (e) {
            table.draw();
        });
        $('#RiskRatingNameFilterId').change(function (e) {
            table.draw();
        });
        $('#OrganizationUnitDisplayNameFilterId').change(function (e) {
            table.draw();
        });

        table.columns.adjust().draw();
    });
})();
