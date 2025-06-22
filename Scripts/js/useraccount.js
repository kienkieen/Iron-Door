/***********Thay đổi nội dung trong thông tin user**************/
$(document).ready(function () {
    $('.menu-chitiet').on('click', function (e) {
        e.preventDefault();

        const url = $(this).data('url'); // Lấy URL từ data-url

        $.ajax({
            url: url,
            type: 'GET',
            success: function (response) {
                // Thay thế nội dung của Partial View số 2 bằng nội dung mới
                $('#partial-container').html(response);
            },
            error: function () {
                alert("Không thể tải nội dung. Vui lòng thử lại!!!!");
            }
        });
    });
});


/*************Cập nhật hồ sơ user******************/
function updateHoSo(button) {
    // Lấy dữ liệu từ form
    var formData = {
        ten: $('#fullname').val(),
        email: $('#email').val(),
        sdt: $('#sdt').val(),
        diachi: $('#diachi').val()
    };

    // Gửi AJAX request
    $.ajax({
        url: '/UserAccount/CapNhatHoSo', // Đường dẫn tới Action Controller
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success) {
                alert(response.message); // Hiển thị thông báo thành công
                $('#hosoForm')[0].reset(); // Reset form nếu cần
                window.location.href = '/UserAccount/ThongTinNguoiDung';
            } else {
                alert(response.message); // Hiển thị thông báo lỗi
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi xử lý yêu cầu.');
        }
    });
}


/****Đổi mật khẩu mới*****/

function updatePassword(button) {
    // Lấy dữ liệu từ form
    var formData = {
        matkhau: $('#mk').val(),
        matkhaumoi: $('#mkm').val(),
        xacnhanmkmoi: $('#xnmkm').val()
    };

    // Gửi AJAX request
    $.ajax({
        url: '/UserAccount/DoiMatKhau', // Đường dẫn tới Action Controller
        type: 'POST',
        data: formData,
        success: function (response) {
            if (response.success) {
                alert(response.message); // Hiển thị thông báo thành công
                $('#passwordForm')[0].reset(); // Reset form nếu cần
                window.location.href = '/UserAccount/ThongTinNguoiDung';
            } else {
                alert(response.message); // Hiển thị thông báo lỗi
            }
        },
        error: function () {
            alert('Có lỗi xảy ra khi xử lý yêu cầu.');
        }
    });
}


/*******Xem chi tiết các đơn hàng**********/
$(document).ready(function () {
    $('.xem-chi-tiet').on('click', function (e) {
        e.preventDefault();

        const url = $(this).data('url'); // Lấy URL từ data-url

        $.ajax({
            url: url,
            type: 'GET',
            success: function (response) {
                // Thay thế nội dung của Partial View số 2 bằng nội dung mới
                $('#noidung').html(response);
            },
            error: function () {
                alert("Không thể tải nội dung. Vui lòng thử lại!");
            }
        });
    });
});

/******Active cho các mục trong thông tin user********/

