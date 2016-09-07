var Places = {
    viewModel: null,
    action: function (name) {
        return '/Places/' + name;
    }
};

function GetMap() {
    var map = new Microsoft.Maps.Map(document.getElementById('myMap'), {
        credentials: 'ApMPiz4KL7I8sfGR8TJpoRB6AeO3xMUq7cdofyut0FXJv0gWXGCiYbxtGypC0sCq',
        center: new Microsoft.Maps.Location(-3.71841, -38.542881),
        zoom: 12
    });
    Microsoft.Maps.loadModule(['Microsoft.Maps.SpatialMath', 'Microsoft.Maps.DrawingTools', 'Microsoft.Maps.WellKnownText'], function () {
        var tools = new Microsoft.Maps.DrawingTools(map);
        tools.showDrawingManager(function (manager) {

            manager.drawingStarted.add(function () { });
            manager.drawingEnded.add(function () {

                var list = manager.getPrimitives();
                var item = list[list.length - 1];
                if (item instanceof Microsoft.Maps.Polygon) {
                    if (list.length > 1) {
                        manager.setPrimitives([item]);
                    }
                    
                    var locations = item.getLocations();
                    locations.push(locations[0]);
                    item.setLocations(locations);
                    var viewModel = Places.viewModel;
                    var centroid = Microsoft.Maps.SpatialMath.Geometry.centroid(new Microsoft.Maps.Polygon(item.getLocations()));
                    
                    viewModel.locationPoints = [];
                    viewModel.centerLatitude = centroid.latitude;
                    viewModel.centerLongitude = centroid.longitude;
                    
                    viewModel.locationWellKnownText = Microsoft.Maps.WellKnownText.write(item);

                    for (var i = 0; i < locations.length; i++) {
                        viewModel.locationPoints.push(new GeoLocationPointViewModel(locations[i]));
                    }
                }
                else {
                    manager.remove(item);
                }
            });
            manager.drawingErased.add(function () { });
        });
    });

    Microsoft.Maps.loadModule('Microsoft.Maps.AutoSuggest', function () {
        var options = {
            maxResults: 4,
            map: map
        };
        var manager = new Microsoft.Maps.AutosuggestManager(options);
        manager.attachAutosuggest('#searchBox', '#searchBoxContainer', selectedSuggestion);
    });
    function selectedSuggestion(suggestionResult) {
        map.setView({ bounds: suggestionResult.bestView });
    }

}

$(function () {
    Places.viewModel = new PlaceViewModel();

    ko.applyBindings(Places.viewModel);
    
});