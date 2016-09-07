function PlaceViewModel() {
    var self = this;

    self.name = ko.observable();

    self.locationPoints = [];

    self.centerLatitude = null;

    self.centerLongitude = null;

    self.submit = function () {
        if (self.locationPoints.length > 0) {
            var data = self.toJson();
            $.post(Places.action('Create'), data, function () {
                window.location.href = Places.action('');
            }).error(function (xq) { alert(xq.responseText); });
        }
        else {
            alert("Selecione o local");
        }
    }

    self.toJson = function () {
        return {
            name: self.name(),
            CenterLatitude: self.centerLatitude.toString().replace(".", ","),
            CenterLongitude: self.centerLongitude.toString().replace(".", ","),
            locationPoints: self.locationPoints
        };
    }
}

function GeoLocationPointViewModel(data) {
    var self = this;

    self.latitude = data.latitude;

    self.longitude = data.longitude;
}