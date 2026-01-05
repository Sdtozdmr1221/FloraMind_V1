function updateCountdowns() {
    document.querySelectorAll('.countdown').forEach(function (element) {
      
        const targetAttr = element.getAttribute('data-target');
        if (!targetAttr) return; 

        const targetDate = new Date(targetAttr).getTime();
        const now = new Date().getTime();
        const distance = targetDate - now;

       
        if (distance < 0) {
            element.innerHTML = "<span style='color: red; font-weight: bold;'>SULAMA ZAMANI! 💧</span>";
            return;
        }

       
        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
        const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        const seconds = Math.floor((distance % (1000 * 60)) / 1000);

        
        if (days > 0) {
            
            element.innerHTML = days + " Gün " + hours + " Sa " + minutes + " Dk";
        } else {
            
            const h = hours < 10 ? "0" + hours : hours;
            const m = minutes < 10 ? "0" + minutes : minutes;
            const s = seconds < 10 ? "0" + seconds : seconds;

            element.innerHTML = h + ":" + m + ":" + s;
        }
    });
}


setInterval(updateCountdowns, 1000);

updateCountdowns();