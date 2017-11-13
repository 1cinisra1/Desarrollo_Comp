var GeoUtils = {

	inwardEdgeNormal: function(edge)
	{
		// Assuming that polygon vertices are in clockwise order
		var dx = edge.vertex2.x - edge.vertex1.x;
		var dy = edge.vertex2.y - edge.vertex1.y;
		var edgeLength = Math.sqrt(dx*dx + dy*dy);
		return {x: -dy/edgeLength, y: dx/edgeLength};
	},

	outwardEdgeNormal: function(edge)
	{
		var n = this.inwardEdgeNormal(edge);
		return {x: -n.x, y: -n.y};
	},

	leftSide: function(vertex1, vertex2, p)
	{
		return ((p.x - vertex1.x) * (vertex2.y - vertex1.y)) - ((vertex2.x - vertex1.x) * (p.y - vertex1.y));
	},

	isReflexVertex: function(polygon, vertexIndex)
	{
		// Assuming that polygon vertices are in clockwise order
		var thisVertex = polygon.vertices[vertexIndex];
		var nextVertex = polygon.vertices[(vertexIndex + 1) % polygon.vertices.length];
		var prevVertex = polygon.vertices[(vertexIndex + polygon.vertices.length - 1) % polygon.vertices.length];
		if (this.leftSide(prevVertex, nextVertex, thisVertex) < 0)
			return true;  // TBD: return true if thisVertex is inside polygon when thisVertex isn't included

		return false;
	},

	createPolygon: function(vertices)
	{
		var polygon = {vertices: vertices};

		var edges = [];
		var minX = (vertices.length > 0) ? vertices[0].x : undefined;
		var minY = (vertices.length > 0) ? vertices[0].y : undefined;
		var maxX = minX;
		var maxY = minY;

		for (var i = 0; i < polygon.vertices.length; i++) {
			vertices[i].label = String(i);
			vertices[i].isReflex = this.isReflexVertex(polygon, i);
			var edge = {
				vertex1: vertices[i], 
				vertex2: vertices[(i + 1) % vertices.length], 
				polygon: polygon, 
				index: i
			};
			edge.outwardNormal = this.outwardEdgeNormal(edge);
			edge.inwardNormal = this.inwardEdgeNormal(edge);
			edges.push(edge);
			var x = vertices[i].x;
			var y = vertices[i].y;
			minX = Math.min(x, minX);
			minY = Math.min(y, minY);
			maxX = Math.max(x, maxX);
			maxY = Math.max(y, maxY);
		}                       
    
		polygon.edges = edges;
		polygon.minX = minX;
		polygon.minY = minY;
		polygon.maxX = maxX;
		polygon.maxY = maxY;
		polygon.closed = true;

		return polygon;
	},

	isClockWise: function(vertices)
	{
		var sum = 0.0;
		var v1;
		var v2;
		console.log(vertices);
		for (var i = 0; i < vertices.length; i++) {
			console.log(i);
			v1 = vertices[i];
			v2 = vertices[(i + 1) % vertices.length]; // % is the modulo operator
			//sum += (v2.x - v1.x) * (v2.y + v1.y);

			sum += v1.x * v2.y;
			sum -= v2.x * v1.y;

			console.log('sum: ' + sum);
		}
		return sum > 0.0;
	},

	createOffsetEdge: function(edge, dx, dy)
	{
		return {
			vertex1: {x: edge.vertex1.x + dx, y: edge.vertex1.y + dy},
			vertex2: {x: edge.vertex2.x + dx, y: edge.vertex2.y + dy}
		};
	},

	edgesIntersection: function(edgeA, edgeB)
	{
		var den = (edgeB.vertex2.y - edgeB.vertex1.y) * (edgeA.vertex2.x - edgeA.vertex1.x) - (edgeB.vertex2.x - edgeB.vertex1.x) * (edgeA.vertex2.y - edgeA.vertex1.y);
		if (den == 0)
			return null;  // lines are parallel or conincident

		var ua = ((edgeB.vertex2.x - edgeB.vertex1.x) * (edgeA.vertex1.y - edgeB.vertex1.y) - (edgeB.vertex2.y - edgeB.vertex1.y) * (edgeA.vertex1.x - edgeB.vertex1.x)) / den;
		var ub = ((edgeA.vertex2.x - edgeA.vertex1.x) * (edgeA.vertex1.y - edgeB.vertex1.y) - (edgeA.vertex2.y - edgeA.vertex1.y) * (edgeA.vertex1.x - edgeB.vertex1.x)) / den;

		if (ua < 0 || ub < 0 || ua > 1 || ub > 1)
			return null;

		return {x: edgeA.vertex1.x + ua * (edgeA.vertex2.x - edgeA.vertex1.x),  y: edgeA.vertex1.y + ua * (edgeA.vertex2.y - edgeA.vertex1.y)};
	},

	appendArc: function(vertices, center, radius, segments, startVertex, endVertex, isPaddingBoundary)
	{
		const twoPI = Math.PI * 2;
		var startAngle = Math.atan2(startVertex.y - center.y, startVertex.x - center.x);
		var endAngle = Math.atan2(endVertex.y - center.y, endVertex.x - center.x);
		if (startAngle < 0)
			startAngle += twoPI;
		if (endAngle < 0)
			endAngle += twoPI;
		//var arcSegmentCount = 5; // An odd number so that one arc vertex will be eactly arcRadius from center.
		
		var angle = ((startAngle > endAngle) ? (startAngle - endAngle) : (startAngle + twoPI - endAngle));
		var angle5 =  ((isPaddingBoundary) ? -angle : twoPI - angle) / segments;

		vertices.push(startVertex);
		for (var i = 1; i < segments; ++i) {
			var angle = startAngle + angle5 * i;
			var vertex = {
				x: center.x + Math.cos(angle) * radius,
				y: center.y + Math.sin(angle) * radius,
			};
			vertices.push(vertex);
		}
		vertices.push(endVertex);
	},

	expandPolygon: function(polygon, offset, arcSegments, isClockWise)
	{
		//if(offset > 0)
		//{
		//	return this.createMarginPolygon(polygon, offset, arcSegments);
		//}

		return this.createPaddingPolygon(polygon, offset, arcSegments);

		//if(offset > 0)
		//{
		//	return this.createMarginPolygon(polygon, isClockWise ? offset : -offset, arcSegments);
		//}
		//else
		//	return this.createPaddingPolygon(polygon, isClockWise ? -offset : offset);
	},

	createMarginPolygon: function(polygon, margin, arcSegments)
	{
		console.log('margin: ' + margin);
		console.log('arcSegments: ' + arcSegments);
		var offsetEdges = [];
		for (var i = 0; i < polygon.edges.length; i++) {
			var edge = polygon.edges[i];
			var dx = edge.outwardNormal.x * margin;
			var dy = edge.outwardNormal.y * margin;
			offsetEdges.push(this.createOffsetEdge(edge, dx, dy));
		}

		var vertices = [];
		for (var i = 0; i < offsetEdges.length; i++) {
			var thisEdge = offsetEdges[i];
			var prevEdge = offsetEdges[(i + offsetEdges.length - 1) % offsetEdges.length];
			var vertex = this.edgesIntersection(prevEdge, thisEdge);
			if (vertex)
				vertices.push(vertex);
			else {
				var arcCenter = polygon.edges[i].vertex1;
				this.appendArc(vertices, arcCenter, margin, arcSegments, prevEdge.vertex2, thisEdge.vertex1, false);
			}
		}

		var marginPolygon = this.createPolygon(vertices);
		//marginPolygon.offsetEdges = offsetEdges;
		return marginPolygon;
	},

	createPaddingPolygon: function(polygon, shapePadding, arcSegments)
	{
		var offsetEdges = [];
		for (var i = 0; i < polygon.edges.length; i++) {
			var edge = polygon.edges[i];
			var dx = edge.inwardNormal.x * shapePadding;
			var dy = edge.inwardNormal.y * shapePadding;
			offsetEdges.push(this.createOffsetEdge(edge, dx, dy));
		}

		var vertices = [];
		for (var i = 0; i < offsetEdges.length; i++) {
			var thisEdge = offsetEdges[i];
			var prevEdge = offsetEdges[(i + offsetEdges.length - 1) % offsetEdges.length];
			var vertex = this.edgesIntersection(prevEdge, thisEdge);
			if (vertex)
				vertices.push(vertex);
			else {
				var arcCenter = polygon.edges[i].vertex1;
				this.appendArc(vertices, arcCenter, arcSegments, prevEdge.vertex2, thisEdge.vertex1, true);
			}
		}

		var paddingPolygon = this.createPolygon(vertices);
		//paddingPolygon.offsetEdges = offsetEdges;
		return paddingPolygon;
	}









};