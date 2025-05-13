document.addEventListener('DOMContentLoaded', function() {
    // Xử lý nút Grid/List view
    const gridViewBtn = document.querySelector('.grid-view');
    const listViewBtn = document.querySelector('.list-view');
    const tourList = document.querySelector('.tour-list');
    
    if (gridViewBtn && listViewBtn && tourList) {
        // Default là grid view
        tourList.classList.add('grid-layout');
        
        gridViewBtn.addEventListener('click', function() {
            tourList.classList.remove('list-layout');
            tourList.classList.add('grid-layout');
            gridViewBtn.classList.add('active');
            listViewBtn.classList.remove('active');
        });
        
        listViewBtn.addEventListener('click', function() {
            tourList.classList.add('list-layout');
            tourList.classList.remove('grid-layout');
            listViewBtn.classList.add('active');
            gridViewBtn.classList.remove('active');
        });
    }
    
    // Xử lý price range slider
    const priceRange = document.getElementById('price-range');
    const minPrice = document.getElementById('min-price');
    const maxPrice = document.getElementById('max-price');
    
    if (priceRange && minPrice && maxPrice) {
        // Đặt giá trị mặc định
        minPrice.value = "0";
        maxPrice.value = "5000000";
        
        priceRange.addEventListener('input', function() {
            maxPrice.value = this.value;
        });
        
        minPrice.addEventListener('change', function() {
            const value = parseInt(this.value.replace(/,/g, ''));
            if (value >= 0 && value <= parseInt(maxPrice.value.replace(/,/g, ''))) {
                // Cập nhật giá trị hợp lệ
            }
        });
        
        maxPrice.addEventListener('change', function() {
            if (parseInt(this.value) >= parseInt(minPrice.value)) {
                priceRange.value = this.value;
            }
        });
    }
    
    // Xử lý nút "Clear all" filters
    const clearAllBtn = document.querySelector('.clear-all');
    const filterCheckboxes = document.querySelectorAll('.filter-option input[type="checkbox"]');
    
    if (clearAllBtn && filterCheckboxes.length > 0) {
        clearAllBtn.addEventListener('click', function() {
            filterCheckboxes.forEach(checkbox => {
                checkbox.checked = false;
            });
            
            if (priceRange) {
                priceRange.value = "5000000"; 
                minPrice.value = "0";
                maxPrice.value = "5000000";
            }
        });
    }
    
    // Xử lý sort options
    const sortSelect = document.getElementById('sort-by');
    if (sortSelect) {
        sortSelect.addEventListener('change', function() {
            // Thêm logic sắp xếp tour ở đây khi cần
            console.log("Sorting by: " + this.value);
        });
    }
});