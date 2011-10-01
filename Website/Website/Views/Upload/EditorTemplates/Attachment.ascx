<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script type="text/javascript">
function onUpload(e) {
    var grid = $(this).closest(".t-grid").data("tGrid");
    var tr = $(this).closest("tr");
    var dataItem = grid.dataItem(tr);

    if (dataItem) {
        e.data = { id: dataItem.ID };
    }
}

function onRemove(e) {
    var grid = $(this).closest(".t-grid").data("tGrid");
    var tr = $(this).closest("tr");
    var dataItem = grid.dataItem(tr);

    if (dataItem) {
        e.data = { id: dataItem.ID };
    }
}

var lastUploadedFile;
function onSuccess(e) {
    var fileName = e.response.Name;
    
    var grid = $(this).closest(".t-grid").data("tGrid");
    var tr = $(this).closest("tr");
    var dataItem = grid.dataItem(tr);

    if (dataItem) {
        grid.dataItem(tr).Name = fileName;
    }

    lastUploadedFile = fileName;
}
</script>



<%= Html.Telerik().Upload()
      .Name("attachment")
      .Multiple(false)
      .Async(async => async
            .Save("Save", "Upload")
            .Remove("Remove", "Upload")
            .AutoUpload(true))
      .ClientEvents(e => e.OnUpload("onUpload")
                          .OnRemove("onRemove")
                          .OnSuccess("onSuccess")
                          )
%>
