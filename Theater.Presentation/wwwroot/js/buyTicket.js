document.addEventListener("DOMContentLoaded", function () {
    // Handle date selection
    document.querySelector(".booking__dates").addEventListener("click", function (event) {
        if (event.target.classList.contains("booking__date")) {
            document.querySelectorAll(".booking__date").forEach((btn) => btn.classList.remove("active"));
            event.target.classList.add("active");
        }
    });

    // Seat selection and price change
    let totalPrice = 0;
    document.addEventListener("click", function (event) {
        if (event.target.classList.contains("seat") && !event.target.classList.contains("reserved")) {
            event.target.classList.toggle("selected");
            if (event.target.classList.contains("selected")) {
                totalPrice += parseFloat(event.target.dataset.price);
            } else {
                totalPrice -= parseFloat(event.target.dataset.price);
            }
            updateTotalPrice(totalPrice);
        }
    });

    function updateTotalPrice(price) {
        document.getElementById("total-price").innerText = `${price}$`;
    }

    // Cancel button functionality
    document.querySelector(".btn--cancel").addEventListener("click", function () {
        window.history.back();
    });
});
