/*上传控件*/

function EFInitUploader(btnid, uploadHandler, deleteHandler, fileSizeLimit, totalSizeLimit, btnTxt, cfg) {
    var folder = document.getElementById(btnid).value;
    var uploader = new qq.FineUploader({
        folder: folder,
        deleteHandler: deleteHandler,
        element: document.getElementById(btnid + "-upload"),
        text: {
            uploadButton: btnTxt ? btnTxt : "添加文件", //'　''添加文件'
            cancelButton: ''//删除
        },
        multiple: true,
        disableCancelForFormUploads: true,
        validation: {
            allowedExtensions: (cfg && cfg.include) ? cfg.include : [], //'jpeg', 'jpg', 'gif', 'png', 'doc', 'docx'
            forbidExtensions: (cfg && cfg.exclude) ? cfg.exclude : [],
            sizeLimit: fileSizeLimit, //5 * 1024 * 1024 // 5 MB = 5 * 1024 * 1024 bytes
            totalSizeLimit: totalSizeLimit,
            regex: (cfg) ? cfg.regex : ""
        },
        dragAndDrop: {
            disableDefaultDropzone: true
        },
        request: {
            endpoint: uploadHandler,
            params: {
                "folder": folder,
                "sizeLimit": fileSizeLimit
            },
            forceMultipart: true
        },
        showMessage: function () { },
        callbacks: {
            onValidate: function (fileData) {
                var maxCount = -1;
                if ($("#" + btnid).attr("MaxFilesCount") != null) {
                    maxCount = parseInt($("#" + btnid).attr("MaxFilesCount"), 10)
                }
                if (maxCount > 0) {
                    var len = fileData.length;
                    len += $("#" + btnid + "-upload-list > li").length;
                    if (len > maxCount) {
                        $.facebox("最多只能上传" + maxCount + "个附件。");
                        return false;
                    }
                }
                return true;
            },
            onSubmit: function (id, fileName) {
                this._options.request.params.sizeLimit = fileSizeLimit;
                this._options.request.params.totalSizeLimit = totalSizeLimit;
            },
            onError: function (id, fileName, reason) {
                $.facebox(reason);
            },
            onComplete: function (id, fileName, responseJSON) {
                if (responseJSON.success) { }
            },
            onCancel: function (id, fileName) {
                $.ajax({
                    type: "POST",
                    url: deleteHandler,
                    data: { "folder": folder, "fileName": fileName },
                    success: function (data) {
                        var d = eval('(' + data + ')');
                        if (!d.success) {
                            $.fn.notify({ message: "文件删除出错，请稍候再试！" });
                        }
                    },
                    error: function (d) {
                        $.fn.notify({ message: "文件删除出错，请稍候再试！" });
                    }
                });
            }
        },
        messages: {
            typeError: "文件扩展名必须为: {extensions}。",
            sizeError: "{file} 文件太大, 最大支持 {sizeLimit}。",
            totalSizeError: "{file} 文件太大, 总文件大小不能超过 {totalSizeLimit}。",
            minSizeError: "{file} 文件太小, 最小支持 {minSizeLimit}。",
            emptyError: "{file} 文件不能为空。",
            regexError: (cfg && cfg.regexMessage && cfg.regexMessage.length > 0) ? cfg.regexMessage : "{file} 正则验证失败。",
            noFilesError: "请选择文件。",
            excludeError: "{file} 该扩展名不允许上传。",
            onLeave: "正在上传文件，如果离开，上传将取消。"
        },
        debug: false
    });
}

function EFConfigInitUploader(cfg) {
    if (typeof __gUploadConfig != 'undefined') {
        if (__gUploadConfig.exclude) {
            cfg.exclude = __gUploadConfig.exclude;
        }

        if (__gUploadConfig.fileSizeLimit) {
            var c = 0;
            if (__gUploadConfig.fileSizeLimit > 0 && cfg.fileSizeLimit > 0) {
                c = Math.min(__gUploadConfig.fileSizeLimit, cfg.fileSizeLimit);
            }
            else {
                if (__gUploadConfig.fileSizeLimit > 0) {
                    c = __gUploadConfig.fileSizeLimit;
                }
                if (cfg.fileSizeLimit > 0) {
                    c = cfg.fileSizeLimit;
                }
            }
            cfg.fileSizeLimit = c;
        }

        if (__gUploadConfig.totalFileSizeLimit) {
            var c = 0;
            if (__gUploadConfig.totalFileSizeLimit > 0 && cfg.totalFileSizeLimit > 0) {
                c = Math.min(__gUploadConfig.totalFileSizeLimit, cfg.totalFileSizeLimit);
            }
            else {
                if (__gUploadConfig.totalFileSizeLimit > 0) {
                    c = __gUploadConfig.totalFileSizeLimit;
                }
                if (cfg.totalFileSizeLimit > 0) {
                    c = cfg.totalFileSizeLimit;
                }
            }
            cfg.totalFileSizeLimit = c;
        }
    }

    EFInitUploader(cfg.buttonId, cfg.uploadHandler, cfg.deleteHandler, cfg.fileSizeLimit, cfg.totalFileSizeLimit, cfg.buttonText, cfg);
}

function EFDeleteUploadFile(el, deleteHandler, folder, fileName, fileID) {
    if (!fileName) {
        fileName = $(el).parent().find(".qq-upload-file").text();
    }
    $.ajax({
        type: "POST",
        url: deleteHandler,
        data: { "folder": folder, "fileName": fileName, "fileID": fileID },
        success: function (data) {
            var d = eval('(' + data + ')');
            if (!d.success) {
                $.fn.notify({ message: "文件删除出错，请稍候再试！" });
            } else {
                $(el).parent().remove();
            }
        },
        error: function (d) {
            $.fn.notify({ message: "文件删除出错，请稍候再试！" });
        }
    });
}

(function ($) {
    if ($.validator && $.validator.unobtrusive) {
        /*上传控件必填验证*/
        /*上传控件至少要上传几个文件*/
        $.validator.addMethod("fuRequired", function (value, element, param) {
            var elId = element.id + "-upload-list";
            if ($("#" + elId + " > li").length < parseInt($(element).attr("MinFilesCount"), 10)) return false;
            return true;
        });
        $.validator.unobtrusive.adapters.addBool("fuRequired");

        /*上传控件最多上传几个文件*/
        $.validator.addMethod("fuMaxFilesCount", function (value, element, param) {
            var elId = element.id + "-upload-list";
            if ($("#" + elId + " > li").length > parseInt($(element).attr("MaxFilesCount"), 10)) return false;
            return true;
        });
        $.validator.unobtrusive.adapters.addBool("fuMaxFilesCount");

        /*上传控件必须包含以下文件*/
        $.validator.addMethod("fuMustFiles", function (value, element, param) {
            var mf = eval('(' + $(element).attr("MustFiles") + ')');
            var elId = element.id + "-upload-list";
            if ($("#" + elId + " > li").length == 0 && mf.length > 0) return false;

            var files = $("#" + elId + " > li span.qq-upload-file");

            for (var i in mf) {
                var m = mf[i];
                var mreg = new RegExp(m);
                var mat = false;
                $.each(files, function (i, o) {
                    if ($(o).text().match(mreg)) {
                        mat = true;
                        return false;//break
                    }
                });
                if (mat == false) {
                    return false;
                }
            }

            return true;
        });
        $.validator.unobtrusive.adapters.addBool("fuMustFiles");
    }
}(jQuery));