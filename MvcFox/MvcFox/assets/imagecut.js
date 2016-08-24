(function(){
    $.imgcut = function (folder
        , uploadUrl, rotateUrl, saveUrl, showUrl, thumbUrl
        , btnId, maxCount, jscallback, fnCountStr, isdouble
        , leftBoxWidth, leftBoxHeight, rightBoxWidth, rightBoxHeight
        , dlgId, dlgHeight, dlgWidth
        , leftimgboxId, rightimgboxId, previewId
        , btnUploadId, btnLeftId, btnRightId, btnSaveId, btnCancelId
        ){

        var x, y, w, h;
        
        var cur_folder = folder;
        var cur_fileid = null;

        var sPercent = 0;
        
        var lImageBox_width = leftBoxWidth;
        var lImageBox_height = leftBoxHeight;
        var rImageBox_width = rightBoxWidth;
        var rImageBox_height = rightBoxHeight;

        //上传后显示在左边的图片大小
        var up_width = 0;
        var up_height = 0;

        var cur_limitfilesize = 1*1024*1024;// 1 MB = 1 * 1024 * 1024 bytes

        var dlg = null;
        var angle = 0;

        var btn = $("#" + btnId);
        var btnUpload = $("#" + btnUploadId);
        var btnLeft = $("#" + btnLeftId);
        var btnRight = $("#" + btnRightId);
        var btnSave = $("#" + btnSaveId);
        var btnCancel = $("#" + btnCancelId);
        var leftImgBox = $("#" + leftimgboxId);
        var rightImgBox = $("#" + rightimgboxId);

        var _double = (isdouble ? 1 : 0);

        function initUploader2() {
            var uploader = new qq.FineUploader({
                element: btnUpload[0],
                text: {
                    uploadButton: '　'
                },
                multiple: false,
                validation: {
                    allowedExtensions: ['jpeg', 'jpg', 'gif', 'png'],
                    sizeLimit: cur_limitfilesize
                },
                request: {
                    endpoint: uploadUrl,
                    params: { "folder": cur_folder, "maxWidth": lImageBox_width
                        , "maxHeight": lImageBox_height, "sizeLimit": cur_limitfilesize },
                    forceMultipart: true
                },
                callbacks: {
                    onSubmit: function (id, fileName) {
                        this._options.request.params.sizeLimit = cur_limitfilesize;
                    },
                    onError: function (id, fileName, reason) {
                    },
                    onComplete: function (id, fileName, responseJSON) {
                        if (responseJSON.success) {
                            cur_fileid = responseJSON.fileid;
                            sPercent = responseJSON.spercent;
                            hResolution = responseJSON.hResolution;//水平分辨率（以“像素/英寸”为单位）
                            vResolution = responseJSON.vResolution;//垂直分辨率（以“像素/英寸”为单位）
                            up_width = responseJSON.width;
                            up_height = responseJSON.height;

                            bindImg();
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
                    new Messi(message, { title: '提示', titleClass: 'anim error', modal: true, buttons: [{ id: 0, label: '关闭', val: 'X'}] });
                },
                debug: false
            });

            $(".qq-upload-button > div").height("24px").width("112px");
        }

        function getShowUrl(folder, fileid){
            return showUrl + "?fileid=" + fileid + "&folder=" + folder + "&d=" + Math.random();
        }

        function getThumbnailUrl(folder, fileid){
            return thumbUrl + "?fileid=" + fileid + "&folder=" + folder + "&d=" + Math.random();
        }

        function bindImg() {
            var targetid = "_" + Math.random();
            targetid = targetid.replace(".", "");
            var image1url = getThumbnailUrl(cur_folder, cur_fileid);
            var image2url = getThumbnailUrl(cur_folder, cur_fileid);
            leftImgBox.html("<img id='" + targetid + "' src='" + image1url + "' />");
            rightImgBox.html("<img src='" + image2url + "' id='"+previewId+"' alt='Preview' class='jcrop-preview' />");

            setTimeout(function(){
                var jcrop_api = $('#' + targetid).Jcrop({
                    onChange: updatePreview2,
                    onSelect: updatePreview2,
                    aspectRatio: rImageBox_width/rImageBox_height,
                    bgFade: true,
                    bgOpacity: .3,
                    setSelect: [0, 0, rImageBox_width, rImageBox_height],
                    allowResize: true//,
                });
            }, 1000);
        }

        //参数coords表示截图框的大小
        function updatePreview2(coords, boundx, boundy) {
            if (parseInt(coords.w) > 0) {
                if(boundx == undefined || boundy == undefined) {
                    return;
                }
                x = coords.x;
                y = coords.y;
                w = coords.w;
                h = coords.h;
                var rx = rImageBox_width / coords.w;
                var ry = rImageBox_height / coords.h;
                $("#" +previewId).css({
                    width: Math.round(rx * up_width) + 'px',
                    height: Math.round(ry * up_height) + 'px',
                    marginLeft: '-' + Math.round(rx * coords.x) + 'px',
                    marginTop: '-' + Math.round(ry * coords.y) + 'px'
                });
            }
        }

        function showImage2(imgEId, fileid, folder) {
            var imgUrl = getShowUrl(folder, fileid);
            var rawImg = new Image();
            rawImg.onload = function () {
                var raw_height = rawImg.height;
                var raw_width = rawImg.width;
                var callback = jscallback
                if(typeof(callback) == "function") {
                    callback(folder, fileid, imgUrl);
                }
            }
            rawImg.src = imgUrl; // this must be done AFTER setting onload
        }

        function clear() {
            cur_fileid = null;
            angle = 0;
            $(".qq-upload-list").empty();
            leftImgBox.empty();
            rightImgBox.empty();
        }

        function fnSave() {
            if(cur_fileid == null) {
                clear();
                var callback = jscallback;
                if(typeof(callback) == "function") {
                    callback(null, null, null);
                }
                dlg.dialog( "close" );
                return;
            }
            var param = {
                "fileid": cur_fileid, "folder": cur_folder, "percent": sPercent
                , "x": x / sPercent, "y": y / sPercent, "w": w / sPercent, "h": h / sPercent
                , "maxWidth": rImageBox_width, "maxHeight": rImageBox_height
                , "angle" : angle, "_double" : _double
            };
            $.post(saveUrl, param, function (data) {
                if (data.success) {
                    showImage2("imgHead", cur_fileid, cur_folder);
                    clear();
                    dlg.dialog( "close" );
                }
            }, "json");
        }
        
        function fnCancel() {
            clear();
            var callback = jscallback;
            if(typeof(callback) == "function") {
                callback(null, null, null);
            }
            dlg.dialog( "close" );
        }

        function rotateImage(ang) {
            var data = {
                'fileid':cur_fileid
                ,'folder':cur_folder
                ,'angle':ang
                ,'maxWidth':lImageBox_width
                ,'maxHeight':lImageBox_height
            };
            $.post(rotateUrl, data, function (data) {
                if (data.success) {
                    up_width = data.width;
                    up_height = data.height;
                    sPercent = data.percent;
                    bindImg();
                }
            }, "json");
        }
        
        function fnRotateLeft() {
            angle += (90);
            rotateImage(angle);
        }
        function fnRotateRight() {
            angle += (-90);
            rotateImage(angle);
        }

        $(function () {
            btn.click(function(){
                try {
                    var fn_Count = fnCountStr;
                    if(maxCount > 0 && typeof(fn_Count) == "function") {
                        if(fn_Count() >= maxCount) {
                            showErrorMsg("最多上传" + maxCount + "张图片。");
                            return;
                        }
                    }
                } catch(e) {}

                dlg = $( "#" + dlgId ).dialog({
                    height: dlgHeight, width: dlgWidth, modal: true
                    ,close: function( event, ui ) {
                        clear();
                        dlg = null;
                    }});
            });
            btnLeft.click(function(){
                fnRotateLeft();
            });
            btnRight.click(function(){
                fnRotateRight();
            });
            btnSave.click(function(){
                fnSave();
            });
            btnCancel.click(function(){
                fnCancel();
            });
            initUploader2();
        });

    }

})();