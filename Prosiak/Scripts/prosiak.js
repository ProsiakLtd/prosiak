$(function () {

    var getPage = function () {
        var $a = $(this);

        var options = {
            url: $a.attr("href"),
            //data: $("form").serialize(),
            type: "get"
        };

        $.ajax(options).done(function (data) {
            var $target = $a.parents("div.pagedList").attr("data-prosiak-target");
            $($target).replaceWith(data);
        });
        return false; //prevents redrawing the page?
    };

    var submitAutocompleteForm = function (event, ui) {

        var $input = $(this);
        $input.val(ui.item.label); //set value by hand

        var $form = $input.parents("form:first");
        $form.submit();
    }

    var createAutocomplete = function () {
        var $input = $(this);

        var options = {
            source: $input.attr("data-prosiak-autocomplete"),
            select: submitAutocompleteForm
        };

        $input.autocomplete(options);

    };

    var postAndReplaceButton = function () {
        var $input = $(this);
        var $form = $input.parents("form:first");
        var token = $('input[name="__RequestVerificationToken"]', $form).val();
        var data = {
            bookId: $input.attr("data-prosiak-btn-id"),
            __RequestVerificationToken: token
        };

        var options = {
            type: "POST",
            data: data
        };

        $.ajax(options).done(function (result) {
            //var $target = $input.parents("td");
            $($input).replaceWith(result);
        });

        return false;
    }

    $(".body-content").on("click", ".pagedList a", getPage);
    $(".body-content").on("click", "input[data-prosiak-btn-id]", postAndReplaceButton);
    //$(".body-content").on("click", "input[data-prosiak-fetch]", postAndReplaceButton);
    $("input[data-prosiak-autocomplete]").each(createAutocomplete);
});