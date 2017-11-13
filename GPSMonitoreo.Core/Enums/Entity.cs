using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using GPSMonitoreo.Libraries.DataAnnotations;


namespace GPSMonitoreo.Core.Enums
{

	[ResourceManager(typeof(Resources.Entities))]
    public enum Entity
    {
		Alarm = 1,
		AlarmCategory = 2,
		Geofence = 3,
		GeofenceRoadSurface = 4,
		GeofenceRoadSurfaceCondition = 5,
		GeofenceLayer = 6,
		GeofenceCategory = 7,
		GeofenceCurveGrade = 8,
		GeofenceCurveType = 9,
		GeofenceShape = 10,
		GeofenceIncline = 11,
		GeofenceInclineGrade = 12,
		GeofenceTrafficGrade = 13,
		GeofenceRoadType = 14,
		City = 15,
		EmailPurpose = 16,
		EmailType = 17,
		Entity = 18,
		EntityArea = 19,
		EntityPosition = 20,
		EntityCategory = 21,
		EntityAddressType = 22,
		EntityAddressLocationType = 23,
		EntityIdentificationType = 24,
		EntityRelationType = 25,
		EntityType = 26,
		Equipment = 27,
		EquipmentGroup = 28,
		EntityFace = 29,
		Brand = 30,
		BrandCategory = 31,
		Model = 32,
		Country = 33,
		PersonProfession = 34,
		PersonSalutation = 35,
		Product = 36,
		ProductCategory = 37,
		Province = 38,
		Region = 39,
		Role = 40,
		Route = 41,
		RouteCategory = 42,
		RouteFace = 43,
		RouteSegment = 44,
		RouteSegmentCategory = 45,
		PhoneType = 46,
		RouteSection = 47,
		RouteSectionCategory = 48,
		RouteCourse = 49,
		User = 50
	}

	[ResourceManager(typeof(Resources.Entities))]
	public class EntityClass
	{

	}
}
