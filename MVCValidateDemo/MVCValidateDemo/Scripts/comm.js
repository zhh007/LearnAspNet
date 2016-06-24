/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

(function ($) {
    $.extend($, {
        //邮编
        checkZipcode: function (str) {
            return /^[1-9][0-9]{5}$/g.test(str);
        },
        //身份证
        checkIdcard: function (str) {
            return /^[1-9]([0-9]{16}|[0-9]{13})[xX0-9]$/.test(str);
        },
        //手机
        checkMobile: function (str) {
            return /^((\+86)|(86))?(1)\d{10}$/.test(str);
        },
        //有效的数字（整数，小数，零）
        checkNumeric: function (str) {
            return /^[+-]?([1-9]\d*)(\.\d*)?$|^[+-]?0(\.\d*[1-9]\d*)?$/.test(str);
        },
        //正数（正整数，正小数）
        checkPosNumeric: function (str) {
            return /^([+]?[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^([+]?0\.\d*[1-9]\d*)$/.test(str);
        },
        //负数（负整数，负小数）
        checkNegNumeric: function (str) {
            return /^(-[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^(-0\.\d*[1-9]\d*)$/.test(str);
        },
        //非正数（负整数，负小数，零）
        checkNonPosNumeric: function (str) {
            return /^(-[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^(-0\.\d*[1-9]\d*)$|^0$/.test(str);
        },
        //非负数（正整数，正小数，零）
        checkNonNegNumeric: function (str) {
            return /^([+]?[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^([+]?0\.\d*[1-9]\d*)$|^0$/.test(str);
        },
        //整数（正整数，负整数，零）
        checkInteger: function (str) {
            return /^[+-]?[1-9][0-9]*$|^0$/.test(str);
        },
        //正整数
        checkPosInteger: function (str) {
            return /^[+]?[1-9][0-9]*$/.test(str);
        },
        //负整数
        checkNegInteger: function (str) {
            return /^-[1-9][0-9]*$/.test(str);
        },
        //非正整数（负整数，零）
        checkNonPosInteger: function (str) {
            return /^-[1-9][0-9]*$|^0$/.test(str);
        },
        //非负整数（正整数，零）
        checkNonNegInteger: function (str) {
            return /^[+]?[1-9][0-9]*$|^0$/.test(str);
        },
        //小数（正小数，负小数）
        checkDecimal: function (str) {
            return /^[+-]?([1-9]\d*)\.\d*$|^[+-]?0\.\d*[1-9]\d*$/.test(str);
        },
        //正小数
        checkPosDecimal: function (str) {
            return /^[+]?([1-9]\d*)\.\d*$|^[+]?0\.\d*[1-9]\d*$/.test(str);
        },
        //负小数
        checkNegDecimal: function (str) {
            return /^-([1-9]\d*)\.\d*$|^-0\.\d*[1-9]\d*$/.test(str);
        }
    });

    if ($.validator && $.validator.unobtrusive) {
        /*邮编*/
        $.validator.addMethod("zipcode", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkZipcode(value);
        });
        $.validator.unobtrusive.adapters.addBool("zipcode");
        /*身份证*/
        $.validator.addMethod("idcard", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkIdcard(value);
        });
        $.validator.unobtrusive.adapters.addBool("idcard");
        /*手机*/
        $.validator.addMethod("mobile", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkMobile(value);
        });
        $.validator.unobtrusive.adapters.addBool("mobile");
        /*日期比较 - 小于*/
        $.validator.unobtrusive.adapters.addSingleVal("datelt", "toid");
        $.validator.addMethod("datelt", function (value, element, toid) {
            var str2 = $("#" + toid).val();
            if (value == '' || str2 == '') {
                return true;
            }
            var d1 = new Date(value);
            var d2 = new Date(str2);
            return d1 < d2;
        });
        /*日期比较 - 小于等于*/
        $.validator.unobtrusive.adapters.addSingleVal("datele", "toid");
        $.validator.addMethod("datele", function (value, element, toid) {
            var str2 = $("#" + toid).val();
            if (value == '' || str2 == '') {
                return true;
            }
            var d1 = new Date(value);
            var d2 = new Date(str2);
            return d1 <= d2;
        });
        /*日期比较 - 大于*/
        $.validator.unobtrusive.adapters.addSingleVal("dategt", "toid");
        $.validator.addMethod("dategt", function (value, element, toid) {
            var str2 = $("#" + toid).val();
            if (value == '' || str2 == '') {
                return true;
            }
            var d1 = new Date(value);
            var d2 = new Date(str2);
            return d1 > d2;
        });
        /*日期比较 - 大于等于*/
        $.validator.unobtrusive.adapters.addSingleVal("datege", "toid");
        $.validator.addMethod("datege", function (value, element, toid) {
            var str2 = $("#" + toid).val();
            if (value == '' || str2 == '') {
                return true;
            }
            var d1 = new Date(value);
            var d2 = new Date(str2);
            return d1 >= d2;
        });
        /*日期比较 - 等于*/
        $.validator.unobtrusive.adapters.addSingleVal("dateeq", "toid");
        $.validator.addMethod("dateeq", function (value, element, toid) {
            var str2 = $("#" + toid).val();
            if (value == '' || str2 == '') {
                return true;
            }
            var d1 = new Date(value);
            var d2 = new Date(str2);
            return d1.getTime() === d2.getTime();
        });
        /*日期比较 - 不等于*/
        $.validator.unobtrusive.adapters.addSingleVal("datene", "toid");
        $.validator.addMethod("datene", function (value, element, toid) {
            var str2 = $("#" + toid).val();
            if (value == '' || str2 == '') {
                return true;
            }
            var d1 = new Date(value);
            var d2 = new Date(str2);
            return d1.getTime() !== d2.getTime();
        });

        /*有效的数字（整数，小数，零）*/
        $.validator.addMethod("numeric", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNumeric(value);
        });
        $.validator.unobtrusive.adapters.addBool("numeric");
        /*正数（正整数，正小数）*/
        $.validator.addMethod("posnumeric", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkPosNumeric(value);
        });
        $.validator.unobtrusive.adapters.addBool("posnumeric");
        /*负数（负整数，负小数）*/
        $.validator.addMethod("negnumeric", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNegNumeric(value);
        });
        $.validator.unobtrusive.adapters.addBool("negnumeric");
        /*非正数（负整数，负小数，零）*/
        $.validator.addMethod("nonposnumeric", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNonPosNumeric(value);
        });
        $.validator.unobtrusive.adapters.addBool("nonposnumeric");
        /*非负数（正整数，正小数，零）*/
        $.validator.addMethod("nonnegnumeric", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNonNegNumeric(value);
        });
        $.validator.unobtrusive.adapters.addBool("nonnegnumeric");
        /*整数（正整数，负整数，零）*/
        $.validator.addMethod("integer", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkInteger(value);
        });
        $.validator.unobtrusive.adapters.addBool("integer");
        /*正整数*/
        $.validator.addMethod("posinteger", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkPosInteger(value);
        });
        $.validator.unobtrusive.adapters.addBool("posinteger");
        /*负整数*/
        $.validator.addMethod("neginteger", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNegInteger(value);
        });
        $.validator.unobtrusive.adapters.addBool("neginteger");
        /*非正整数（负整数，零）*/
        $.validator.addMethod("nonposinteger", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNonPosInteger(value);
        });
        $.validator.unobtrusive.adapters.addBool("nonposinteger");
        /*非负整数（正整数，零）*/
        $.validator.addMethod("nonneginteger", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNonNegInteger(value);
        });
        $.validator.unobtrusive.adapters.addBool("nonneginteger");
        /*小数（正小数，负小数）*/
        $.validator.addMethod("decimal", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkDecimal(value);
        });
        $.validator.unobtrusive.adapters.addBool("decimal");
        /*正小数*/
        $.validator.addMethod("posdecimal", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkPosDecimal(value);
        });
        $.validator.unobtrusive.adapters.addBool("posdecimal");
        /*负小数*/
        $.validator.addMethod("negdecimal", function (value, element, param) {
            if (value == null || value == '') return true;
            return $.checkNegDecimal(value);
        });
        $.validator.unobtrusive.adapters.addBool("negdecimal");
    }

    //输入限制
    function limitInput() {
        //邮政编码（0-9）
        $("input[data-val-zipcode]").css("ime-mode", "disabled").bind("keypress", function () {
            if (event.keyCode < 48 || event.keyCode > 57) { event.returnValue = false; return false; }
        });

        //身份证（0-9，x，X）
        $("input[data-val-idcard]").css("ime-mode", "disabled").bind("keypress", function () {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 88 && event.keyCode != 120)) { event.returnValue = false; return false; }
        });

        //手机号码（0-9，+）
        $("input[data-val-mobile]").css("ime-mode", "disabled").bind("keypress", function () {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 43)) { event.returnValue = false; return false; }
        });

        /*日期（0-9，-，/）*/
        $("input[data-val-date],input[data-val-datelt],input[data-val-datele],input[data-val-dategt],input[data-val-datege],input[data-val-dateeq],input[data-val-datene]")
            .css("ime-mode", "disabled").bind("keypress", function () {
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 45 && event.keyCode != 47)) { event.returnValue = false; return false; }
            });

        /*有效的数字，小数（0-9，.，+，-）*/
        $("input[data-val-numeric],input[data-val-decimal]").css("ime-mode", "disabled").bind("keypress", function () {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 46 && event.keyCode != 43 && event.keyCode != 45)) { event.returnValue = false; return false; }
        });

        //正数，非负数，正小数（0-9，.，+）
        $("input[data-val-posnumeric],input[data-val-nonnegnumeric],input[data-val-posdecimal]")
            .css("ime-mode", "disabled").bind("keypress", function () {
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 46 && event.keyCode != 43)) { event.returnValue = false; return false; }
            });

        //负数，非正数，负小数（0-9，.，-）
        $("input[data-val-negnumeric],input[data-val-nonposnumeric],input[data-val-negdecimal]")
            .css("ime-mode", "disabled").bind("keypress", function () {
                if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 46 && event.keyCode != 45)) { event.returnValue = false; return false; }
            });

        //整数（0-9，+，-）
        $("input[data-val-integer]").css("ime-mode", "disabled").bind("keypress", function () {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 43 && event.keyCode != 45)) { event.returnValue = false; return false; }
        });

        //正整数，非负整数（0-9，+）
        $("input[data-val-posinteger],input[data-val-nonneginteger]").css("ime-mode", "disabled").bind("keypress", function () {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 43)) { event.returnValue = false; return false; }
        });

        //负整数，非正整数（0-9，-）
        $("input[data-val-neginteger],input[data-val-nonposinteger]").css("ime-mode", "disabled").bind("keypress", function () {
            if ((event.keyCode < 48 || event.keyCode > 57) && (event.keyCode != 45)) { event.returnValue = false; return false; }
        });

    }

    $(function () {
        limitInput();
    });

}(jQuery));