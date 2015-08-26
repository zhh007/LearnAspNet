/* TorchUI dialog button fix */
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