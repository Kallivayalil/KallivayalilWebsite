  function ChangeImage(fileId, imageId,previewId) {
              var myform = document.createElement("form");
        myform.style.display = "none";
        myform.action = "http://localhost/kallivayalil/ImagePreview/AjaxSubmit";
        myform.enctype = "multipart/form-data";
        myform.method = "post";
        var imageLoad;
        var imageLoadParent;
        var is_chrome = /chrome/.test(navigator.userAgent.toLowerCase());
        if (is_chrome && document.getElementById(fileId).value == '')
            return; //Chrome bug onchange cancel
        if (document.all || is_chrome) {//IE
            imageLoad = document.getElementById(fileId);
            imageLoadParent = document.getElementById(fileId).parentNode;
            myform.appendChild(imageLoad);
            document.body.appendChild(myform);
        }
        else {//FF
            imageLoad = document.getElementById(fileId).cloneNode(true);
            myform.appendChild(imageLoad);
            document.body.appendChild(myform);
        }
        $(myform).ajaxSubmit({ success:
        function (responseText) {
            var d = new Date();
            $(imageId)[0].src = "http://localhost/kallivayalil/ImagePreview/ImageLoad?a=" + d.getMilliseconds();
            $(previewId)[0].src = "http://localhost/kallivayalil/ImagePreview/ImageLoad?a=" + d.getMilliseconds();
            if (document.all || is_chrome)//IE
                imageLoadParent.appendChild(myform.firstChild);
            else//FF                     
                document.body.removeChild(myform);
        }
        });
    }


function UploadSubmit(fileId, imageId) {
              var myform = document.createElement("form");
        myform.style.display = "none";
        myform.action = "http://localhost/kallivayalil/ImagePreview/UploadSubmit";
        myform.enctype = "multipart/form-data";
        myform.method = "post";
        var imageLoad;
        var imageLoadParent;
        var is_chrome = /chrome/.test(navigator.userAgent.toLowerCase());
        if (is_chrome && document.getElementById(fileId).value == '')
            return; //Chrome bug onchange cancel
        if (document.all || is_chrome) {//IE
            imageLoad = document.getElementById(fileId);
            imageLoadParent = document.getElementById(fileId).parentNode;
            myform.appendChild(imageLoad);
            document.body.appendChild(myform);
        }
        else {//FF
            imageLoad = document.getElementById(fileId).cloneNode(true);
            myform.appendChild(imageLoad);
            document.body.appendChild(myform);
        }
        $(myform).ajaxSubmit({ success:
        function (responseText) {
            var d = new Date();
            $(imageId)[0].src = "http://localhost/kallivayalil/ImagePreview/ImageLoad?a=" + d.getMilliseconds();
            if (document.all || is_chrome)//IE
                imageLoadParent.appendChild(myform.firstChild);
            else//FF                     
                document.body.removeChild(myform);
        }
        });
    }
