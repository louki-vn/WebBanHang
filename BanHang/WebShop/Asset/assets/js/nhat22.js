function Add_To_Cart_Click(a, b) {
    if (String(a) == "1") {
        $.ajax({
            type: "GET",
            data: { id: b },
            url: "/ProductSales/Add_To_CartAjax",
            success: function (e) {
                alert("Thêm vào giỏ hàng thành công!!");
            }
        })
        alert("Thêm vào giỏ hàng thành công!!");
    }
    else {
        alert("Bạn chưa đăng nhập!!");
    }
}

function Add_To_Cart_Click2(a) {
    if (String(a) == "1") {
        alert("Thêm vào giỏ hàng thành công!!");
    }
    else {
        alert("Bạn chưa đăng nhập !!");
    }
}

function Update_Member_Information(id) {
    var e1 = document.getElementById("member_name_id");
    var e2 = document.getElementById("member_phone_id");
    var e3 = document.getElementById("member_address_id");
    var name = e1.value;
    var phone = e2.value;
    var address = e3.value;
    $.ajax({
        type: "GET",
        data: {
            member_id: id, name: name,phone: phone, address: address
        },
        url: "/UserInformation/update_information",
        success: function (e) {
            alert("Cập nhật thông tin thành công!!")
        }
    })
}
