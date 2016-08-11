/*
 * 图片上传控件
 * 依赖jquery,bootstrap3.0,font-awesome.min.css,lightbox
 */

(function ($) {
    function initPicUploader(folder, btnid, uploadurl, deleteurl, inputId, max) {
        var $box = $("#box" + inputId);
        var $btn = $("#" + btnid);
        var btnInBox = $("#" + btnid, $box).length > 0;
        var curLimitfilesize = 10 * 1024 * 1024; // 1 MB = 1 * 1024 * 1024 bytes
        var lImageBoxHeight = 120, lImageBoxWidth = 120;

        var uploader = new qq.FineUploader({
            element: $btn[0],
            text: {
                uploadButton: '　'
            },
            multiple: false,
            validation: {
                allowedExtensions: ['jpeg', 'jpg', 'png'],
                sizeLimit: curLimitfilesize //5 * 1024 * 1024 // 5 MB = 5 * 1024 * 1024 bytes
            },
            request: {
                endpoint: uploadurl,
                params: {
                    "folder": folder,
                    "thumbWidth": lImageBoxWidth,
                    "thumbHeight": lImageBoxHeight,
                    "sizeLimit": curLimitfilesize
                },
                forceMultipart: true
            },
            callbacks: {
                onSubmit: function (id, fileName) {
                    this._options.request.params.sizeLimit = curLimitfilesize;
                    var html = '<div class="pp-box clsimagepre"><div class="progress"></div></div>';
                    if (btnInBox) {
                        $btn.before(html);
                        $btn.hide();
                    } else {
                        var $last = $(".pp-box:last", $box);
                        if ($last.length > 0) {
                            $last.after(html);
                        } else {
                            $box.prepend(html);
                        }
                    }
                },
                onError: function (id, fileName, reason) {
                    //alert(reason);
                },
                onProgress: function (id, fileName, loaded, total) {
                    var percent = Math.round(loaded / total * 100);
                    var phinner = '<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: ' + percent + '%;">';
                    phinner += percent + "%";
                    phinner += '</div>';
                    $(".progress", $box).html(phinner);
                },
                onComplete: function (id, fileName, responseJson) {
                    if (responseJson.success) {
                        bindImg(responseJson);
                    }
                }
            },
            messages: {
                typeError: "{file} 支持的文件扩展名: {extensions}.",
                sizeError: "{file} 文件太大, 最大支持 {sizeLimit}.",
                minSizeError: "{file} 文件太小, 最小支持 {minSizeLimit}.",
                emptyError: "{file} 文件不能为空.",
                noFilesError: "请选择文件.",
                onLeave: "正在上传文件，如果离开，上传将取消."
            },
            showMessage: function (message) {
                alert(message);
            },
            debug: true
        });

        function bindImg(responseJson) {
            $(".clsimagepre", $box).remove();
            var index = $(".pp-box", $box).length;
            if (btnInBox) { index = index - 1; }
            var inpState = inputId + "State" + index;
            var inpFileName = inputId + "FileName" + index;
            var inpFileUrl = inputId + "FileUrl" + index;
            var inpThumbName = inputId + "ThumbName" + index;
            var inpThumbUrl = inputId + "ThumbUrl" + index;
            var html = '<div class="pp-box">';
            html += '<div class="pp-img">';
            html += '<a href="' + responseJson.fileurl + '" data-lightbox="roadtrip">';
            html += '<img src="' + responseJson.thumburl + '"></a></div>';
            html += '<div class="btn-del">';
            html += "<a href='javascript:;' class='del'><i class='fa fa-minus-circle fa-lg'></i>";
            html += '</a>';
            html += '</div>';
            html += '<input type="hidden" class="state" value="1" name="' + inpState + '"/>';
            html += '<input type="hidden" class="filename" value="' + responseJson.filename + '" name="' + inpFileName + '"/>';
            html += '<input type="hidden" class="fileurl" value="' + responseJson.fileurl + '" name="' + inpFileUrl + '"/>';
            html += '<input type="hidden" class="thumbname" value="' + responseJson.thumbname + '" name="' + inpThumbName + '"/>';
            html += '<input type="hidden" class="thumburl" value="' + responseJson.thumburl + '" name="' + inpThumbUrl + '"/>';
            html += '</div>';
            if (btnInBox) {
                $btn.before(html);
                if (index >= max) {
                    $btn.hide();
                } else {
                    $btn.show();
                }
            } else {
                var $last = $(".pp-box:last", $box);
                if ($last.length > 0) {
                    $last.after(html);
                } else {
                    $box.prepend(html);
                }
            }

            valid();
        }

        function deletePic(obj) {
            var thispic = $(obj).parents("div.pp-box");
            var state = thispic.find("input[class='state']").val();
            var filename = thispic.find("input[class='filename']").val();
            if (state == '0') {//非新上传文件
                var len = $("input[class='delete_file']", $box).length;
                var inpDelete = inputId + "Delete" + len;
                $box.append('<input type="hidden" class="filename" value="' + filename + '" name="' + inpDelete + '"/>');
            }
            thispic.remove();
            buildPicInputName();
            if (btnInBox) {
                $btn.show();
            }
            valid();
            $.post(deleteurl, { 'folder': folder, 'filename': filename }, function (result) { }, "json");
        }

        function buildPicInputName() {
            $("div.pp-box", $box).each(function (i, o) {
                $(this).find("input[class='state']").attr("name", inputId + "State" + i);
                $(this).find("input[class='filename']").attr("name", inputId + "FileName" + i);
                $(this).find("input[class='fileurl']").attr("name", inputId + "FileUrl" + i);
                $(this).find("input[class='thumbname']").attr("name", inputId + "ThumbName" + i);
                $(this).find("input[class='thumburl']").attr("name", inputId + "ThumbUrl" + i);
            });
        }

        function valid() {
            if ($.validator && $.validator.unobtrusive) {
                $box.parents("form").validate().element("#" + inputId);
            }
        }

        //删除事件
        $box.on("click", "a.del", function (evt) {
            deletePic(this);
        });
    }

    $.picupload = function (uploadurl, deleteurl, inputId, btnid, max) {
        var folder = $('input[name=' + inputId + ']').val();
        initPicUploader(folder, btnid, uploadurl, deleteurl, inputId, max);
    }

    if ($.validator && $.validator.unobtrusive) {
        /*上传控件必填验证*/
        /*上传控件至少要上传几个文件*/
        $.validator.addMethod("puRequired", function (value, element, param) {
            var elId = "box" + element.id;
            if ($("#" + elId + " .pp-img").length < parseInt($(element).attr("MinFilesCount"), 10)) return false;
            return true;
        });
        $.validator.unobtrusive.adapters.addBool("puRequired");

        /*上传控件最多上传几个文件*/
        $.validator.addMethod("puMaxFilesCount", function (value, element, param) {
            var elId = "box" + element.id;
            if ($("#" + elId + " .pp-img").length > parseInt($(element).attr("MaxFilesCount"), 10)) return false;
            return true;
        });
        $.validator.unobtrusive.adapters.addBool("puMaxFilesCount");
    }
}(jQuery));