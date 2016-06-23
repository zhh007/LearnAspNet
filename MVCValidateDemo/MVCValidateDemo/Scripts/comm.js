/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

function checkZipcode(str) {
    return /^[1-9][0-9]{5}$/g.test(str);
}

function checkIdcard(str) {
    return /^[1-9]([0-9]{16}|[0-9]{13})[xX0-9]$/.test(str);
}

function checkMobile(str) {
    return /^((\+86)|(86))?(1)\d{10}$/.test(str);
}

function checkNumeric(str) {
    return /^[+-]?([1-9]\d*)(\.\d*)?$|^[+-]?0(\.\d*[1-9]\d*)?$/.test(str);
}

function checkPosNumeric(str) {
    return /^([+]?[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^([+]?0\.\d*[1-9]\d*)$/.test(str);
}

function checkNegNumeric(str) {
    return /^(-[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^(-0\.\d*[1-9]\d*)$/.test(str);
}

function checkNonPosNumeric(str) {
    return /^(-[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^(-0\.\d*[1-9]\d*)$|^0$/.test(str);
}

function checkNonNegNumeric(str) {
    return /^([+]?[1-9]+\d*(\.\d*[1-9]?\d*)?)$|^([+]?0\.\d*[1-9]\d*)$|^0$/.test(str);
}

function checkInteger(str) {
    return /^[+-]?[1-9][0-9]*$|^0$/.test(str);
}

function checkPosInteger(str) {
    return /^[+]?[1-9][0-9]*$/.test(str);
}

function checkNegInteger(str) {
    return /^-[1-9][0-9]*$/.test(str);
}

function checkNonPosInteger(str) {
    return /^-[1-9][0-9]*$|^0$/.test(str);
}

function checkNonNegInteger(str) {
    return /^[+]?[1-9][0-9]*$|^0$/.test(str);
}

function checkDecimal(str) {
    return /^[+-]?([1-9]\d*)\.\d*$|^[+-]?0\.\d*[1-9]\d*$/.test(str);
}

function checkPosDecimal(str) {
    return /^[+]?([1-9]\d*)\.\d*$|^[+]?0\.\d*[1-9]\d*$/.test(str);
}

function checkNegDecimal(str) {
    return /^-([1-9]\d*)\.\d*$|^-0\.\d*[1-9]\d*$/.test(str);
}

if ($.validator && $.validator.unobtrusive) {
    /*身份证*/
    $.validator.addMethod("idcard", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkIdcard(value);
    });
    $.validator.unobtrusive.adapters.addBool("idcard");
    /*邮编*/
    $.validator.addMethod("zipcode", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkZipcode(value);
    });
    $.validator.unobtrusive.adapters.addBool("zipcode");
    /*手机*/
    $.validator.addMethod("mobile", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkMobile(value);
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
        return checkNumeric(value);
    });
    $.validator.unobtrusive.adapters.addBool("numeric");
    /*正数（正整数，正小数）*/
    $.validator.addMethod("posnumeric", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkPosNumeric(value);
    });
    $.validator.unobtrusive.adapters.addBool("posnumeric");
    /*负数（负整数，负小数）*/
    $.validator.addMethod("negnumeric", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkNegNumeric(value);
    });
    $.validator.unobtrusive.adapters.addBool("negnumeric");
    /*非正数（负整数，负小数，零）*/
    $.validator.addMethod("nonposnumeric", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkNonPosNumeric(value);
    });
    $.validator.unobtrusive.adapters.addBool("nonposnumeric");
    /*非负数（正整数，正小数，零）*/
    $.validator.addMethod("nonnegnumeric", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkNonNegNumeric(value);
    });
    $.validator.unobtrusive.adapters.addBool("nonnegnumeric");
    /*整数（正整数，负整数，零）*/
    $.validator.addMethod("integer", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkInteger(value);
    });
    $.validator.unobtrusive.adapters.addBool("integer");
    /*正整数*/
    $.validator.addMethod("posinteger", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkPosInteger(value);
    });
    $.validator.unobtrusive.adapters.addBool("posinteger");
    /*负整数*/
    $.validator.addMethod("neginteger", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkNegInteger(value);
    });
    $.validator.unobtrusive.adapters.addBool("neginteger");
    /*非正整数（负整数，零）*/
    $.validator.addMethod("nonposinteger", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkNonPosInteger(value);
    });
    $.validator.unobtrusive.adapters.addBool("nonposinteger");
    /*非负整数（正整数，零）*/
    $.validator.addMethod("nonneginteger", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkNonNegInteger(value);
    });
    $.validator.unobtrusive.adapters.addBool("nonneginteger");
    /*小数（正小数，负小数）*/
    $.validator.addMethod("decimal", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkDecimal(value);
    });
    $.validator.unobtrusive.adapters.addBool("decimal");
    /*正小数*/
    $.validator.addMethod("posdecimal", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkPosDecimal(value);
    });
    $.validator.unobtrusive.adapters.addBool("posdecimal");
    /*负小数*/
    $.validator.addMethod("negdecimal", function (value, element, param) {
        if (value == null || value == '') return true;
        return checkNegDecimal(value);
    });
    $.validator.unobtrusive.adapters.addBool("negdecimal");
}