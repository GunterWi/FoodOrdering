let list = new Array();
class GioHang {
    constructor(id_sp, sl) {
        this.id_sp = id_sp
        this.sl = sl
    }
    get getID_sp() {
        return this.id_sp
    }
    set setID_sp(i) {
        this.id_sp = i
    }
    get getSL() {
        return this.sl
    }
    set setSL(i) {
        this.sl = i
    }
    toString() {
        return this.id_sp + "|" + this.sl
    }
}

function getGioHang() {
    if (getCookie("GioHang") == null || getCookie("GioHang") == "") return new Array();
    let gioHang = new Array();
    let GioHangs = getCookie("GioHang").split(',');
    GioHangs.forEach(function (item, index) {
        let gh = item.split('|');
        gioHang.push({ id_sp: gh[0], sl: gh[1] })
    })
    return gioHang
}

function setGioHang(gh) {
    if (getCookie("confirm") !== "true") {
        Swal.fire({
            title: 'Hãy cho chúng tôi tiếp cận cookie của bạn?',
            text: 'Chúng tôi muốn phục vụ bạn tốt hơn. Bạn vui lòng cho chúng tôi tiếp cận cookie của bạn',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Đồng ý',
            cancelButtonText: 'Hủy bỏ',
        }).then((result) => {
            if (result.isConfirmed) {
                let str = "";
                gh.forEach(function (item, index) {
                    str += item.id_sp + "|" + item.sl + ","
                })
                setCookie("GioHang", str.substring(0, str.length - 1))
                $("#badge").text(getGioHang().length)
            }
        });
    } else {
        let str = "";
        gh.forEach(function (item, index) {
            str += item.id_sp + "|" + item.sl + ","
        })
        setCookie("GioHang", str.substring(0, str.length - 1))
        $("#badge").text(getGioHang().length)
    }
}

function addGioHang(id_sp, sl) {
    let gioHang = getGioHang()
    let coMA = false
    if (gioHang !== null) {
        gioHang.forEach(function (item, index) {
            if (item.id_sp == id_sp) {
                coMA = true
                item.sl = sl
            }

        })
    }
    if (!coMA) {
        gioHang.push({ id_sp: id_sp, sl: sl })

    }
    setGioHang(gioHang)
}

function increaseGioHang(id_sp) {
    let gioHang = getGioHang()
    let coMA = false
    if (gioHang !== null) {
        // thêm số lượng món ăn
        gioHang.forEach(function (item, index) {
            if (item.id_sp == id_sp) {
                coMA = true
                item.sl++
            }

        })
        /*Swal.fire({
            icon: "success",
            title: "Thành công!",
            text: "Xem chi tiết tại giỏ hàng nhé <3",
            showConfirmButton: false,
            timer: 1500
        });*/
    }
    // nếu mới bắt đầu thêm
    if (!coMA) {
        gioHang.push({ id_sp: id_sp, sl: 1 })
        Swal.fire({
            icon: "success",
            title: "Thành công!",
            text: "Xem chi tiết tại giỏ hàng nhé <3",
            showConfirmButton: false,
            timer: 1500
        });
    }
    setGioHang(gioHang)
}

function reductionGioHang(id_sp) {
    let gioHang = getGioHang()
    if (gioHang !== null) {
        gioHang.forEach(function (item, index) {
            if (item.id_sp == id_sp) {
                coMA = true
                if (item.sl > 0)
                    item.sl--
            }

        })
    }
    setGioHang(gioHang)
}

function dropGioHang(id_sp, callback) {
    Swal.fire({
        title: "Bạn có chắc chắn",
        text: "Xóa món ăn này khỏi giỏ hàng",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy bỏ',
    }).then((result) => {
        if (result.isConfirmed) {
            let gioHang = getGioHang();
            let found = false;
            if (gioHang !== null) {
                gioHang.forEach(function (item, index) {
                    if (item.id_sp == id_sp) {
                        gioHang.splice(index, 1); //  xóa 1 giỏ hàng
                        found = true;
                    }
                });
                if (found) {
                    setGioHang(gioHang);
                    console.log("món ăn đã được xóa, gọi callback"); // Log để kiểm tra
                    callback(); // Gọi callback khi xác nhận xóa
                } else {
                    Swal.fire({
                        title: 'Lỗi',
                        text: 'món ăn không tồn tại trong giỏ hàng.',
                        icon: 'error',
                        confirmButtonText: 'Đóng',
                    });
                }
            }
            else {
                Swal.fire({
                    title: 'Lỗi',
                    text: 'Giỏ hàng trống.',
                    icon: 'error',
                    confirmButtonText: 'Đóng',
                });
            }
        }
    });
}
