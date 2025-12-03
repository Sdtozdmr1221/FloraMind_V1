document.addEventListener('DOMContentLoaded', function () {
    const container = document.querySelector('.image_slider');
    const slides = document.querySelectorAll('.slide');
    const prev = document.querySelector('#prev');
    const next = document.querySelector('#next');
    const circles = document.querySelectorAll('.circle');
    let imgCount = 0;

    if (circles.length > 0) {
        circles[0].classList.add('active');
    }

    function updateSlider() {
        const slideWidth = container.offsetWidth; 

        slides.forEach((slide) => {
            slide.style.transform = `translateX(-${slideWidth * imgCount}px)`;
        });

        circles.forEach((circle) => {
            circle.classList.remove('active');
        });

        if (circles[imgCount]) {
            circles[imgCount].classList.add('active');
        }
    }
    function after() {
        if (imgCount >= slides.length - 1) {
            imgCount = 0;
        } else {
            imgCount++;
        }
        updateSlider();
    }
    function before() {
        if (imgCount <= 0) {
            imgCount = slides.length - 1;
        } else {
            imgCount--;
        }
        updateSlider();
    }
    // Event listeners
    if (next) next.addEventListener('click', after);
    if (prev) prev.addEventListener('click', before);

    // Otomatik geçiş
    setInterval(after, 5000);

    // Ekran boyutu değiştiğinde yeniden hesaplama
    let resizeTimer;
    window.addEventListener('resize', function () {
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            updateSlider(); 
        }, 250);
    });

    
    updateSlider();
});