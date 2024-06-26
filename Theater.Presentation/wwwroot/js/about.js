document.addEventListener("DOMContentLoaded", function () {
    const tabs = document.querySelectorAll(".tab");
    const tabContents = document.querySelectorAll(".tab-content");

    tabs.forEach((tab) => {
        tab.addEventListener("click", function (e) {
            e.preventDefault();

            // Remove active class from all tabs
            tabs.forEach((t) => t.classList.remove("active"));
            // Add active class to the clicked tab
            this.classList.add("active");

            // Get the target content id
            const target = this.getAttribute("data-tab");

            // Hide all tab contents
            tabContents.forEach((content) => content.classList.remove("active"));

            // Show the target tab content
            document.getElementById(target).classList.add("active");
        });
    });
});
