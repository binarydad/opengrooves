<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OpenGrooves.Core.Location>" %>

<meta name="viewport" content="initial-scale=1.0, user-scalable=no" />
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=true"></script>

<div id="map_canvas" style="width: 100%; height: 500px;"></div>

<script type="text/javascript">

    function initialize(lat, long) {
        var latlng = new google.maps.LatLng(lat, long);
        var myOptions = {
            zoom: 8,
            center: latlng,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };

        var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);

        var marker = new google.maps.Marker({
            position: latlng, 
            map: map, 
            title:"Hello World!"
        }); 
    }

    initialize(<%: Model.Coordinate.Latitude %>, <%: Model.Coordinate.Longitude %>);
</script>

