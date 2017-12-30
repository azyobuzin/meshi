$("#username").popup({
    popup: "#logout-popup",
    on: "click"
});

$("#logout-link").on("click", function () {
    $("#logout-form").submit();
    return false;
});

$('.ui.radio.checkbox').checkbox();
