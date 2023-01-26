(function () {


    $(function () {
        var _$risksTable = $('#RisksTypeTable');
        var _risksService = abp.services.app.risks;


        _risksService.getRiskTypeByDepartment()
            .done(function (data) {

                console.log(data);
            });

        var dataTable = _$risksTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            searching: true,
            dom: 'Bfrtip',
            buttons: [
                'excel', 'pdf', 'print'
            ],
            listAction: {
                ajaxFunction: _risksService.getRiskTypeByDepartment
            },
            columnDefs: [
                {
                    targets: 0,
                    "data": null, "render": function (data, type, full, meta) {
                        return meta.row + 1;
                    }
                }, 
                {
                    targets: 1,
                    data: 'organizationUnit'
                },
                {
                    targets: 2,
                    data: 'riskType'
                },
                {
                    targets: 3,
                    data: 'riskCount'
                },
                
            ],
        });

        dataTable.on('order.dt search.dt', function () {
            dataTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();

        function getRisks() {
            dataTable.ajax.reload();
        }

       
        $('#GetRisksButton').click(function (e) {
            e.preventDefault();
            getRisks();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getRisks();
            }
        });
    });
})();
