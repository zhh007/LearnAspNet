/// <reference path="jquery-1.10.2.js" />
/// <reference path="jquery.validate.js" />
/// <reference path="jquery.validate.unobtrusive.js" />

function checkZipcode(value) {
    return /^[1-9][0-9]{5}$/g.test(value);
}

if ($.validator && $.validator.unobtrusive) {
    /*邮编*/
    $.validator.addMethod("zipcode", function (value, element, param) {
        if (value == null || value == '')
            return true;
        return checkZipcode(value);
    });
    $.validator.unobtrusive.adapters.addBool("zipcode");

}