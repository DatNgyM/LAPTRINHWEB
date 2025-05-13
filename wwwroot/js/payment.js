document.addEventListener('DOMContentLoaded', function() {
    // Đảm bảo tiêu đề hiển thị đúng
    const paymentTitle = document.querySelector('.payment-title');
    if (paymentTitle) {
        paymentTitle.style.color = '#333';
        paymentTitle.style.fontSize = '32px';
        paymentTitle.style.fontWeight = '700';
        paymentTitle.style.opacity = '1';
        paymentTitle.style.textShadow = 'none';
    }
    
    // Lấy các thông tin từ URL (được gửi từ form đặt tour)
    const urlParams = new URLSearchParams(window.location.search);
    
    // Cập nhật thông tin hiển thị
    const tourName = urlParams.get('tour_name') || 'Đà Lạt - Thành phố ngàn hoa';
    const tourId = urlParams.get('tour_id') || 'DLAT0325';
    const departureDate = urlParams.get('departure_date') || '2025-06-01';
    const quantity = parseInt(urlParams.get('quantity')) || 1;
    const customerName = urlParams.get('customer_name') || 'Nguyễn Văn A';
    const customerPhone = urlParams.get('phone') || '0901234567';
    const customerEmail = urlParams.get('email') || 'email@example.com';
    
    // Tính toán giá đúng dựa trên số lượng người
    const pricePerPerson = 2990000; // Giá tiền mỗi người
    const totalPrice = pricePerPerson * quantity;
    
    // Format ngày
    const formattedDate = new Date(departureDate).toLocaleDateString('vi-VN');
    
    // Cập nhật thông tin hiển thị
    document.getElementById('tour-name').textContent = tourName;
    if (document.getElementById('tour-name-breadcrumb')) {
        document.getElementById('tour-name-breadcrumb').textContent = tourName;
    }
    document.getElementById('departure-date-display').textContent = formattedDate;
    document.getElementById('quantity-display').textContent = quantity;
    document.getElementById('customer-name-display').textContent = customerName;
    document.getElementById('customer-phone-display').textContent = customerPhone;
    document.getElementById('customer-email-display').textContent = customerEmail;
    
    // Cập nhật thông tin giá
    document.getElementById('price-per-person').textContent = formatCurrency(pricePerPerson);
    document.getElementById('quantity-summary').textContent = `${quantity} người`;
    document.getElementById('total-price-summary').textContent = formatCurrency(totalPrice);
    
    // Tạo mã thanh toán
    const paymentReference = `${tourId}-${customerName.toUpperCase().replace(/\s/g, '')}`;
    document.getElementById('payment-reference').textContent = paymentReference;
    if (document.getElementById('momo-reference')) {
        document.getElementById('momo-reference').textContent = paymentReference;
    }
    
    // Cải thiện xử lý hiển thị phương thức thanh toán
    const paymentOptions = document.querySelectorAll('.payment-option');
    
    // Hiển thị mặc định phương thức đầu tiên
    const defaultMethod = document.querySelector('input[name="payment_method"]:checked');
    if (defaultMethod) {
        const detailsElement = defaultMethod.parentElement.querySelector('.payment-details');
        if (detailsElement) {
            detailsElement.style.display = 'block';
        }
    }
    
    // Xử lý sự kiện click vào phương thức thanh toán
    paymentOptions.forEach(option => {
        const radio = option.querySelector('input[type="radio"]');
        const label = option.querySelector('.payment-label');
        const details = option.querySelector('.payment-details');
        
        // Click vào label hoặc toàn bộ option
        option.addEventListener('click', function() {
            // Đảm bảo radio button được chọn
            radio.checked = true;
            
            // Ẩn tất cả các chi tiết
            document.querySelectorAll('.payment-details').forEach(detail => {
                detail.style.display = 'none';
            });
            
            // Hiển thị chi tiết của phương thức được chọn
            if (details) {
                details.style.display = 'block';
            }
        });
    });
});

// Hàm xử lý submit form
function submitPaymentForm(event) {
    if (event) event.preventDefault();
    
    // Kiểm tra checkbox đã được chọn chưa
    const termsAgreement = document.getElementById('terms-agreement');
    if (!termsAgreement.checked) {
        alert('Vui lòng đồng ý với điều khoản đặt tour và chính sách hủy tour');
        return false;
    }
    
    // Chuyển hướng đến trang success
    window.location.href = 'booking-success.html';
    return false;
}

// Hàm định dạng tiền tệ
function formatCurrency(amount) {
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + " VND";
}