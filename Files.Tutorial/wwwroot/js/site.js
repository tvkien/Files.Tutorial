//(function ($) {
//    "use strict";

//    var bar = $('.bar');
//    var percent = $('.percent');
//    var status = $('#status');

//    $(".submit-form").on('click', function (e) {

//        e.preventDefault();
//        //if ($(':focus').is('[formaction]')) {
//        //    console.warn($(':focus').attr('formaction'));
//        //}
//        //var actionUrl = form.attr('action');
//        //var actionUrl = $(':focus').attr('formaction');
//        var actionUrl = $(this).attr('formaction');

//        var dataToSend = $("#form-upload").serialize();

//        $.ajax({
//            xhr: function () {
//                var xhr = new window.XMLHttpRequest();

//                xhr.upload.addEventListener("progress", function (evt) {
//                    if (evt.lengthComputable) {
//                        var percentComplete = evt.loaded / evt.total;
//                        percentComplete = parseInt(percentComplete * 100);
//                        console.log(percentComplete);

//                        if (percentComplete === 100) {

//                        }

//                    }
//                }, false);

//                return xhr;
//            },
//            url: actionUrl,
//            type: "POST",
//            data: JSON.stringify(dataToSend),
//            contentType: "application/json",
//            dataType: "json",
//            success: function (result) {
//                console.log(result);
//            }
//        });
//    });    
//})(jQuery);