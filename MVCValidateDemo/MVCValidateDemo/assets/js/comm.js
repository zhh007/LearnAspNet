/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

/*
*功能：字符串去两边空格
*参数：
*返回：
*/
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

/*
*功能：取字符串长度
*参数：
*返回：
*/
String.prototype.len = function () {
    return this.replace(/[^\x00-\xff]/g, "aa").length;
}

/*
*小数格式化
*参数：n-待格式化数字,b-保留小数点位数
*返回：格式化后的数字
*/
function formatNumber(n, b) {
    return Math.round(n * Math.pow(10, b)) / Math.pow(10, b);
}

/*
*功能：判断日期类型是否为YYYY-MM-DD或YYYY/MM/DD
*参数：d-待检参数
*返回：格式正确返回true,否则返回false
*/
function isDateFormat(d) {
    var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/;
    return reg.test(d.trim());
}

/*
*功能：判断日期类型是否为hh:mm:ss(24小时制)
*参数：t-待检参数
*返回：格式正确返回true,否则返回false
*/
function isTimeFormat(t) {
    var reg = /^((20|21|22|23|[0-1]\d)\:[0-5][0-9])(\:[0-5][0-9])?$/;
    return reg.test(t.trim());
}

/*
*功能：判断日期类型是否为YYYY-MM-DD hh:mm:ss(或YYYY/MM/DD hh:mm:ss)
*参数：d-待检参数
*返回：格式正确返回true,否则返回false
*/
function IsDateTimeFormat(d) {
    var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
    return reg.test(d.trim());
}

/*
*功能：日期有效性判断(YYYY-MM-DD)
*参数：d-待检参数
*返回：有效返回true,否则返回false
*/
function isDate(d) {
    d = d.replace(/\s+/g, ""); //去所有空格
    var reg = /^(\d{4})-(\d{1,2})-(\d{1,2})$/;
    if (reg.test(d)) {
        var y = parseInt(RegExp.$1);
        var m = parseInt(RegExp.$2);
        var d = parseInt(RegExp.$3);
        switch (m) {
            case 1:
            case 3:
            case 5:
            case 7:
            case 8:
            case 10:
            case 12:
                if (d > 31) {
                    return false;
                } else {
                    return true;
                }
                break;
            case 2:
                if ((y % 4 == 0 && d > 29) || ((y % 4 != 0 && d > 28))) {
                    return false;
                }
                else {
                    return true;
                }
                break;
            case 4:
            case 6:
            case 9:
            case 11:
                if (d > 30) {
                    return false;
                } else {
                    return true;
                }
                break;
            default:
                return false;
        }
    }
    else {
        return false;
    }
}

//校验合法时间   
function isDate2(e) {
    if (!/^(\d{4})(\.|\/|\-)(\d{1,2})(\.|\/|\-)(\d{1,2})$/.test(e)) {
        return false;
    }
    var y = parseInt(RegExp.$1);
    var m = parseInt(RegExp.$3);
    var d = parseInt(RegExp.$5);
    var D = new Date(y, m - 1, d);
    return (D.getFullYear() == y && (D.getMonth() + 1) == m && D.getDate() == d);
};

/*
*功能：判断输入的EMAIL格式是否正确
*参数：e-待检参数
*返回：Email返回true,否则返回false
*/
function isEmail(e) {
    var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return reg.test(e.trim());
}

/*
Phone : /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/
Mobile : /^((\(\d{2,3}\))|(\d{3}\-))?13\d{9}$/
Url : /^http:\/\/[A-Za-z0-9]+\.[A-Za-z0-9]+[\/=\?%\-&_~`@[\]\':+!]*([^<>\"\"])*$/
IdCard : /^\d{15}(\d{2}[A-Za-z0-9])?$/
QQ : /^[1-9]\d{4,8}$/
*/

/*
*功能：校验ip地址的格式
*参数：i-待检参数
*返回：格式正确返回true,否则返回false
*/
function isIP(i) {
    var reg = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/;
    if (reg.test(i.trim())) {
        if (RegExp.$1 < 256 && RegExp.$2 < 256 && RegExp.$3 < 256 && RegExp.$4 < 256) return true;
    }
    return false;
}

//比较时间 格式 yyyy-mm-dd hh:mi:ss
function dateCompare(beginTime, endTime) {
    var beginTimes = beginTime.substring(0, 10).split('-');
    var endTimes = endTime.substring(0, 10).split('-');

    beginTime = beginTimes[1] + '-' + beginTimes[2] + '-' + beginTimes[0] + ' ' + beginTime.substring(10, 19);
    endTime = endTimes[1] + '-' + endTimes[2] + '-' + endTimes[0] + ' ' + endTime.substring(10, 19);

    // alert(Date.parse(endTime));alert(Date.parse(beginTime));

    var a = (Date.parse(endTime) - Date.parse(beginTime)) / 3600 / 1000;

    if (a < 0) {
        return -1;
    } else if (a > 0) {
        return 1;
    } else if (a == 0) {
        return 0;
    } else {
        return 'exception'
    }
}

function getFrameWindow(framename) {
    var ifr = window.frames[framename];
    return ifr;
}
function createHideFrame(framename) {
    var frame = document.createElement("iframe");
    frame.name = framename;
    frame.id = framename;
    document.body.appendChild(frame);
    frame.style.width = "0px";
    frame.style.height = "0px";
    frame.style.border = "0px";
    var frameWindow = window.frames[framename];
    return frameWindow;
}

function openDialog(opt) {
    var cmd = "resizable: " + (opt.resizable ? opt.resizable : "no") + ";";
    cmd += "status: " + (opt.status ? opt.status : "no") + ";";
    cmd += "scroll: " + (opt.scroll ? opt.scroll : "no") + ";";
    cmd += "help: " + (opt.help ? opt.help : "no") + ";";
    cmd += "center: " + (opt.center ? opt.center : "no") + ";";
    cmd += "dialogWidth: " + (opt.dialogWidth ? opt.dialogWidth : "700px") + ";";
    cmd += "dialogHeight: " + (opt.dialogHeight ? opt.dialogHeight : "600px") + ";";

    if (opt.boxurl.indexOf("?") != -1)
        opt.boxurl += "&tmp=" + Math.random();
    else
        opt.boxurl += "?tmp=" + Math.random();

    if (opt.url.indexOf("?") != -1)
        opt.url += "&isdialog=1&tmp=" + Math.random();
    else
        opt.url += "?isdialog=1&tmp=" + Math.random();

    var args = extend({ __url: opt.url }, opt.args, true);

    if (opt.modal)
        return window.showModalDialog(opt.boxurl, args, cmd);
    else
        return window.showModelessDialog(opt.boxurl, args, cmd);
}

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
        $("input[data-val-zipcode]").css("ime-mode", "disabled").bind("keypress", function (e) {
            if (e.keyCode < 48 || e.keyCode > 57) { return false; }
        });

        //身份证（0-9，x，X）
        $("input[data-val-idcard]").css("ime-mode", "disabled").bind("keypress", function (e) {
            if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 88 && e.keyCode != 120)) { return false; }
        });

        //手机号码（0-9，+）
        $("input[data-val-mobile]").css("ime-mode", "disabled").bind("keypress", function (e) {
            if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 43)) { return false; }
        });

        /*日期（0-9，-，/）*/
        $("input[data-val-date],input[data-val-datelt],input[data-val-datele],input[data-val-dategt],input[data-val-datege],input[data-val-dateeq],input[data-val-datene]")
            .css("ime-mode", "disabled").bind("keypress", function (e) {
                if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 45 && e.keyCode != 47)) { return false; }
            });

        /*有效的数字，小数（0-9，.，+，-）*/
        $("input[data-val-numeric],input[data-val-decimal]").css("ime-mode", "disabled").bind("keypress", function (e) {
            if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 46 && e.keyCode != 43 && e.keyCode != 45)) { return false; }
        });

        //正数，非负数，正小数（0-9，.，+）
        $("input[data-val-posnumeric],input[data-val-nonnegnumeric],input[data-val-posdecimal]")
            .css("ime-mode", "disabled").bind("keypress", function (e) {
                if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 46 && e.keyCode != 43)) { return false; }
            });

        //负数，非正数，负小数（0-9，.，-）
        $("input[data-val-negnumeric],input[data-val-nonposnumeric],input[data-val-negdecimal]")
            .css("ime-mode", "disabled").bind("keypress", function (e) {
                if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 46 && e.keyCode != 45)) { return false; }
            });

        //整数（0-9，+，-）
        $("input[data-val-integer]").css("ime-mode", "disabled").bind("keypress", function (e) {
            if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 43 && e.keyCode != 45)) { return false; }
        });

        //正整数，非负整数（0-9，+）
        $("input[data-val-posinteger],input[data-val-nonneginteger]").css("ime-mode", "disabled").bind("keypress", function (e) {
            if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 43)) { return false; }
        });

        //负整数，非正整数（0-9，-）
        $("input[data-val-neginteger],input[data-val-nonposinteger]").css("ime-mode", "disabled").bind("keypress", function (e) {
            if ((e.keyCode < 48 || e.keyCode > 57) && (e.keyCode != 45)) { return false; }
        });

    }

    $(function () {
        limitInput();
    });

}(jQuery));