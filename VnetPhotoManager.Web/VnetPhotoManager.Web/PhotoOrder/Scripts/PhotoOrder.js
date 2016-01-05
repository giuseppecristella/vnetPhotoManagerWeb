var jcrop_api;
$(document).ready(function () {

    // Configuro Dropzone
    Dropzone.autoDiscover = false;
    $("#dZUpload").dropzone({
        url: "../PhotoUploader.ashx",
        addRemoveLinks: true,
        removedfile: function (file) {
            $("#pnlCrop").css("display", 'none');
            var _ref;
            return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
        },
        success: function (file, response) {
            var imgName = response;
            file.previewElement.classList.add("dz-success");
            console.log("Successfully uploaded :" + imgName);
            jcrop_api.setImage("../PhotoOrder/Images/" + imgName);
            $("#pnlCrop").css("display","block");
        },
        error: function (file, response) {
            file.previewElement.classList.add("dz-error");
        }
    });

   $("#imgCropped").Jcrop({
        onSelect: storeCoords
    }, function () {
        jcrop_api = this;
    });

    function storeCoords(c) {
        $("#X").val(c.x);
        $("#Y").val(c.y);
        $("#W").val(c.w);
        $("#H").val(c.h);
    };

});