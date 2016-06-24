function a() {
    //通过身份证验证小于18岁的不得进行开户
    var cardno = $("#CredentialNo").val();
    var birthday = "";
    if (cardno.length == 18) {
        birthday = cardno.substr(6, 4) + "-" + cardno.substr(10, 2) + "-" + cardno.substr(12, 2);
    } else if (cardno.length == 15) {
        birthday = "19" + cardno.substr(6, 2) + "-" + cardno.substr(8, 2) + "-" + cardno.substr(10, 2);
    }
    var bday = new Date(birthday);
    bday.setFullYear(bday.getFullYear() + 18, bday.getMonth(), bday.getDay());
    if (bday > new Date()) {
        errors += "<div class='errormsg'>身份证显示您小于18岁的不得进行开户！</div>";
    }
}