document.addEventListener("DOMContentLoaded", function () {
    const hall1Images = [
        "../../assets/media/images/first-hall/hall1-1.webp",
        "../../assets/media/images/first-hall/hall1-2.jpg",
    ];
    const hall2Images = [
        "../../assets/media/images/second-hall/hall2-1.jpg",
        "../../assets/media/images/second-hall/hall2-2.jpg",
    ];

    let hall1Index = 0;
    let hall2Index = 0;

    function updateImage(hall, images, index) {
        document.getElementById(hall).src = images[index];
    }

    const hall1PrevButton = document.querySelector(".hall1-prev");
    const hall1NextButton = document.querySelector(".hall1-next");
    const hall2PrevButton = document.querySelector(".hall2-prev");
    const hall2NextButton = document.querySelector(".hall2-next");

    if (
        hall1PrevButton &&
        hall1NextButton &&
        hall2PrevButton &&
        hall2NextButton
    ) {
        hall1PrevButton.addEventListener("click", function () {
            hall1Index = hall1Index > 0 ? hall1Index - 1 : hall1Images.length - 1;
            updateImage("hall1-image", hall1Images, hall1Index);
        });

        hall1NextButton.addEventListener("click", function () {
            hall1Index = hall1Index < hall1Images.length - 1 ? hall1Index + 1 : 0;
            updateImage("hall1-image", hall1Images, hall1Index);
        });

        hall2PrevButton.addEventListener("click", function () {
            hall2Index = hall2Index > 0 ? hall2Index - 1 : hall2Images.length - 1;
            updateImage("hall2-image", hall2Images, hall2Index);
        });

        hall2NextButton.addEventListener("click", function () {
            hall2Index = hall2Index < hall2Images.length - 1 ? hall2Index + 1 : 0;
            updateImage("hall2-image", hall2Images, hall2Index);
        });
    } else {
        console.error("One or more buttons are not found in the DOM.");
    }
});
