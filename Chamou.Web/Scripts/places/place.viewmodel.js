function PlaceViewModel() {
    var self = this;

    self.name = ko.observable();

    self.locationPoints = [];

    self.centerLatitude = null;

    self.centerLongitude = null;

    self.locationWellKnownText = null;

    self.submit = function () {
        if (self.locationPoints.length > 0) {
            $.post(Places.action('Create'), self.toJson(), function () {
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
            centerLatitude: self.centerLatitude.toString().replace(".", ","),
            centerLongitude: self.centerLongitude.toString().replace(".", ","),
            locationWellKnownText: self.locationWellKnownText,
            locationPoints: $.map(self.locationPoints, function (item) {
                return {
                    latitude: item.latitude.toString().replace(".", ","),
                    longitude: item.longitude.toString().replace(".", ",")
                };
            //map callback
            })
        //object
        };
    //function
    }
}

function GeoLocationPointViewModel(data) {
    var self = this;

    self.latitude = data.latitude;

    self.longitude = data.longitude;
}