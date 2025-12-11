document.addEventListener("DOMContentLoaded", function () {
    const quantityInputs = document.querySelectorAll("input[name='quantity']");

    quantityInputs.forEach(input => {
        input.addEventListener("change", function () {
            const form = input.closest("form");
            form.submit(); // هيرسل الفورم للـ controller
        });
    });
});
