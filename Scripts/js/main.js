(function ($) {
    "use strict";
    
    // Dropdown on mouse hover
    $(document).ready(function () {
        function toggleNavbarMethod() {
            if ($(window).width() > 992) {
                $('.navbar .dropdown').on('mouseover', function () {
                    $('.dropdown-toggle', this).trigger('click');
                }).on('mouseout', function () {
                    $('.dropdown-toggle', this).trigger('click').blur();
                });
            } else {
                $('.navbar .dropdown').off('mouseover').off('mouseout');
            }
        }
        toggleNavbarMethod();
        $(window).resize(toggleNavbarMethod);
    });


    // Date and time picker
    $('.date').datetimepicker({
        format: 'L'
    });
    $('.time').datetimepicker({
        format: 'LT'
    });
    
    
    // Back to top button
    $(window).scroll(function () {
        if ($(this).scrollTop() > 100) {
            $('.back-to-top').fadeIn('slow');
        } else {
            $('.back-to-top').fadeOut('slow');
        }
    });
    $('.back-to-top').click(function () {
        $('html, body').animate({scrollTop: 0}, 1500, 'easeInOutExpo');
        return false;
    });


    // Team carousel
    $(".team-carousel, .related-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1000,
        center: true,
        margin: 30,
        dots: false,
        loop: true,
        nav : true,
        navText : [
            '<i class="fa fa-angle-left" aria-hidden="true"></i>',
            '<i class="fa fa-angle-right" aria-hidden="true"></i>'
        ],
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            }
        }
    });


    // Testimonials carousel
    $(".testimonial-carousel").owlCarousel({
        autoplay: true,
        smartSpeed: 1500,
        margin: 30,
        dots: true,
        loop: true,
        center: true,
        responsive: {
            0:{
                items:1
            },
            576:{
                items:1
            },
            768:{
                items:2
            },
            992:{
                items:3
            }
        }
    });


    // Vendor carousel
    $('.vendor-carousel').owlCarousel({
        loop: true,
        margin: 30,
        dots: true,
        loop: true,
        center: true,
        autoplay: true,
        smartSpeed: 1000,
        responsive: {
            0:{
                items:2
            },
            576:{
                items:3
            },
            768:{
                items:4
            },
            992:{
                items:5
            },
            1200:{
                items:6
            }
        }
    });
    
})(jQuery);


//

document.querySelectorAll('.accordion-button').forEach(button => {
    button.addEventListener('click', function () {
        // Kiểm tra trạng thái hiện tại của 'collapsed'
        const isCollapsed = this.classList.contains('collapsed');

        // Toggle trạng thái 'collapsed'
        if (isCollapsed) {
            this.classList.remove('collapsed');
        } else {
            this.classList.add('collapsed');
        }

        // Điều chỉnh thuộc tính aria-expanded
        const isExpanded = this.getAttribute('aria-expanded') === 'true';
        this.setAttribute('aria-expanded', !isExpanded);

        // Tìm accordion body liên quan
        const accordionBody = this.closest('.accordion-item').querySelector('.accordion-collapse');

        // Toggle class 'show' để hiển thị/ẩn accordion-body
        accordionBody.classList.toggle('show');
    });
});
function toggleDropdown(event) {
    event.preventDefault();
    const dropdown = document.getElementById('accountDropdown');
    const selection = dropdown.nextElementSibling; // Lấy phần tử dropdown_selection ngay sau <a>
    selection.style.visibility = selection.style.visibility === 'visible' ? 'hidden' : 'visible';
    selection.style.opacity = selection.style.opacity === '1' ? '0' : '1';
}

// Đóng dropdown khi nhấp ra ngoài
document.addEventListener('click', function (event) {
    const dropdown = document.getElementById('accountDropdown');
    const selection = dropdown.nextElementSibling;
    if (!dropdown.contains(event.target) && !selection.contains(event.target)) {
        selection.style.visibility = 'hidden';
        selection.style.opacity = '0';
    }
});



//Shopping cart

