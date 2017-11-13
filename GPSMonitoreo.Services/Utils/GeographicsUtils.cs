using GPSMonitoreo.Data.Extensions;
using GPSMonitoreo.Data.Models;
using GPSMonitoreo.Dtos.Base.Geofences;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services.Utils
{
    public static class GeographicsUtils
    {

		public static GeofenceForMapDto GetGeofenceForMap(int id, string description, byte layerId, byte shapedId, string coordsJson)
		{
			JObject coordsObject = coordsJson != null ? JObject.Parse(coordsJson) : null;
			JArray coordsObjectPoints;
			JToken vertex1Object;
			JToken vertex2Object;
			JToken circleObject;

			GeofenceCoordExtended coordExtended;


			var ret = new GeofenceForMapDto
			{
				Id = id,
				Description = description,
				LayerId = layerId,
				ShapeId = shapedId
			};


			List<GeofenceCoord> coordExtendedCoords;

			coordExtended = null;


			if (coordsObject != null)
			{
				vertex1Object = coordsObject["vertice1"];
				vertex2Object = coordsObject["vertice2"];


				coordExtended = new GeofenceCoordExtended
				{
					Vertex1 = new GeofenceCoord
					{
						lat = vertex1Object.Value<double>("lat"),
						lng = vertex1Object.Value<double>("lng")
					},
					Vertex2 = new GeofenceCoord
					{
						lat = vertex2Object.Value<double>("lat"),
						lng = vertex2Object.Value<double>("lng")
					}
				};

				if (ret.ShapeId == 1)
				{

					circleObject = coordsObject["circular_punto"];

					coordExtended.CircleCoord = new GeofenceCoord
					{
						lat = circleObject.Value<double>("lat"),
						lng = circleObject.Value<double>("lng")
					};

					coordExtended.CircleRadius = (double)coordsObject["circular_radio"];
				}


				coordsObjectPoints = (JArray)coordsObject["puntos"];

				coordExtendedCoords = new List<GeofenceCoord>(coordsObjectPoints.Count);

				foreach (var coordObjectPoint in coordsObjectPoints)
				{
					coordExtendedCoords.Add(new GeofenceCoord
					{
						lat = coordObjectPoint.Value<double>("lat"),
						lng = coordObjectPoint.Value<double>("lng")
					});
				}

				coordExtended.Coords = coordExtendedCoords;

				ret.Coords = coordExtended;
			}

			return ret;
		}

		public static List<GeofenceForMapDto> GetAllGeofencesForMap(EntitiesContext dbContext, string storeProcedureName, int id)
		{
			var pars = new List<KeyValuePair<string, object>>
			{
				new KeyValuePair<string, object>("P_ID", id)
			};

			var reader = dbContext.ProcedureDataReader(storeProcedureName, pars);

			var items = new List<GeofenceForMapDto>();

			while (reader.Read())
			{
				items.Add(GetGeofenceForMap((int)reader["ID"], reader["DESCRIPCION"] as string, Convert.ToByte(reader["CAPA_ID"]), Convert.ToByte(reader["FORMA_ID"]), reader["COORDS"] != DBNull.Value ? reader["COORDS"] as string : null));
			}

			reader.Close();

			return items;
		}
    }
}
