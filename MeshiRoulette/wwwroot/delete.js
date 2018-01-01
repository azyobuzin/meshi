$("#delete-btn").on("click", function () {
    $("#delete-modal")
        .modal({
            selector: {
                approve: ".actions .ok",
                deny: ".actions .cancel"
            },
            onApprove: function () {
                $("#delete-form").submit();
            }
        })
        .modal("show");
});
