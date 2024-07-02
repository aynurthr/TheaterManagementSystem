(function ($) {

    $.newid = () => "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c => (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16));

    $.fn.singleImageChooser = function () {

        this.each((index, element) => {
            element.id = `ch-${$.newid()}`;
            const imageUrl = $(element).data('image');
            $(element).removeAttr('data-image');

            const reader = new FileReader();
            const label = $('<label/>', {
                class: 'file-choose empty',
                for: element.id
            });

            if (imageUrl) {
                $(label).removeClass('empty').css('background-image', `url(${imageUrl})`)
            }

            reader.addEventListener("load", () => $(label).removeClass('empty').css('background-image', `url(${reader.result})`), false);

            $(element).on('change', function (e) {
                reader.readAsDataURL(e.currentTarget.files[0]);
            });

            $(label).insertBefore(element);
        });
    }


})(jQuery);