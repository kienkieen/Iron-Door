//Thêm sản phẩm
    function addToCart(maSP) {
        fetch('/ShoppingCart/ThemHang?maSP=' + maSP, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/x-www-form-urlencoded',
            },
            body: 'maSP=' + maSP
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert(data.msg);
                    document.getElementById('header__second__cart--notices').innerText = data.count;
                } else {
                    alert(data.msg);
                }
            })
            .catch(error => {
                alert("Có lỗi xảy ra khi thêm sản phẩm!", error );
            });
    }

//
$(document).ready(function () {
    // Khi người dùng nhấn vào icon giỏ hàng
    $('#cart-icon').click(function () {
        $.ajax({
            url: '/ShoppingCart/XemGioHang',  // Đường dẫn đến action XemGioHang
            type: 'POST',  // Phương thức gửi yêu cầu GET
            dataType: 'json',  // Dữ liệu trả về dưới dạng JSON
            success: function (response) {
                if (!response.success) {
                    alert(response.msg); 
                    window.location.href = '/Products/Index';
                }
            },
            error: function (xhr, status, error) {
                // Xử lý lỗi nếu có
                console.error("Có lỗi xảy ra:", error);
            }
        });
    });
});


//Lọc sản phẩm theo giá

$(document).ready(function () {
    // Bắt sự kiện khi người dùng chọn một radio button
    $("input[name='prices']").change(function () {
        var selectedPrice = $("input[name='prices']:checked").val(); // Lấy giá trị của radio button đã chọn

        // Gửi yêu cầu AJAX tới controller
        $.ajax({
            url: '/Products/SanPhamTheoGia',  // Đảm bảo URL chính xác
            type: 'GET',
            data: { prices: selectedPrice },  // Truyền giá trị của radio button đã chọn
            success: function (response) {
                // Đưa danh sách sản phẩm vào phần tử có id="product-list"
                $('#product-list').html(response);
            },
            error: function (error) {
                console.error("Có lỗi xảy ra:", error);
            }
        });
    });
});



// Chỉ được chọn 1 radio trong nhóm giá và loại
$('input[name="prices"]').change(function () {
    $('input[name="loai"]').prop('checked', false);
});

$('input[name="loai"]').change(function () {
    $('input[name="prices"]').prop('checked', false);
});

///Xoá hàng trong giỏ hàng

//Update số lượng của 1 sản phẩm trong giỏ hàng
    function updateQuantity(button, change) {
        var inputField = $(button).closest('.input-group').find('input');
        var currentValue = parseInt(inputField.val());
        var newValue = currentValue + change;

        if (newValue < 1) {
            newValue = 1; // Ngăn không cho số lượng nhỏ hơn 1
        }

        inputField.val(newValue);

        // Lấy ID sản phẩm từ một attribute (giả sử input có attribute data-masp)
        var productId = $(button).closest('.input-group').data('masp');

        // Gửi số lượng mới lên server qua AJAX
        $.ajax({
            url: '/ShoppingCart/UpdateQuantity', // Đường dẫn tới action update số lượng
            type: 'POST',
            data: {
                maSP: productId,
                soLuong: newValue
            },
            success: function (response) {
                console.log('Số lượng đã được cập nhật');
            },
            error: function () {
                console.error('Có lỗi xảy ra khi cập nhật số lượng');
            }
        });
        location.reload();
}

//Input số lượng sản phẩm
function updateQuantityFromInput(input) {
    var newValue = parseInt(input.value);
    if (isNaN(newValue) || newValue < 1) {
        newValue = 1;
        input.value = newValue;
    }

    var productId = $(input).data('masp');

    // Gửi AJAX giống như trong updateQuantity()
    $.ajax({
        url: '/ShoppingCart/UpdateQuantity',
        type: 'POST',
        data: {
            maSP: productId,
            soLuong: newValue
        },
        success: function (response) {
            console.log('Số lượng đã được cập nhật (từ input)');
            location.reload();
        },
        error: function () {
            console.error('Có lỗi xảy ra khi cập nhật số lượng (từ input)');
        }
    });
}



