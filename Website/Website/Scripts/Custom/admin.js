    var id;
    function onRowSelected(e) {
        var grid = $('#MatchesGrid').data('tGrid');
        id = e.row.cells[0].innerHTML;

        $.ajax({
            url: 'http://localhost/Kallivayalil/Admin/Matches?id='+id,
            type: 'POST',
            dataType: 'json',
            error: function (xhr, status) {
                alert(status);
            },
            accept: "application/json",
            success: function (result) {
                grid.dataBind(result);
            }
        });

    }

    function onDataBinding(e) 
    {
        e.data = $.extend(e.data, { id: id });
    }
