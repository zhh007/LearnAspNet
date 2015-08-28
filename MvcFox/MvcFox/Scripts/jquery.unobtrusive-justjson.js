(function ($) {
    var $jQval = $.validator,
        form_selector = 'form.use-justjson',
        form_data_key = 'unobtrusiveJson',
        //beforesubmit_event = 'before-submit',
        success_event = 'ajax-success',
        failed_event = 'ajax-failed',
        field_failed = 'field-invalid',
        field_passed = 'field-valid',
        error_event = 'field-validated';

    function onValidated(form, validator) {  // 'this' is the form element
        var f$ = $(this),
            data = f$.data(form_data_key);

        var args = {
            errors: validator.errorList.concat($.map(data.errors, function (item) { return { message: item, element: null }; })),
            useDefault: true
        };

        f$.triggerHandler(error_event, [args]);

        if (args.useDefault && data.invalidHandler) {
            data.invalidHandler(form, validator);
        }
    }

    function default_ajaxsuccess() {
        alert("保存成功。");
    }

    function default_ajaxfailed(errorData) {
        var result = "";
        result += '<div class="row">';
        result += '<div class="col-md-12">';
        result += '<div class="alert alert-danger">';
        result += '<span class="glyphicon glyphicon-exclamation-sign"></span>请处理以下错误：';
        result += '</div>';
        result += '</div>';
        result += '</div>';
        result += '<div><ul>';
        $.each(errorData, function (k, v) {
            result += '<li>' + v + '</li>';
        });
        result += '</ul></div>'
        
        $('<div></div>').html(result).dialog({
            title: "错误提示",
            resizable: false,
            draggable: false,
            modal: true,
            minHeight: 300,
            minWidth: 500,
            closeOnEscape: false,
            open: function () { $(".ui-dialog-titlebar-close").hide(); },
            buttons: [{
                    text: "确定",
                    click: function () { $(this).dialog("close"); },
                    'class': "btn btn-primary"
            }]
        });
    }

    $jQval.unobtrusive.justjson = {
        parse: function () {

            var registered = [];
            // find all forms that are are to post back via AJAX and return JSON.
            $(form_selector).each(function (i, form) {
                var f$ = $(form);

                f$
                //.bind('before-submit', function (ev, form, args) {

                //})
                //.bind('ajax-success', function () {
                //    // do something.
                //    alert("保存成功。");
                //})
                //.bind('ajax-failed', function () {
                //    // maybe do something.
                //    alert("发生错误。");
                //})
                //.bind('field-validated', function (ev, args) {
                //    // add or remove errors to error summary using args.errors.
                //})
                .bind('field-valid', function (ev, el) {
                    // do your own custom field error processing
                    // (this here integrates for bootstrap style HTML errors).
                    $(el).parents('div.control-group').removeClass('error');
                })
                .bind('field-invalid', function (ev, el) {
                    $(el).parents('div.control-group').addClass('error');
                });

                // store extra state info in form.data item. if already exists then skip over as we 
                // have already wired-up this form.
                if (!f$.data(form_data_key)) {
                    var validate = f$.validate();

                    // rebind the invalid-form event (triggered by jquery.validate) so we can replace 
                    // unobtrusive onErrors function with our own.
                    var invalidHandler = validate.settings.invalidHandler;
                    f$.unbind("invalid-form.validate")
                      .bind("invalid-form.validate", $.proxy(onValidated, form));

                    // intercept unobtrusive onError function so we can trigger update of summary error
                    var defaultHandle = validate.settings.errorPlacement;

                    validate.settings.errorPlacement = $.proxy(function (error, inputElement) {
                        // first call unobtrusive's errorPlacement handler.
                        defaultHandle(error, inputElement);

                        // invalid-form event triggers the validate's invalidHandler method. we want this
                        // to be called whenever a field validation takes place so that our onValidate function
                        // can be called.
                        f$.triggerHandler("invalid-form", [validate]);

                        // notify any external listeners by firing custom pass or fail event.
                        f$.triggerHandler(error.text() == '' ? field_passed : field_failed, [inputElement]);
                    }, form);

                    //implement a reset form capability by hooking into internal validate mechanism.
                    f$.unbind("resetform.justjson").bind("resetform.justjson", function () {
                        // record errorList elements (contains the currently displayed errors)
                        var errorElements = $.map(validate.errorList, function (item) { return item.element; });

                        // reserForm will clear all validation internal state including errorList.
                        validate.resetForm();

                        // successList are fields that have since passed validation so set to error elements
                        validate.successList = errorElements;

                        f$.data(form_data_key).errors = [];

                        // showErrors will process errorList (nw empty) and succssList internally and do
                        // appropriate cleanup handling.
                        validate.showErrors();

                        // trigger onValidated handler so clean up any summary errors.
                        f$.triggerHandler("invalid-form", [validate]);

                    });

                    f$.data(form_data_key, { errors: [], invalidHandler: invalidHandler });

                    // note which forms have been processed.
                    registered[registered.length] = form;
                }
            });

            // intercept the form submit event so we can perform ajax action
            $(registered).unbind('submit.justjson').bind('submit.justjson', function (evt) {
                var f$ = $(this);

                if (!f$.valid())
                    return false;

                // extract values to submit         
                //var data = {};
                //f$.find(':input').each(function (i, el) {
                //    // TODO: enhance to handle array values.    
                //    if (el.name)
                //        data[el.name] = $(el).val();
                //});

                // notify any external listeners that we are about to submit the form. 
                //f$.triggerHandler(beforesubmit_event, [f$, data]);

                $.ajax({
                    url: f$.attr('action'),
                    data: f$.serialize(),//ko.toJSON(data),
                    type: 'post'//,
                    //contentType: 'application/json; charset=utf-8'
                })
                .done(function (result) { // success callback...        
                    f$.data(form_data_key).errors = [];
                    f$.triggerHandler("invalid-form", [f$.validate()]);
                    if ($._data(f$[0], "events")[success_event] != null) {
                        f$.triggerHandler(success_event, [result]);
                    } else {
                        default_ajaxsuccess();
                    }
                })
                .fail(function (err) {
                    var errorData = $.parseJSON(err.responseText).errors;

                    var fld = {};
                    for (var p in errorData) {
                        if (p === '') {
                            // general errors are stored in custom data item.
                            f$.data(form_data_key).errors = errorData[p];
                        } else {
                            // build error object suitable for jquery validate consumption. 
                            var msg = errorData[p].join(', ');
                            if (msg.length > 0)
                                fld[p] = msg;
                        }
                    }
                    // displayed field errors.
                    f$.validate().showErrors(fld);
                    if ($._data(f$[0], "events")[failed_event] != null) {
                        f$.triggerHandler(failed_event, [err]);
                    } else {
                        default_ajaxfailed(errorData);
                    }
                });

                // stop form submitting
                evt.preventDefault();
            });

        }
    };

    $(function () {
        $jQval.unobtrusive.justjson.parse();
    });
}(jQuery));