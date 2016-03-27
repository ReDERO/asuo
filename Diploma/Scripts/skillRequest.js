function request(uri) {

    $.ajax({
        url: uri,
        type: "GET",
        async: false,
        cache: false
    }).done(function (data) {
        $("#caption-search-button").click();
    });
}