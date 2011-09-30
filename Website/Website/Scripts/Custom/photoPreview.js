  var x1 =0, x2=115, y1=0, y2=115;
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


            var jcrop_api, boundx, boundy;

            $('#imgThumbnail2').Jcrop({
                onChange: updatePreview,
                onSelect: updatePreview,
                minSize: [x2, y2],
                maxSize:[x2,y2],
                aspectRatio: 1
            }, function () {
                // Use the API to get the real image size
                var bounds = this.getBounds();
                boundx = bounds[0];
                boundy = bounds[1];
                // Store the API in the jcrop_api variable
                jcrop_api = this;
            });
            function updatePreview(c) {
                if (parseInt(c.w) > 0) {

                    x1 = c.x;
                    y1 = c.y;
                    x2 = c.x2;
                    y2 = c.y2;

                    $('#x1').val(x1);
                    var rx = 100 / c.w;
                    var ry = 100 / c.h;
                    $('#preview').css({
                        width: Math.round(rx * boundx) + 'px',
                        height: Math.round(ry * boundy) + 'px',
                        marginLeft: '-' + Math.round(rx * c.x) + 'px',
                        marginTop: '-' + Math.round(ry * c.y) + 'px'
                    });
                }
            };


            if (document.all || is_chrome)//IE
                imageLoadParent.appendChild(myform.firstChild);
            else//FF                     
                document.body.removeChild(myform);

        }
        });

    };


function UploadSubmit(fileId, imageId) {
              var myform = document.createElement("form");
        myform.style.display = "none";
        myform.action = "http://localhost/kallivayalil/ImagePreview/UploadSubmit";
        myform.enctype = "multipart/form-data";
        myform.method = "post";
        myform.dataType = "json";
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
    };


