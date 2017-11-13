var GMaps = {
	defaults: {
		polylineOptions: {
			strokeColor: '#FF0000',
			strokeOpacity: 1.0,
			strokeWeight: 2,
			clickable: true,
			editable: true,
			draggable: true
		},
		polygonOptions: {
			fillColor: '#FF0000',
			fillOpacity: 0.5,
			strokeColor: '#FF0000',
			strokeOpacity: 1.0,
			strokeWeight: 2,
			clickable: true,
			editable: true,
			draggable: true
		},
		rectangleOptions: {
			fillColor: '#FF0000',
			fillOpacity: 0.5,
			strokeColor: '#FF0000',
			strokeOpacity: 1.0,
			strokeWeight: 2,
			clickable: true,
			editable: true,
			draggable: true
		},
		circleOptions: {
			fillColor: '#FF0000',
			fillOpacity: 0.5,
			strokeColor: '#FF0000',
			strokeOpacity: 1.0,
			strokeWeight: 2,
			clickable: true,
			editable: true,
			draggable: true
		},

	},

	mvcPathsToArray: function(mvcArray)
	{
		var arr = [];
		mvcArray.getArray()[0].forEach(function (ite) { arr.push(ite) });
		return arr;
	},

	mvcPathsToObjectArray: function(mvcArray)
	{
		var arr = [];
		mvcArray.getArray()[0].forEach(function (ite) { arr.push({lat: ite.lat(), lng: ite.lng()})});
		return arr;
	},

	drawPolyline: function(map, point1, point2)
	{

		var flightPath = new google.maps.Polyline($.extend({
        	path: [point1, point2],
			map: map
		}, this.defaults.polylineOptions));

		return flightPath;
	},

	drawPolygon: function(map, pts, options)
	{
		options = options || this.defaults.polygonOptions;

		var final_options = $.extend({
			path: pts,
			map: map
		}, options);
		var flightPath = new google.maps.Polygon(final_options);

        return flightPath;
	},

	drawCircle: function(map, point, radius, options)
	{
		options = options || this.defaults.circleOptions;
		var flightPath = new google.maps.Circle($.extend({
			center: point,
			radius: radius,
			map: map
		}, options));

        return flightPath;
	},

	drawRectangle: function(map, bounds)
	{
		var flightPath = new google.maps.Rectangle($.extend({
			bounds: bounds,
			map: map
		}, this.defaults.rectangleOptions));

        return flightPath;
	
	},

	intersect: function(p0,p1,p2,p3)
	{
		// this function computes the intersection of the sent lines p0-p1 and p2-p3
		// and returns the intersection point,

		var a1,b1,c1, // constants of linear equations
			a2,b2,c2,
			det_inv,  // the inverse of the determinant of the coefficient matrix
			m1,m2;    // the slopes of each line

		var x0 = p0.x;
		var y0 = p0.y;
		var x1 = p1.x;
		var y1 = p1.y;
		var x2 = p2.x;
		var y2 = p2.y;
		var x3 = p3.x;
		var y3 = p3.y;

		// compute slopes, note the cludge for infinity, however, this will
		// be close enough

		if ((x1-x0)!==0)
			m1 = (y1-y0)/(x1-x0);
		else
			m1 = 1e+10;   // close enough to infinity

		if ((x3-x2)!==0)
			m2 = (y3-y2)/(x3-x2);
		else
			m2 = 1e+10;   // close enough to infinity

		// compute constants

		a1 = m1;
		a2 = m2;

		b1 = -1;
		b2 = -1;

		c1 = (y0-m1*x0);
		c2 = (y2-m2*x2);

		// compute the inverse of the determinate

		det_inv = 1/(a1*b2 - a2*b1);

		// use Kramers rule to compute xi and yi

		var xi=((b1*c2 - b2*c1)*det_inv);
		var yi=((a2*c1 - a1*c2)*det_inv);

		return new google.maps.Point(Math.round(xi),Math.round(yi));

	},

	latLngToPoint : function(map, latlng, z){
		var normalizedPoint = map.getProjection().fromLatLngToPoint(latlng); // returns x,y normalized to 0~255
		var scale = 1;
		var pixelCoordinate = new google.maps.Point(normalizedPoint.x * scale, normalizedPoint.y * scale);
		return pixelCoordinate;
	},

	pointToLatlng : function(map, point, z){
		var scale = 1;
		var normalizedPoint = new google.maps.Point(point.x / scale, point.y / scale);
		var latlng = map.getProjection().fromPointToLatLng(normalizedPoint);
		return latlng;
	},

	createOffsetEdge: function(edge, dx, dy)
	{
		return {
			vertex1: {x: edge.vertex1.x + dx, y: edge.vertex1.y + dy},
			vertex2: {x: edge.vertex2.x + dx, y: edge.vertex2.y + dy}
		};
	},


	initResizerPanel: function(map, menu){
		$resizerPanel = $('<div class="googlemaps_floatingpanel right" style="top:80px;width:240px;height:65px;"><div></div></div>');
		map.$container.append($resizerPanel);

		var $slider = $resizerPanel.children();

		$slider.jqxSlider({
			width: 235,
			height: 65,
			template: 'info',
			min:-50,
			max:50,
			
			showMinorTicks: true, 
			
			minorTickSize: 4,
			ticksFrequency: 10,
			minorTicksFrequency: 5,
			step: 5,
			showTickLabels: true,
			tooltip: true, 
			mode: 'fixed',
			showRange: false,
			showButtons: false,
			ticksPosition: 'both'
		});

		var resizingPolygon = null;
		var projection = null;

		var slider = $slider.data('jqxWidget');

		//console.log(slider);

		//var seen = [];

		//console.log(JSON.stringify(slider, function(key, val) {
		//	if (val != null && typeof val == "object") {
		//		if (seen.indexOf(val) >= 0) {
		//			return;
		//		}
		//		seen.push(val);
		//	}
		//	return val;
		//}));

		seen = [];

		//JSON.stringify(dropClasses(slider));

		var isSliding = false;
		var avoidEvent = false;
		var clipperScale = 10000000000000;
		var clipperOffset = new ClipperLib.ClipperOffset(100, 0.25);

		var clipperSolution = new ClipperLib.Paths();
		

		var polygonOptions = $.extend({}, this.defaults.polygonOptions);
		polygonOptions.editable = false;
		polygonOptions.draggable = false;


		var circleOptions = $.extend({}, this.defaults.circleOptions);
		circleOptions.editable = false;
		circleOptions.draggable = false;

		var rectangleOptions = $.extend({}, this.defaults.rectangleOptions);
		circleOptions.editable = false;
		circleOptions.draggable = false;

		var slidingFlightPath = null;


		function finishSliding(){
			
			//console.log('slide end');
			
			isSliding = false;
			

			var val = slider.getValue();
			var direction = val > 0 ? -1 : 1;

			if(val != 0)
			{
				avoidEvent = true;
				var interval = setInterval(function(){
					//console.log('finished');
					val += direction;

					slider.setValue(val);

					if(val == 0)
					{
						clearInterval(interval);
						avoidEvent = false;
						//console.log('interval finished');
					}
				}, 100 / Math.abs(val));
			
			}


			
			//slider._refresh();
			//console.log(slider);
			//slider.decrementValue();
			//slider.decrementValue();
			//slider.decrementValue();
			//slider.decrementValue();
			//slider.decrementValue();
			//$slider.unbind('slideStart');
			//$slider.on('slideStart', function(evt){
			//	console.log('slideStart');
			//});

			//slider.disable();
			//slider.enable();

			//slider.val(0);
			//slider.disable();
			//slider.enable();
			
			//avoidEvent = false;



			var flightPath = map._currentFlightPath;

			switch(true)
			{
				case flightPath instanceof google.maps.Polygon:
				case flightPath instanceof google.maps.Polyline:
					flightPath.setOptions(GMaps.defaults.polygonOptions);
					google.maps.event.addListener(flightPath, 'rightclick', function (e) {
						menu.open(flightPath, e);
					});		
					break;

				case flightPath instanceof google.maps.Circle:
					flightPath.setOptions(GMaps.defaults.circleOptions);
					break;

				case flightPath instanceof google.maps.Rectangle:
					flightPath.setOptions(GMaps.defaults.rectangleOptions);
					break;
			}
		};

		function prepareSliding(){
			clipperOffset.Clear();
			projection = map.getProjection();
			//console.log('prepareSliding');
			//var coords = GMaps.mvcPathsToArray(map._currentFlightPath.getPaths());

			var flightPath = map._currentFlightPath;

			var x;
				
			switch(true)
			{
				case flightPath instanceof google.maps.Polygon:
				case flightPath instanceof google.maps.Polyline:
					var coords = flightPath.getPath().getArray();
					//projection = map.getProjection();
					var polygonPoints = [];
					var pt;

					for(x = 0; x < coords.length; x++)
					{
						pt = projection.fromLatLngToPoint(coords[x]);
						polygonPoints.push({X: pt.x * clipperScale, Y: pt.y  * clipperScale});
					}

					if(flightPath instanceof google.maps.Polyline)
					{
						for(x = coords.length - 2; x > 0; x--)
						{
							pt = projection.fromLatLngToPoint(coords[x]);
							polygonPoints.push({X: pt.x * clipperScale, Y: pt.y  * clipperScale});
						}
					}


					clipperOffset.AddPath(polygonPoints, ClipperLib.JoinType.jtMiter, flightPath instanceof google.maps.Polyline ? ClipperLib.EndType.etClosedLine : ClipperLib.EndType.etClosedPolygon);
					

					break;

				case flightPath instanceof google.maps.Circle:
					flightPath._originalRadius = flightPath.getRadius();
					flightPath.setOptions(circleOptions);
					break;


				case flightPath instanceof google.maps.Rectangle:
					flightPath._originalBounds = flightPath.getBounds();
					flightPath.setOptions(rectangleOptions);
					break;
			}
		};


		$slider.on('slideStart', function(evt){
			//console.log('slideStart');
			//return;
			prepareSliding();

			isSliding = true;
		});

		//$slider.on('change', function(evt){
		//	console.log('change');
		//});
		

		$slider.on('slide', function(evt){
			
			//console.log('slide');
			//return;
			
			if(avoidEvent)
				return;

			if(!isSliding)
			{
				prepareSliding();
			}

			var flightPath = map._currentFlightPath;

			var offset;

			switch(true)
			{
				case flightPath instanceof google.maps.Polygon:
				case flightPath instanceof google.maps.Polyline:
					offset = 360 / (2 * Math.PI * 6378100) * evt.args.value;
					clipperOffset.Execute(clipperSolution, offset * clipperScale);

					var paths = [];
					var clipperPoints = clipperSolution[0];

					if(clipperPoints)
					{
						var clipperPoint;

						for(var x = 0; x < clipperPoints.length; x++)
						{
							clipperPoint = clipperPoints[x];
							paths.push(projection.fromPointToLatLng({x: clipperPoint.X / clipperScale, y: clipperPoint.Y / clipperScale}));


						}

						//console.log(polygonOptions);
						map.setCurrentFlightPath(null);

						var polygon = GMaps.drawPolygon(map, paths, polygonOptions);
						//console.log(polygon);
						
						
						map.setCurrentFlightPath(polygon);
					}

					break;

				case flightPath instanceof google.maps.Circle:
					//console.log('setting radius: ' + offset);
					flightPath.setRadius(flightPath._originalRadius + evt.args.value);
					break;

				case flightPath instanceof google.maps.Rectangle:
					offset = 360 / (2 * Math.PI * 6378100) * evt.args.value;

					var vertex1 = flightPath._originalBounds.getSouthWest();
					var vertex2 = flightPath._originalBounds.getNorthEast();

					var newVertex1 = {lat: vertex1.lat() + (Math.cos(45) * -offset), lng: vertex1.lng() + (Math.sin(45) * -offset) };
					var newVertex2 = {lat: vertex2.lat() + (Math.cos(45) * offset), lng: vertex2.lng() + (Math.sin(45) * offset) };

					var newBounds = new google.maps.LatLngBounds(newVertex1, newVertex2);
					flightPath.setBounds(newBounds);
					break;
			}

			if(!isSliding)
			{
				finishSliding();
			}

		});

		$slider.on('slideEnd', function(evt){
			//console.log(JSON.stringify(slider, function(key, val) {
			//	if (val != null && typeof val == "object") {
			//		if (seen.indexOf(val) >= 0) {
			//			return;
			//		}
			//		seen.push(val);
			//	}
			//	return val;
			//}));
			slider._lastValue = [0,0,null,null,0];
			finishSliding();
		});




		//$slider.on('slideStart', function(evt){
		//	//console.log('slideStart');
		//	//var flightPath = map._currentFlightPath;
		//	switch(true)
		//	{
		//		case flightPath instanceof google.maps.Polygon:
		//			//projection = map.getProjection();

		//			//var overlay = new google.maps.OverlayView();
		//			//overlay.draw = function() {};
		//			//overlay.setMap(map);
		//			//projection = overlay.getProjection();
		//			//overlay.setMap(null);

		//			projection = map.getProjection();



		//			var coords = flightPath.getPath().getArray();
		//			var points = [];

		//			for(var x = 0; x < coords.length; x++)
		//				points.push(projection.fromLatLngToPoint(coords[x]));


		//			//console.log(points);
		//			resizingPolygon = GeoUtils.createPolygon(points);

		//			break;
			
		//	}

		//});

		


	},

	expandPolygon: function(polygon, offsetMeters)
	{



		window.poly = polygon;

		//console.log(polygon.getPath().getArray())
	
	
	
	
	},

	convertPolylineToPolygon: function(map, projection, points, offsetMeters)
	{
		var projection = map.getProjection();

		var pts = [];
		for(var x = 0; x < points.length; x++)
			pts.push(projection.fromLatLngToPoint(points[x]));

		for(var x = points.length - 2; x > 0; x--)
			pts.push(projection.fromLatLngToPoint(points[x]));


		var margin = 360 / (2 * Math.PI * 6378100) * offsetMeters;

		var poli = GeoUtils.createPolygon(pts);
		var marginPoli = GeoUtils.createMarginPolygon(poli, margin, 1);

		//console.log(poli);
		//console.log(marginPoli);

		var paths = [];
		for(var x = 0; x < marginPoli.vertices.length; x++)
		{
			paths.push(projection.fromPointToLatLng(marginPoli.vertices[x]));
		}

		//this.drawPolygon(map, paths);

		//console.log(paths);

		return paths;

		return;


		var edges = [];


		//console.log(pts);

		var shapeMargin = 10;

		var offsetEdges = [];

		for (var i = 0; i < pts.length; i++) {
			var point = pts[i];
			var dx = point.x * shapeMargin;
			var dy = point.y * shapeMargin;
			offsetEdges.push(createOffsetEdge(edge, dx, dy));
		}

		var vertices = [];
		for (var i = 0; i < offsetEdges.length; i++) {
			var thisEdge = offsetEdges[i];
			var prevEdge = offsetEdges[(i + offsetEdges.length - 1) % offsetEdges.length];
			var vertex = edgesIntersection(prevEdge, thisEdge);
			if (vertex)
				vertices.push(vertex);
			else {
				var arcCenter = polygon.edges[i].vertex1;
				appendArc(vertices, arcCenter, shapeMargin, prevEdge.vertex2, thisEdge.vertex1, false);
			}
		}
		
		//var converted = google.maps.Data.GeometryCollection(pts);

		//console.log(converted);
	},

	convertPolylineToPolygon3: function(map, projection, points, offset, weight)
	{
		  var pts1 = new Array();//left side of center
		  var pts2 = new Array();//right side of center
			var zoom = map.getZoom();
		  //shift the pts array away from the centre-line by half the gap + half the line width
		  var o = (offset)/2;

		  var p2l,p2r;

		  for (var i=1; i< points.length; i++){
			var p1lm1;
			var p1rm1;
			var p2lm1;
			var p2rm1;
			var thetam1;

			var p1 = projection.fromLatLngToContainerPixel(points[i-1]);
			var p2 = projection.fromLatLngToContainerPixel(points[i]);
			var theta = Math.atan2(p1.x-p2.x,p1.y-p2.y) + (Math.PI/2);
			var dl = Math.sqrt(((p1.x-p2.x)*(p1.x-p2.x))+((p1.y-p2.y)*(p1.y-p2.y)));
			  if(theta > Math.PI)
				  theta -= Math.PI*2;
			var dx = Math.round(o * Math.sin(theta));
			var dy = Math.round(o * Math.cos(theta));

			var p1l = new google.maps.Point(p1.x+dx,p1.y+dy);
			var p1r = new google.maps.Point(p1.x-dx,p1.y-dy);
			p2l = new google.maps.Point(p2.x+dx,p2.y+dy);
			p2r = new google.maps.Point(p2.x-dx,p2.y-dy);

			if(i==1){   //first point
			  pts1.push(projection.fromContainerPixelToLatLng(p1l));
			  pts2.push(projection.fromContainerPixelToLatLng(p1r));
			}

			else{ // mid points

			if(theta == thetam1){
			  // adjacent segments in a straight line
			  pts1.push(projection.fromContainerPixelToLatLng(p1l));
			  pts2.push(projection.fromContainerPixelToLatLng(p1r));
			}
			else{
			  var pli = this.intersect(p1lm1,p2lm1,p1l,p2l);
			  var pri = this.intersect(p1rm1,p2rm1,p1r,p2r);

			  var dlxi = (pli.x-p1.x);
			  var dlyi = (pli.y-p1.y);
			  var drxi = (pri.x-p1.x);
			  var dryi = (pri.y-p1.y);
			  var di = Math.sqrt((drxi*drxi)+(dryi*dryi));
			  var s = o / di;

			  var dTheta = theta - thetam1;
			  if(dTheta < (Math.PI*2))
				dTheta += Math.PI*2;
			  if(dTheta > (Math.PI*2))
				dTheta -= Math.PI*2;

			  if(dTheta < Math.PI){
				//intersect point on outside bend
				pts1.push(projection.fromContainerPixelToLatLng(p2lm1));
				pts1.push(projection.fromContainerPixelToLatLng(new google.maps.Point(p1.x+(s*dlxi),p1.y+(s*dlyi)),zoom));
				pts1.push(projection.fromContainerPixelToLatLng(p1l));
			  }
			  else if (di < dl){
				pts1.push(projection.fromContainerPixelToLatLng(pli));
			  }
			  else{
				pts1.push(projection.fromContainerPixelToLatLng(p2lm1));
				pts1.push(projection.fromContainerPixelToLatLng(p1l));
			  }

			  var dxi = (pri.x-p1.x)*(pri.x-p1.x);
			  var dyi = (pri.y-p1.y)*(pri.y-p1.y);
			  if(dTheta > Math.PI){
				//intersect point on outside bend
				pts2.push(projection.fromContainerPixelToLatLng(p2rm1));
				pts2.push(projection.fromContainerPixelToLatLng(new google.maps.Point(p1.x+(s*drxi),p1.y+(s*dryi)),zoom));
				pts2.push(projection.fromContainerPixelToLatLng(p1r));
			  }
			  else if(di<dl)
			pts2.push(projection.fromContainerPixelToLatLng(pri));
			  else{
				pts2.push(projection.fromContainerPixelToLatLng(p2rm1));
				pts2.push(projection.fromContainerPixelToLatLng(p1r));
			  }
			}
		}

			p1lm1 = p1l;
			p1rm1 = p1r;
			p2lm1 = p2l;
			p2rm1 = p2r;
			thetam1 = theta;
		  }

		  pts1.push(projection.fromContainerPixelToLatLng(p2l));//final point
		  pts2.push(projection.fromContainerPixelToLatLng(p2r));



		return pts1.concat(pts2.reverse());
	
	
	
	},

	convertPolylineToPolygon2: function(map, projection, points, offset, weight)
	{
	   // var zoom = map.getZoom();
		var zoom = map.getZoom();
		zoom = 1;
		//zoom = projection.zoom;
		weight = 0;
		//lef and right swapped throughout!


		var pts1 = new Array(); //left side of center
		var pts2 = new Array(); //right side of center

		//shift the pts array away from the centre-line by half the gap + half the line width
		var o = (offset + weight) / 2;

		var p2l, p2r;

		for (var i = 1; i < points.length; i++) {
			var p1lm1;
			var p1rm1;
			var p2lm1;
			var p2rm1;
			var thetam1;

			var p1 = projection.fromLatLngToContainerPixel(points[i - 1]);
			var p2 = projection.fromLatLngToContainerPixel(points[i]);


			var theta = Math.atan2(p1.x - p2.x, p1.y - p2.y) + (Math.PI / 2);
			var dl = Math.sqrt(((p1.x - p2.x) * (p1.x - p2.x)) + ((p1.y - p2.y) * (p1.y - p2.y)));
			if (theta > Math.PI)
				theta -= Math.PI * 2;
			var dx = Math.round(o * Math.sin(theta));
			var dy = Math.round(o * Math.cos(theta));

			var p1l = new google.maps.Point(p1.x + dx, p1.y + dy);
			var p1r = new google.maps.Point(p1.x - dx, p1.y - dy);
			p2l = new google.maps.Point(p2.x + dx, p2.y + dy);
			p2r = new google.maps.Point(p2.x - dx, p2.y - dy);

			if (i == 1) { //first point
				pts1.push(projection.fromContainerPixelToLatLng(p1l));
				pts2.push(projection.fromContainerPixelToLatLng(p1r));
			} else { // mid points

				if (theta == thetam1) {
					// adjacent segments in a straight line
					pts1.push(projection.fromContainerPixelToLatLng(p1l));
					pts2.push(projection.fromContainerPixelToLatLng(p1r));
				} else {
					var pli = this.intersect(p1lm1, p2lm1, p1l, p2l);
					var pri = this.intersect(p1rm1, p2rm1, p1r, p2r);

					var dlxi = (pli.x - p1.x);
					var dlyi = (pli.y - p1.y);
					var drxi = (pri.x - p1.x);
					var dryi = (pri.y - p1.y);
					var di = Math.sqrt((drxi * drxi) + (dryi * dryi));
					var s = o / di;

					var dTheta = theta - thetam1;
					if (dTheta < (Math.PI * 2))
						dTheta += Math.PI * 2;
					if (dTheta > (Math.PI * 2))
						dTheta -= Math.PI * 2;

					if (dTheta < Math.PI) {
						//intersect point on outside bend
						pts1.push(projection.fromContainerPixelToLatLng(p2lm1));
						pts1.push(projection.fromContainerPixelToLatLng(new google.maps.Point(p1.x + (s * dlxi), p1.y + (s * dlyi)), zoom));
						pts1.push(projection.fromContainerPixelToLatLng(p1l));
					} else if (di < dl) {
						pts1.push(projection.fromContainerPixelToLatLng(pli));
					} else {
						pts1.push(projection.fromContainerPixelToLatLng(p2lm1));
						pts1.push(projection.fromContainerPixelToLatLng(p1l));
					}

					var dxi = (pri.x - p1.x) * (pri.x - p1.x);
					var dyi = (pri.y - p1.y) * (pri.y - p1.y);
					if (dTheta > Math.PI) {
						//intersect point on outside bend
						pts2.push(projection.fromContainerPixelToLatLng(p2rm1));
						pts2.push(projection.fromContainerPixelToLatLng(new google.maps.Point(p1.x + (s * drxi), p1.y + (s * dryi)), zoom));
						pts2.push(projection.fromContainerPixelToLatLng(p1r));
					} else if (di < dl)
						pts2.push(projection.fromContainerPixelToLatLng(pri));
					else {
						pts2.push(projection.fromContainerPixelToLatLng(p2rm1));
						pts2.push(projection.fromContainerPixelToLatLng(p1r));
					}
				}
			}

			p1lm1 = p1l;
			p1rm1 = p1r;
			p2lm1 = p2l;
			p2rm1 = p2r;
			thetam1 = theta;
		}


		pts1.push(projection.fromContainerPixelToLatLng(p2l)); //final point
		pts2.push(projection.fromContainerPixelToLatLng(p2r));


		return pts1.concat(pts2.reverse());
	},

	getPolygonCenterInside : function(pts) {
		var first = pts[0],
		  last = pts[pts.length - 1];
		if (first.lat != last.lat || first.lng != last.lng) pts.push(first);
		var twicearea = 0,
		  x = 0,
		  y = 0,
		  nPts = pts.length,
		  p1, p2, f;
		for (var i = 0, j = nPts - 1; i < nPts; j = i++) {
			p1 = pts[i];
			p2 = pts[j];
			f = p1.lng * p2.lat - p2.lng * p1.lat;
			twicearea += f;
			x += (p1.lng + p2.lng) * f;
			y += (p1.lat + p2.lat) * f;
		}
		f = twicearea * 3;
		return {
			lng: x / f,
			lat: y / f
		};
	}
}


function FlightPathMenu(divId, map, options) {
	var div = $(divId);
	this._div = div;
	this._currentFlightPath = null;
	this._options = options || {};

	this._menuItems = [];

	this._editing = false;
	

	var menu = this;

	div.find('> .delete_vertex').click(function () {
		menu.deleteVertex();
	});

	div.find('> .delete').click(function () {
		menu.deleteFlightPath();
	});
	
	div.find('> .convert_polygon').click(function () {
		menu.convertToPolygon(map);
	});

	div.find('> .edit').click(function () {
		menu.startEdit();
	});

	div.find('> .save').click(function () {
		menu.save();
	});

	div.find('> .cancel').click(function () {
		menu.cancel();
	});

	div.find('> .view').click(function () {
		menu.view();
	});

	
}

FlightPathMenu.prototype = new google.maps.OverlayView();

FlightPathMenu.prototype.open = function (flightPath, evt) {

	//console.log('opening');
	
	this._openEvent = evt;
	
	this._currentFlightPath = flightPath;

	if(flightPath._editing)
	{
		this._div.find('> .save').css('display', '');
		this._div.find('> .cancel').css('display', '');
		this._div.find('> .edit').css('display', 'none');
	}
	else
	{
		this._div.find('> .save').css('display', 'none');
		this._div.find('> .cancel').css('display', 'none');
		this._div.find('> .edit').css('display', '');
	}

	
	if(evt.vertex != undefined)
	{
		var path = flightPath.getPath();
		this.set('path', path);
		this.set('position', path.getAt(evt.vertex));
		this.set('vertex', evt.vertex);
		this._div.find('> .delete_vertex').css('display', '');
	}
	else
	{
		this.set('position', evt.latLng);
		this._div.find('> .delete_vertex').css('display', 'none');
	}

	var convertPolygonDisplay = 'none';

	switch(true)
	{
		
		case flightPath instanceof google.maps.Polygon:
			break;

		case flightPath instanceof google.maps.Polyline:
			convertPolygonDisplay = '';
			break;

		default:
			
			break;
	}

	this._div.find('> .convert_polygon').css('display', convertPolygonDisplay);


	var menuItem;
	var displayElement = true;

	//console.log(this._menuItems);

	for(var x = 0; x < this._menuItems.length; x++)
	{
		menuItem = this._menuItems[x];

		//console.log(menuItem);

		if(menuItem.options)
		{
			switch(menuItem.options.visibility)
			{
				case 'editing':
					displayElement = this._editing;
					break;

				case 'notediting':
					displayElement = !this._editing;
					break;
			}
		}

		if(displayElement)
			menuItem.$element.css('display', '');
		else
			menuItem.$element.css('display', 'none');
	}

	this.setMap(flightPath.getMap());
	this.draw();
};


FlightPathMenu.prototype.draw = function () {
	var position = this.get('position');
	
	var projection = this.getProjection();

	if (!position || !projection) {
		return;
	}

	this._projection = projection;

	var point = projection.fromLatLngToDivPixel(position);

	this._div.css('top', point.y + 'px');
	this._div.css('left', point.x + 'px');
	this._div.css('display', 'block');
};

FlightPathMenu.prototype.close = function () {
	this.setMap(null);
};

FlightPathMenu.prototype.onAdd = function () {
	var menu = this;
	var map = this.getMap();
	this.getPanes().floatPane.appendChild(this._div[0]);

	// mousedown anywhere on the map except on the menu div will close the
	// menu.

	this.divListener_ = google.maps.event.addDomListener(map.getDiv(), 'mousedown', function (e) {
		//console.log(e.target);
		if(e.target.parentNode != menu._div[0])
			menu.close();

		//if (e.target != menu._div[0]) {
		//	menu.close();
		//}
	}, true);
};

FlightPathMenu.prototype.onRemove = function () {
	google.maps.event.removeListener(this.divListener_);
	this._div[0].parentNode.removeChild(this._div[0]);

	// clean up
	this.set('position');
	this.set('path');
	this.set('vertex');
};

FlightPathMenu.prototype.save = function(){
	var flightPath = this._currentFlightPath;
	flightPath._editing = false;
	this._editing = false;
	flightPath.setOptions(flightPath._options);

	if(this._options.save)
	{
		this._options.save(this._currentFlightPath);
	}


	this.close();
};


FlightPathMenu.prototype.endEdit = function() {
	this._currentFlightPath._editing = false;
	this._editing = false;
	this.close();
};


FlightPathMenu.prototype.cancel = function(){
	var flightPath = this._currentFlightPath;

	switch(true)
	{
		case flightPath instanceof google.maps.Polygon:
			//state.paths = GMaps.mvcPathsToObjectArray(flightPath.getPaths());
			flightPath.setPaths(flightPath._state.paths);
			break;

		case flightPath instanceof google.maps.Rectangle:
			//options = GMaps.defaults.rectangleOptions;

			break;

		case flightPath instanceof google.maps.Circle:
			flightPath.setCenter(flightPath._state.center);
			flightPath.setRadius(flightPath._state.radius);
			break;
	}

	flightPath.setOptions(flightPath._state.options);
	this.endEdit();
};

FlightPathMenu.prototype.addMenuItem = function (label, onClick, options) {
	var $element = $('<div>');

	$element.html(label);

	var self = this;

	this._menuItems.push({$element: $element, options: options});

	this._div.append($element);
	
	$element.click(function(){
		onClick.call(this, self, self._currentFlightPath)
		//onClick(this, menu, self._currentFlightPath);
	});
};




FlightPathMenu.prototype.view = function(){
	if(this._options.view)
	{
		this._options.view(this._openEvent.latLng, this._currentFlightPath);
	}



	this.close();
};


FlightPathMenu.prototype.startEdit = function(){
	var flightPath = this._currentFlightPath;

	flightPath._editing = true;

	this._editing = true;

	var options = {};

	var state = {options: {}};

	var keys = ['clickable', 'draggable', 'editable', 'fillColor', 'fillOpacity', 'strokeColor', 'strokeOpacity', 'strokeWeight'];

	var key;
	for(var x = 0; x < keys.length; x++)
	{
		key = keys[x];
		if(flightPath.hasOwnProperty(key))
			state.options[key] = flightPath[key];


		switch(true)
		{
			case flightPath instanceof google.maps.Polygon:
				state.paths = GMaps.mvcPathsToObjectArray(flightPath.getPaths());
				break;

			case flightPath instanceof google.maps.Rectangle:
				//options = GMaps.defaults.rectangleOptions;

				break;

			case flightPath instanceof google.maps.Circle:
				state.center = flightPath.getCenter().toJSON();
				state.radius = flightPath.getRadius();
				break;
		}

	}

	flightPath._state = state;
	//console.log(state);

	switch(true)
	{
		case flightPath instanceof google.maps.Polygon:
			options = GMaps.defaults.polygonOptions;
			break;

		case flightPath instanceof google.maps.Rectangle:
			options = GMaps.defaults.rectangleOptions;
			break;

		case flightPath instanceof google.maps.Circle:
			options = GMaps.defaults.circleOptions;
			break;
	}

	flightPath.setOptions(options);

	this.close();
}

FlightPathMenu.prototype.deleteVertex = function () {
	var path = this.get('path');
	var vertex = this.get('vertex');

	if (!path || vertex == undefined) {
		this.close();
		return;
	}

	path.removeAt(vertex);
	this.close();
};



FlightPathMenu.prototype.deleteFlightPath = function () {
	this.getMap().setCurrentFlightPath(null);
//	this._currentFlightPath.setMap(null);
//	this.getMap()._currentFlightPath = null;
	this.close();
	this._editing = false;
};


FlightPathMenu.prototype.removeFlightPath = function (flightPath) {
	flightPath.setMap(null);
	this._editing = false;
	this.close();
};

FlightPathMenu.prototype.convertToPolygon = function (map) {
	var self = this;
	App.showInput('INPUT', 'Ingresa la brecha en metros:', function(val){
		var pts = self._currentFlightPath.getPath().getArray();
		//self._currentFlightPath.setMap(null);
		map.setCurrentFlightPath(null);
		self.close();

		//console.log(self.getProjection());
		//self.setProjection();
		//console.log(self.getProjection());
		//console.log(self.getMap().getProjection());
		//var projection = self.getMap().getProjection();

		//var map = self.getMap();
		//console.log('map');
		//console.log(map);

		var overlay = new google.maps.OverlayView();
		overlay.draw = function() {};
		overlay.setMap(map);
		var projection = overlay.getProjection();
		overlay.setMap(null);
		//var projection = self._projection;
	  
		//var geofence = new BDCCParallelLines(pts, GMaps.defaults.polygonOptions.strokeColor, GMaps.defaults.polygonOptions.strokeWeight, GMaps.defaults.polygonOptions.strokeOpacity, val*1.0,'polygon', true);
		var newPts = GMaps.convertPolylineToPolygon(map, projection, pts, val * 1.0, GMaps.defaults.polygonOptions.strokeWeight);
		var polygon = GMaps.drawPolygon(map, newPts);
		
		self._currentFlightPath = polygon;
		//map._currentFlightPath = polygon;
		map.setCurrentFlightPath(polygon);
		//polygon.setMap(map);

        google.maps.event.addListener(polygon, 'rightclick', function (e) {
       		self.open(polygon, e);
        });
	}, {width:300, height:200});
};
