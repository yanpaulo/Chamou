function PlaceViewModel(data) {
    var self = this;
    
    self.id = data.Id;

    self.name = ko.observable(data.Name);

    self.locationPoints = data.LocationPoints ? $.map(data.LocationPoints, function (point) {
        return { latitude: point.Latitude, longitude: point.Longitude };
    }) : [];

    self.centerLatitude = data.CenterLatitude;

    self.centerLongitude = data.CenterLongitude;

    self.locationWellKnownText = null;

    self.mapValidationMessage = ko.observable();

    self.isLocationSet = ko.observable(false);

    self.isLocationSet.subscribe(function (newValue) {
        if (newValue) {
            self.mapValidationMessage(null);
        }
    });

    self.submit = function (formElement) {

        if ($(formElement).valid()) {
            //Separated var because other validation rules may apply.
            var mapValid = true;
            if (!self.isLocationSet()) {
                self.mapValidationMessage('Selecione o local');
                mapValid = false;
            }

            if (mapValid) {
                $.post(formElement.action, self.toJson(), function () {
                    window.location = Places.action('');
                }).error(function (xq) { alert(xq.responseText); });
            }
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