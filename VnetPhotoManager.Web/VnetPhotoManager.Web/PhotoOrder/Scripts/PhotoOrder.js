var jcrop_api;
$(document).ready(function () {

    // Configuro Dropzone
    var dropZone = new Dropzone("div#dZUpload", {
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
            if (jcrop_api != null) {
                jcrop_api.destroy();
            }

            jcrop_api = $.Jcrop('#imgCropped', { setSelect: [1, 1, 1024, 576], onSelect: storeCoords });
            jcrop_api.setImage("../PhotoOrder/Images/" + imgName.replace(".jpg", "_resized.jpg"), function () {
                jcrop_api.setOptions({
                    setSelect: [1, 1, 1024, 576],
                    onSelect: storeCoords
                });
            });
            //jcrop_api.setSelect= [60, 70, 540, 330];
            $("#pnlCrop").css("display", "block");

            //$("#imgCropped").css({
            //    width: 500,
            //    overflow: 'hidden'
            //});
            //jcrop_api = $.Jcrop('#imgCropped', { setSelect: [60, 70, 540, 330] });
        },
        error: function (file, response) {
            file.previewElement.classList.add("dz-error");
        }
    });


    //Dropzone.autoDiscover = false;
    //$("#dZUpload").dropzone({
    //    url: "../PhotoUploader.ashx",
    //    addRemoveLinks: true,
    //    removedfile: function (file) {
    //        $("#pnlCrop").css("display", 'none');
    //        var _ref;
    //        return (_ref = file.previewElement) != null ? _ref.parentNode.removeChild(file.previewElement) : void 0;
    //    },
    //    success: function (file, response) {
    //        var imgName = response;
    //        file.previewElement.classList.add("dz-success");
    //        console.log("Successfully uploaded :" + imgName);
    //        jcrop_api.setImage("../PhotoOrder/Images/" + imgName);
    //        $("#pnlCrop").css("display","block");
    //    },
    //    error: function (file, response) {
    //        file.previewElement.classList.add("dz-error");
    //    }
    //});

    //$("#imgCropped").Jcrop({
    //    onSelect: storeCoords
    //}, function () {
    //    jcrop_api = this;
    //});

    //$('#target1').Jcrop({
    //    setSelect: [60, 70, 540, 330]
    //}, function () {
    //    jcrop_api = this;
    //});

    function storeCoords(c) {
        $("#X").val(c.x);
        $("#Y").val(c.y);
        $("#W").val(c.w);
        $("#H").val(c.h);
    };

    $("#divAddAndCrop").click(function () {
        if (dropZone.getAcceptedFiles().length > 0) {
            $("#addPhotoModal").modal('show');
        }
    });

    $("#divAddSavedPhoto").click(function () {
        $("#addSavedPhotoModal").modal('show');
    });

    var openAlreadyExistModal = function () {
        $("#addPhotoModal").modal('show');
    }

});