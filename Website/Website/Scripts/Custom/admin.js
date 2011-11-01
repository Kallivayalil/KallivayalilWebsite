    var id;
    function onRowSelected(e) {
        var grid = $('#MatchesGrid').data('tGrid');
        id = e.row.cells[0].innerHTML;

        $.ajax({
            url: 'http://localhost/Kallivayalil/Admin/Matches?constId=' + id,
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
                    a.click();

                /*var b = $('a[href$="/Kallivayalil/Admin/Confirm"]');
                if (b.parent().hasClass('t-state-default'))
                    b.click();*/
            }
        });

    }

    function onMatchRowSelect(e) {
        id = e.row.cells[1].innerHTML;
        $.ajax({
            url: 'http://localhost/Kallivayalil/Admin/SelectMatch?constId=' + id,
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
            data: {isAdmin: $('#makeAdmin').is(':checked') },
            success: function (result) {
                alert('User Registration has been approved.');
                window.location.href = "Registrations";
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
            data: { isAdmin: $('#makeAdmin').is(':checked') },
            success: function (result) {
                alert('User Registration has been approved and linked with the existing user.');
                window.location.href = "Registrations";
            }
        });

    }
    
    function Reject() {
        var the_reason = window.prompt("What is the reason name?", "");

        if (the_reason != null && $.trim(the_reason).length > 0) {

            $.ajax({
                url: 'http://localhost/Kallivayalil/Admin/Reject',
                type: 'POST',
                dataType: 'json',
                accept: "application/json",
                data: { Reason: the_reason},
                success: function (result) {
                    alert('User registration is rejected.');
                    window.location.href = "Registrations";
                }
            });
        }
       
    }

   
