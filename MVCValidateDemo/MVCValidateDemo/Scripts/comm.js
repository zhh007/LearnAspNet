/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

function checkZipcode(str) {
    return /^[1-9][0-9]{5}$/g.test(str);
}

function checkIdcard(str) {
    return /^[1-9]([0-9]{16}|[0-9]{13})[xX0-9]$/.test(str);
}

if ($.validator && $.validator.unobtrusive) {
    /*身份证*/
    $.validator.addMethod("idcard", function (value, element, param) {
        if (value == "") return true;
        return checkIdcard(value);
    });
    $.validator.unobtrusive.adapters.addBool("idcard");
    /*邮编*/
    $.validator.addMethod("zipcode", function (value, element, param) {
        if (value == null || value == '')
            return true;
        return checkZipcode(value);
    });
    $.validator.unobtrusive.adapters.addBool("zipcode");
    /*日期比较 - 小于*/
    $.validator.unobtrusive.adapters.addSingleVal("datelt", "toid");
    $.validator.addMethod("datelt", function (value, element, toid) {
        var str2 = $("#"+toid).val();
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
}