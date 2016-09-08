$(function () {
    var json = $("#JsonData");
    var data = json.length ? JSON.parse(json.val()) : {};

    Places.viewModel = new PlaceViewModel(data);

    ko.applyBindings(Places.viewModel);

});