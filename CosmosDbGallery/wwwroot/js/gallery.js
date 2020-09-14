$(function () {
    $(".like").click(function () {
        var col = $(this).parent();
        var imageId = col.find('#imageId').val();
        $.ajax({
            dataType: "json",
            data: {
                imageId: imageId
            },
            url: "/Gallery/AddLike",
            success: function (data) {
                window.location.href = "/Gallery/Index";
            },
            error: function (jqXHR, exception) {
                alert("Sorry, there has been an error");
            }
        });
    });
});