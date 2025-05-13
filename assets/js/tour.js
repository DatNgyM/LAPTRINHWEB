document.addEventListener('DOMContentLoaded', function() {
    // Code cho trang tour-detail
    const quantityInput = document.getElementById('quantity-input');
    const totalPriceElement = document.querySelector('.total-price .price');
    const totalPriceInput = document.getElementById('total-price-input');
    const pricePerPerson = 2990000; // 2.990.000 VND
    
    // Xử lý nút tăng/giảm số lượng
    if (quantityInput) {
        // Cập nhật tổng tiền khi số lượng thay đổi
        function updateTotalPrice() {
            const quantity = parseInt(quantityInput.value);
            const total = pricePerPerson * quantity;
            totalPriceElement.textContent = formatCurrency(total);
            totalPriceInput.value = total; // Cập nhật giá trị input hidden
        }
        
        // Xử lý nút tăng số lượng
        const plusBtn = document.querySelector('.qty-btn.plus');
        if (plusBtn) {
            plusBtn.addEventListener('click', function() {
                quantityInput.value = Math.min(parseInt(quantityInput.value) + 1, 20);
                updateTotalPrice();
            });
        }
        
        // Xử lý nút giảm số lượng
        const minusBtn = document.querySelector('.qty-btn.minus');
        if (minusBtn) {
            minusBtn.addEventListener('click', function() {
                quantityInput.value = Math.max(parseInt(quantityInput.value) - 1, 1);
                updateTotalPrice();
            });
        }
        
        // Xử lý khi người dùng trực tiếp thay đổi số lượng
        quantityInput.addEventListener('change', function() {
            const value = parseInt(this.value);
            if (isNaN(value) || value < 1) {
                this.value = 1;
            } else if (value > 20) {
                this.value = 20;
            }
            updateTotalPrice();
        });
        
        // Thiết lập ngày tối thiểu cho ngày khởi hành (ngày hôm nay)
        const departureDateInput = document.getElementById('departure-date');
        if (departureDateInput) {
            const today = new Date();
            const tomorrow = new Date(today);
            tomorrow.setDate(tomorrow.getDate() + 1);
            
            // Format yyyy-mm-dd
            const formattedDate = tomorrow.toISOString().split('T')[0];
            departureDateInput.setAttribute('min', formattedDate);
        }
        
        // Khởi tạo giá ban đầu
        updateTotalPrice();
    }
    
    // Xử lý nút "Đặt ngay"
    const bookNowBtn = document.querySelector('.btn-book-now');
    if (bookNowBtn) {
        bookNowBtn.addEventListener('click', function() {
            // Cuộn trang xuống phần đặt tour
            const bookingForm = document.querySelector('.tour-booking-summary');
            if (bookingForm) {
                bookingForm.scrollIntoView({ behavior: 'smooth' });
            }
        });
    }
    
    // Xử lý tab các phần chi tiết tour
    const tabBtns = document.querySelectorAll('.tab-btn');
    if (tabBtns.length > 0) {
        tabBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                // Loại bỏ active từ tất cả các tab và nội dung
                document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
                document.querySelectorAll('.tab-pane').forEach(p => p.classList.remove('active'));
                
                // Thêm active cho tab được click
                this.classList.add('active');
                
                // Hiển thị nội dung tương ứng
                const tabId = this.getAttribute('data-tab');
                const tabContent = document.getElementById(tabId);
                if (tabContent) {
                    tabContent.classList.add('active');
                }
            });
        });
    }
});

// Hàm định dạng tiền tệ
function formatCurrency(amount) {
    return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + " VND";
}