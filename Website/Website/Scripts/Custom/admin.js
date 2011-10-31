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
                var a = $('a[href$="#PanelBar-2"]');
                if (a.parent().hasClass('t-state-default'))
                    a.click()
            }
        });

    }

    function onMatchRowSelect(e) {
        id = e.row.cells[1].innerHTML;
        $.ajax({
            url: 'http://localhost/Kallivayalil/Admin/SelectMatch?id='+id,
            type: 'POST',
            dataType: 'json',
            error: function (xhr, status) {
                alert(status);
            },
            accept: "application/json",
            success: function (result) {
                
            }
        });
    }

    function onDataBinding(e) 
    {
        e.data = $.extend(e.data, { id: id });
    }

    function RegisterNew()
     {
        $.ajax({
            url: 'http://localhost/Kallivayalil/Admin/RegisterNew',
            type: 'POST',
            dataType: 'json',
            accept: "application/json",
            data: {isAdmin:true},
            success: function (result) {
            }
        });

    }

    function RegisterAndLink()
     {
        $.ajax({
            url: 'http://localhost/Kallivayalil/Admin/RegisterAndLink',
            type: 'POST',
            dataType: 'json',
            accept: "application/json",
            data: { isAdmin: true },
            success: function (result) {
            }
        });
       
    }

   
