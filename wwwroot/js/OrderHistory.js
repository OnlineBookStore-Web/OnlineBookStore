// رسالة تأكيد عند الضغط على Cancel
document.addEventListener('DOMContentLoaded', function () {
    const cancelButtons = document.querySelectorAll('a.btn-danger');
    cancelButtons.forEach(btn => {
        btn.addEventListener('click', function (e) {
            if (!confirm('Are you sure you want to cancel this order?')) {
                e.preventDefault();
            }
        });
    });
});
