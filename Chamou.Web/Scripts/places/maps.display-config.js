function GetMap() {
    var viewModel = Places.viewModel;
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        credentials: 'ApMPiz4KL7I8sfGR8TJpoRB6AeO3xMUq7cdofyut0FXJv0gWXGCiYbxtGypC0sCq',
        center: new Microsoft.Maps.Location(-3.71841, -38.542881),
        zoom: 12
    });

    map.setView({
        center: new Microsoft.Maps.Location(viewModel.centerLatitude, viewModel.centerLongitude),
        zoom: 18
    });

    map.entities.push(new Microsoft.Maps.Polygon(
        $.map(viewModel.locationPoints, function (p) {
            return new Microsoft.Maps.Location(p.latitude, p.longitude);
        })
    ));
}