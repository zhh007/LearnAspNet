﻿/*-----------------------九宫格JS代码------------------------------*/

function deletePic(obj, boxid) {
    var thispic = $(obj).parents("div.speed-main");
    thispic.remove();
    buildPicInputName(boxid);
    $("#btn" + boxid).show();
}

function buildPicInputName(elId) {
    $("#" + elId).find("div.speed-main").each(function (i, o) {
        $(this).find("input[class*='filename']").attr("name", elId + "FileName" + i);
        $(this).find("input[class*='thumburl']").attr("name", elId + "ThumbUrl" + i);
    });
}

function init9PicUploader(curFolder, btid, endpointUrl, boxid, max) {
    var curLimitfilesize = 10 * 1024 * 1024; // 1 MB = 1 * 1024 * 1024 bytes
    var lImageBoxHeight = 120, lImageBoxWidth = 120;

    var uploader = new qq.FineUploader({
        element: document.getElementById(btid),
        text: {
            uploadButton: '　'
        },
        multiple: false,
        validation: {
            allowedExtensions: ['jpeg', 'jpg', 'png'],
            sizeLimit: curLimitfilesize //5 * 1024 * 1024 // 5 MB = 5 * 1024 * 1024 bytes
        },
        request: {
            endpoint: endpointUrl,
            params: {
                "folder": curFolder,
                "thumbWidth": lImageBoxWidth,
                "thumbHeight": lImageBoxHeight,
                "sizeLimit": curLimitfilesize
            },
            forceMultipart: true
        },
        callbacks: {
            onSubmit: function (id, fileName) {
                this._options.request.params.sizeLimit = curLimitfilesize;
                var html = '<div class="speed-main clsimagepre"><div class="progress" style="margin:47px auto; width:80px"></div></div>';
                $("#" + btid).before(html);
                $("#" + btid).hide();
            },
            onError: function (id, fileName, reason) {
                //alert(reason);
            },
            onProgress: function (id, fileName, loaded, total) {
                var percent = Math.round(loaded / total * 100);
                var phinner = '<div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: ' + percent + '%;">';
                phinner += percent + "%";
                phinner += '</div>';
                $("#" + boxid).find(".progress").html(phinner);
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
        $("#" + boxid).find(".clsimagepre").remove();
        var len = $("#" + boxid).children(".speed-main").length;
        var filename = boxid + "FileName" + (len - 1);
        var thumburl = boxid + "ThumbUrl" + (len - 1);
        var html = '<div class="speed-main">';
        html += '<div class="speed-img">';
        html += '<img src="' + responseJson.thumburl + '"></div>';
        html += '<div class="speed-del">';
        html += "<a href='javascript:;' onclick=\"deletePic(this,\'" + boxid + "\')\"><i class='fa fa-minus-circle fa-lg'></i>";
        html += '</a>';
        html += '</div>';
        html += '<input type="hidden" class="filename" value="' + responseJson.filename + '" name="' + filename + '"/>';
        html += '<input type="hidden" class="thumburl" value="' + responseJson.thumburl + '" name="' + thumburl + '"/>';
        html += '</div>';
        $("#" + btid).before(html);
        if (len >= max) {
            $("#" + btid).hide();
        } else {
            $("#" + btid).show();
        }
    }
}

/*-----------------------九宫格JS代码end------------------------------*/