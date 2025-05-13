document.addEventListener('DOMContentLoaded', function() {
    // Code cho trang chính
    
    // Xử lý nút chuyển slide carousel
    const carouselBtns = document.querySelectorAll('.carousel-btn');
    if (carouselBtns.length) {
        carouselBtns.forEach(btn => {
            btn.addEventListener('click', function() {
                const container = document.querySelector('.carousel-container');
                const scrollAmount = container.offsetWidth;
                
                if (this.classList.contains('prev')) {
                    container.scrollBy({
                        left: -scrollAmount,
                        behavior: 'smooth'
                    });
                } else {
                    container.scrollBy({
                        left: scrollAmount,
                        behavior: 'smooth'
                    });
                }
            });
        });
    }
    
    // Format tiền tệ
    function formatCurrency(amount) {
        return amount.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".") + " VND";
    }
});