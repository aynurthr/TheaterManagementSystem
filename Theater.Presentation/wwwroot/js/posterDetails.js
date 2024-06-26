document.addEventListener("DOMContentLoaded", function () {
    // Tab functionality
    const tabs = document.querySelectorAll(".tab");
    const tabContents = document.querySelectorAll(".tab-content");

    tabs.forEach((tab) => {
        tab.addEventListener("click", function (event) {
            event.preventDefault();
            const target = this.getAttribute("data-tab");

            tabs.forEach((t) => t.classList.remove("active"));
            tabContents.forEach((content) => (content.style.display = "none"));

            this.classList.add("active");
            document.getElementById(target).style.display = "block";
        });
    });

    tabs[0].classList.add("active");
    tabContents[0].style.display = "block";

    // Toggle eye icon logic
    const toggleIcon = document.getElementById("toggle-icon");
    let isIconActive = false;
    toggleIcon.addEventListener("click", function () {
        isIconActive = !isIconActive;
        if (!isIconActive) {
            currentRating = 0;
            resetStars();
        }
        updateEyeIcon();
        highlightStars(currentRating);
    });

    // Star rating logic
    const stars = document.querySelectorAll(".star-rating i");
    let currentRating = 0;

    stars.forEach((star) => {
        star.addEventListener("mouseover", function () {
            resetStars();
            highlightStars(star.getAttribute("data-value"));
        });

        star.addEventListener("mouseout", function () {
            resetStars();
            highlightStars(currentRating);
        });

        star.addEventListener("click", function () {
            const rating = star.getAttribute("data-value");
            if (currentRating == rating) {
                currentRating = 0;
                isIconActive = false;
            } else {
                currentRating = rating;
                isIconActive = true;
            }
            highlightStars(currentRating);
            updateEyeIcon();
        });
    });

    function resetStars() {
        stars.forEach((star) => {
            star.classList.remove("fa-solid");
            star.classList.add("fa-regular");
            star.style.color = "#282828";
        });
    }

    function highlightStars(rating) {
        stars.forEach((star) => {
            if (star.getAttribute("data-value") <= rating) {
                star.classList.remove("fa-regular");
                star.classList.add("fa-solid");
                star.style.color = "#FFD43B";
            }
        });
    }

    function updateEyeIcon() {
        if (isIconActive) {
            toggleIcon.innerHTML =
                '<i class="fa-solid fa-eye" style="color: #fa434b;"></i>';
        } else {
            toggleIcon.innerHTML =
                '<i class="fa-regular fa-eye" style="color: #282828"></i>';
        }
    }
});

