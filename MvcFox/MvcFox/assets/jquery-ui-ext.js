/*
1.修改jqueryui dialog 按钮为bootstrap button样式
2.修复对话框拉伸后的样式bug
*/
(function ($) {
    var _createButtons = $.ui.dialog.prototype._createButtons;
    $.ui.dialog.prototype._createButtons = function () {
        var that = this,
			buttons = this.options.buttons;

        // if we already have a button pane, remove it
        this.uiDialogButtonPane.remove();
        this.uiButtonSet.empty();

        if ($.isEmptyObject(buttons) || ($.isArray(buttons) && !buttons.length)) {
            this.uiDialog.removeClass("ui-dialog-buttons");
            return;
        }

        $.each(buttons, function (name, props) {
            var click, buttonOptions;
            props = $.isFunction(props) ?
				{ click: props, text: name } :
				props;
            // Default to a non-submitting button
            props = $.extend({ type: "button" }, props);
            // Change the context for the click callback to be the main element
            click = props.click;
            props.click = function () {
                click.apply(that.element[0], arguments);
            };
            buttonOptions = {
                icons: props.icons,
                text: props.showText
            };
            delete props.icons;
            delete props.showText;

            var btn = $("<button></button>", props);
            btn.addClass("btn btn-default");
            btn.appendTo(that.uiButtonSet);
        });
        this.uiDialog.addClass("ui-dialog-buttons");
        this.uiDialogButtonPane.appendTo(this.uiDialog);
    };

    var _create = $.ui.dialog.prototype._create;
    $.ui.dialog.prototype._create = function () {
        this.options.resizeStop = function (event, ui) {
            $(this).parents(".ui-dialog").first().css("height", "auto");
        }
        _create.apply(this);
    };
})(jQuery);

/*
处理ajax错误
*/
$(function () {
    var dialogOptions = {
        autoOpen: false,
        draggable: false,
        modal: true,
        resizable: false,
        minHeight: 200,
        minWidth: 400,
        title: "错误",
        closeOnEscape: false,
        open: function () { $(".ui-dialog-titlebar-close").hide(); },
        buttons: [{
            text: "确定",
            click: function () { $(this).dialog("close"); },
            'class': "btn btn-primary"
        }]
    };
    var dlg404 = $("<div>页面不存在。</div>").dialog(dialogOptions);
    var dlg500 = $("<div>服务器处理时发生错误。</div>").dialog(dialogOptions);

    $(document).ajaxError(function (event, jqXHR, ajaxSettings, thrownError) {
        if (jqXHR.status == 404) {
            dlg404.dialog("open");
        } else if (jqXHR.status == 500) {
            dlg500.dialog("open");
        } else if (jqXHR.status == 400) {
            //不处理ajax表单验证错误
        } else {
            alert("发生错误。");
        }
    });
});