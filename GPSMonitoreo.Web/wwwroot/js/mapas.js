
function AppMap(options)
{
	this.flightPaths = [];
	this.mapLabels = [];
	this.markers = [];
	this.menu = null;
	this.infoWindow = new google.maps.InfoWindow();
	this.cerca_url = null;
	//this.$container = options.$container;
	this.$mapContainer = options.$mapContainer;
	this.map = null;
	this.initialCenter = {lat: -2.1903268, lng: -79.9378324}; //mamut
	this.id = options.id;

	

	this.infoBubble = new InfoBubble({
		shadowStyle: 1,
		padding: 10,
		borderRadius: 5,
		borderWidth: 1,
		borderColor: '#ccc',
		arrowSize: 15,
		disableAutoPan: true,
		arrowPosition: 50,
		backgroundClassName: 'phoney',
		arrowStyle: 0,
		maxWidth: 1000
	});

	this.infoBubble.open();

	//this.infoBubble.e.style.boxSizing = 'content-box';
	this.infoBubble.bubble_.style.boxSizing = 'content-box';
};


AppMap.prototype = {
	hookMouseCursor : function(callback) {
		google.maps.event.addListener(this.map, 'click', function(event) {
			//mouseLocation = event.latLng;
			//console.log(event);
			callback(event);
		});
	}
};

AppMap.prototype.addMarker = function(options, onClick)
{
	options = options || {};
	options.map = this.map;

	var marker = new google.maps.Marker(options);


	if(onClick)
	{
		google.maps.event.addListener(marker, 'click', onClick);
	}


	this.markers.push(marker);


	return marker;

};

AppMap.prototype.initMap = function(mapOptions)
{

	//var $mapContainer;

	//if(this.$mapContainer)
	//{
	//	$mapContainer = this.$mapContainer;
	//}
	//else if(this.id)
	//{
	//	$mapContainer = this.$container.find('#' + this.id);
	//}

	//this.map = new google.maps.Map($mapContainer[0], mapOptions);
	this.map = new google.maps.Map(this.$mapContainer[0], mapOptions);
};

AppMap.prototype.initReportMap = function(){
	var mapOptions = {
		zoom: 13,
		center: this.initialCenter,
		scaleControl: true,
		zoomControl: true
	};
	this.initMap(mapOptions);
};

AppMap.prototype.initMapaCercas = function(options)
{
	var mapOptions = {
		zoom: 13,
		center: this.initialCenter,
		scaleControl: true,
		zoomControl: true
	};


	this.initMap(mapOptions);

	if(options)
	{
		if(options.initEditMenu)
			this.initMenuMapaCercas();

		if(options.flightPathOnClick)
		{
			this._flightPathOnClick = options.flightPathOnClick;
		}
	}
}

AppMap.prototype.showInfoWindow = function(content, position) {
	this.infoWindow.setContent('');
	this.infoWindow.setPosition(position);
	this.infoWindow.open(this.map);
	this.infoWindow.setContent(content);
};

AppMap.prototype.showInfoWindowByUrl = function(url, position) {
	this.infoWindow.setContent('');
	this.infoWindow.setPosition(position);
	this.infoWindow.open(this.map);
	var self = this;
	
	App.getHtml(url, function(response) {
		self.infoWindow.setContent(response);
	});
};


AppMap.prototype.loadInfoWindowContent = function(url) {

};

AppMap.prototype.infoWindowSetContent = function(elementOrHtml) {
	//this.infoWindow.setContent(elementOrHtml);

	this.infoWindow.setOptions({
		maxWidth: 900,
		content: elementOrHtml
	});

};

AppMap.prototype.infoWindowOpen = function(position) {
	//this.infoWindow.setPosition(position);

	this.infoWindow.setOptions({
		//maxWidth: 900,
		position: position
	});

	this.infoWindow.open(this.map);
};



AppMap.prototype.infoBubbleSetContent = function(elementOrHtml) {
	this.infoBubble.setContent(elementOrHtml);
	//to be fixed pending, remove later from open bubble
	//this.infoBubble.content.parentNode.parentNode.style.boxSizing = 'content-box';
	
};

AppMap.prototype.infoBubbleOpen = function(position) {
	this.infoBubble.setPosition(new google.maps.LatLng(position.lat, position.lng));
	this.infoBubble.open(this.map);
	//this.infoBubble.content.parentNode.parentNode.style.boxSizing = 'content-box';
	//console.log('ñññññññ');
	//console.log(this.infoBubble.content.parentNode);
	
};


AppMap.prototype.createInfoWindow = function() {
	var infoWindow = new google.maps.InfoWindow();

	return infoWindow;
}

AppMap.prototype.removeMarkers = function(){
	for(var x = 0; x < this.markers.length; x++)
		this.markers[x].setMap(null);

	this.markers = [];
};


AppMap.prototype.initMenuMapaCercas = function()
{
	var self = this;
	var flighPathMenuOptions = {
		view: function(position, flightPath){
			
			self.infoWindow.setContent('');
			self.infoWindow.setPosition(position);
			self.infoWindow.open(self.map);

			App.getHtml('/geographics/geofences/details/' + flightPath._cercaId, function(response){
				self.infoWindow.setContent(response);
			});
		},

		save: function(flightPath){
			var cercaId = flightPath._cercaId;

			var forma = 0;
			var puntos = [];
			var radio = 0;

			switch(true)
			{
				case flightPath instanceof google.maps.Polygon:
					forma = 3;
					puntos = GMaps.mvcPathsToObjectArray(flightPath.getPaths());
					break;


				case flightPath instanceof google.maps.Rectangle:
					forma = 2;
					var bounds = flightPath.getBounds();
					var southWest = bounds.getSouthWest();
					var northEast = bounds.getNorthEast();
					puntos = [southWest.toJSON(), northEast.toJSON()];
					break;

				case flightPath instanceof google.maps.Circle:
					forma = 1;
					puntos = [flightPath.getCenter().toJSON()];
					radio = flightPath.getRadius();
					break;
			}

			var data = {
				Id: cercaId, 
				ShapeId: forma,
				Coords : puntos,
				CurveRadius : radio
			}

			App.postJson('/geographics/geofences/updatecoords', data);
		}
	};

	$flightPathMenu = this.$mapContainer.parent().find('.googlemaps_flightpathmenu');

	//this.menu = new FlightPathMenu(this.$container.find('#' + this.id + '_flightPathMenu'), this.map, flighPathMenuOptions);
	this.menu = new FlightPathMenu($flightPathMenu, this.map, flighPathMenuOptions);

	this.menu.addMenuItem('Editar cerca', function(menu, currentFlightPath) {
		App.geographics.geofences.edit(currentFlightPath._cercaId);
		menu.close();
	});

	this.menu.addMenuItem('Refrescar cerca', function(menu, currentFlightPath) {
		App.get('/geographics/geofences/coords/' + currentFlightPath._cercaId, function(response) {
			//console.log(response.record);
			//console.log(currentFlightPath);
			//menu.close();
			menu.removeFlightPath(currentFlightPath);
			self.cargarCerca(response.record);
		});
	}, {visibility: 'notediting'});
};


AppMap.prototype.refresh = function(){
	google.maps.event.trigger(this.map, "resize");
	this.map.setCenter(this.initialCenter);
	google.maps.event.trigger(this.map, "resize");
};


AppMap.prototype.flightPathAdded = function(flightPath, cerca_id){
	//google.maps.event.addListener(flightPath, 'click', new function(){
	//	var id = cerca_id;
	//	return function(event){
	//		showInfo(event, id);
	//	};
	//});
	var self = this;

	if(this.menu)
	{
		
		google.maps.event.addListener(flightPath, 'rightclick', function (e) {
			self.menu.open(flightPath, e);
		});
	}


	if(this._flightPathOnClick)
	{
		google.maps.event.addListener(flightPath, 'click', this._flightPathOnClick);
	}



	flightPath._index = this.flightPaths.length;
	flightPath._cercaId = cerca_id;


	this.flightPaths.push(flightPath);
}

AppMap.prototype.ponerCerca = function(cerca, options, label)
{
	var flightPath;
	var center;
	var centerLiteral;
	var coords = cerca.Coords;
	var puntos = coords.Coords;

	if(!this.menu)
	{
		//options.fillColor = 'black';
		options.clickable = false;
	}

	switch(cerca.ShapeId)
	{
		case 1: //circular
			flightPath = GMaps.drawCircle(this.map, coords.CircleCoord, coords.CircleRadius, Object.create(options));
			center = new google.maps.LatLng(coords.CircleCoord);
			centerLiteral = coords.CircleCoord;
			//this.flightPaths.push(flightPath);
			break;


		case 2: //rectangular



		case 3: // poligono
			//console.log(options);
			flightPath = GMaps.drawPolygon(this.map, puntos, Object.create(options));
			centerLiteral = GMaps.getPolygonCenterInside(puntos);
			center = new google.maps.LatLng(centerLiteral);
			break;
	}


	if(label)
	{
		this.mapLabels.push(new MapLabel({
			text: label,
			position: center,
			map: this.map,
			fontSize: 20,
			align: 'left'
		}));	
	
	
	}


	flightPath._options = options;
	this.flightPathAdded(flightPath, cerca.Id);


};

AppMap.prototype.cargarCerca = function(cerca) {
	switch(cerca.LayerId)
	{

		case 1: //en ruta
			color = '#'+Math.floor(Math.random()*16777215).toString(16);;
			break;

		case 2: //paradas operativas
			color = 'green';
			break;

		case 3: //paradas conductor
			color = 'yellow';
			break;


		case 4: //avisos logisticos
			color = 'blue';
			break;

		case 5: //paradas no autorizadas
			color = 'red';
			break;
	}


	options = {
		fillColor: color,
		fillOpacity: 0.5,
		strokeColor: color,
		strokeOpacity: 1.0,
		strokeWeight: 1,
		editable: false,
		draggable: false
	};


	this.ponerCerca(cerca, options);

};

AppMap.prototype.cargarCercasArray = function(cercas)
{
	this.clear();

	var color;
	var cerca;
	var options;

	for(var x = 0; x < cercas.length; x++)
	{
		cerca = cercas[x];
		this.cargarCerca(cerca);
	}

	if(cercas.length)
	{
		//initialMapCenter = cercas[0].vertice2;
		//map_ruta.setCenter(initialMapCenter);
		//self.map.setCenter(cercas[0].vertice2);
		this.initialCenter = cercas[0].vertice2;
	}

	this.refresh();
};

AppMap.prototype.clear = function(){
	while(this.flightPaths.length){
		this.flightPaths.pop().setMap(null);
	}

	while(this.mapLabels.length){
		this.mapLabels.pop().setMap(null);
	}
};

AppMap.prototype.cargarCercas = function(url)
{
	this.clear();

	

	var self = this;

	App.get(url, function(response){
		var cercas = response.Geofences;
		var cercas_otras = response.OtherGeofences;
		var cerca;
		var color;
		var puntos;
		var bounds;
		var flightPath;
		var center;
		var centerLiteral;
		var options = {};

		for(var x = 0; x < cercas.length; x++)
		{
			cerca = cercas[x];
			
			color = '#'+Math.floor(Math.random()*16777215).toString(16);
			options = {
				fillColor: color,
				fillOpacity: 0.5,
				strokeColor: color,
				strokeOpacity: 1.0,
				strokeWeight: 1,
				editable: false,
				draggable: false
			};

			self.ponerCerca(cerca, options, x + 1);
		}

		for(var x = 0; x < cercas_otras.length; x++)
		{
			cerca = cercas_otras[x];
			
			

			switch(cerca.LayerId)
			{
				case 2: //paradas operativas
					color = 'green';
					break;

				case 3: //paradas conductor
					color = 'yellow';
					break;


				case 4: //avisos logisticos
					color = 'blue';
					break;

				case 5: //paradas no autorizadas
					color = 'red';
					break;
			}


			options = {
				fillColor: color,
				fillOpacity: 0.5,
				strokeColor: color,
				strokeOpacity: 1.0,
				strokeWeight: 1,
				editable: false,
				draggable: false
			};


			self.ponerCerca(cerca, options);
		}

		if(cercas.length)
		{
			//initialMapCenter = cercas[0].vertice2;
			//map_ruta.setCenter(initialMapCenter);
			//self.map.setCenter(cercas[0].vertice2);
			self.initialCenter = cercas[0].vertice2;
		}

		self.refresh();

		//google.maps.event.trigger(self.map, "resize");
	}, {async : true});
};

//AppMap.prototype.showInfo = function(event, id)	{
//	this.menu.view('/geografico/cercas/details/' + id, event.latLng);
//}




