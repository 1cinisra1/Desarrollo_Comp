using GPSMonitoreo.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GPSMonitoreo.Dtos.Extensions.LinqExtensions;


namespace GPSMonitoreo.Dtos.Devices
{
    public class AlarmedGpsDto: GpsDto
    {
		public List<GpsAlarmDto> Alarms;

		static AlarmedGpsDto()
		{
			Console.WriteLine("static AlarmedGpsDto()");
		}

		public static new Expression<Func<GPS, AlarmedGpsDto>> Selector = GpsDto.Selector.Extend(project => new Devices.AlarmedGpsDto
		{
			//Alarms = ((GPS)project).ALARMAS.Select(item => new GpsAlarmDto
			//{
			//	DateTime = item.FECHAHORA,
			//	AlarmDescription = item.ALARMA.DESCRIPCION_LARGA,
			//	GeoFenceId = item.CERCA.ID,
			//	GeoFenceDescription = item.CERCA.DESCRIPCION_LARGA,
			//	AlarmValue = item.LOG.VALOR_EFECTIVO,
			//	AlarmValueStandard = item.LOG.VALOR_STANDARD

			//}).ToList()
		});



		//public static new IQueryable<AlarmedGpsDto> FromEntity(IQueryable<Data.Models.GPS> entity)
		////public static IQueryable<GpsDto> FromEntity(IQueryable<IComunInt32> entity)
		//{
		//	//var query = GpsDto.FromEntity(entity);

		//	//var query = GpsDto.FromEntity(entity).Select(item => new AlarmedGpsDto
		//	//{
		//	//	Id = item.Id,
		//	//	Description = item.Description
		//	//	//Alarms = null
		//	//});








		//	System.Linq.Expressions.Expression<Func<GPS, GpsDto>> exp1 = project2 => new Devices.GpsDto
		//	{
		//		Id = project2.ID,
		//		Description = project2.DESCRIPCION_LARGA
		//	};


		//	//System.Linq.Expressions.Expression<Func<GPS, AlarmedGpsDto>> exp2 = project => new Devices.AlarmedGpsDto
		//	//{
		//	//	Alarms = ((GPS)project).ALARMAS.Select(item => new GpsAlarmDto
		//	//	{
		//	//		DateTime = item.FECHAHORA,
		//	//		AlarmDescription = item.ALARMA.DESCRIPCION_LARGA,
		//	//		//GeoFenceDescription = item.CERCA.DESCRIPCION_LARGA
		//	//	}).ToList()
		//	//};

		//	var exp3 = exp1.Extend(project => new Devices.AlarmedGpsDto
		//	{
		//		Alarms = ((GPS)project).ALARMAS.Select(item => new GpsAlarmDto
		//		{
		//			DateTime = item.FECHAHORA,
		//			AlarmDescription = item.ALARMA.DESCRIPCION_LARGA,
		//			//GeoFenceDescription = item.CERCA.DESCRIPCION_LARGA
		//		}).ToList()
		//	});

		//	//var exp3 = LinqHelpers.Combine2(exp1, exp2);


		//	//var qry = entity.Select(exp3);
		//	//qry.Select(item => item.)

		//	//Console.WriteLine(exp3.ToString());



		//	//Console.WriteLine(exp1.ToString());
		//	//Console.WriteLine(exp1.Parameters[0].ToString());
		//	//Console.WriteLine(exp1.Parameters[0].Type.ToString());
		//	//Console.WriteLine(exp1.Parameters[0].GetType().ToString());
		//	//Console.WriteLine(exp1.Body.GetType().ToString());


		//	//var member = (MemberInitExpression)exp1.Body;


		//	////member.NewExpression.

		//	////foreach(var i in member.Bindings.Select(item => item.Member.Name))
		//	//foreach (var i in member.NewExpression.Members.Select(item => item.Name))
		//	//{
		//	//	Console.WriteLine("member name: " + i);
		//	//}


		//	var query = entity.Select(exp3);




		//	//exp1.Compile()



		//	//System.Linq.Expressions.Expression<Func<Data.Models.GPS, GpsDto>> exp = System.Linq.Expressions.Expression < Func < Data.Models.GPS, GpsDto>>



		//	//	System.Linq.Expressions.Expression<Func<Data.Models.GPS, AlarmedGpsDto>> exp = item => item.

		//	//Console.WriteLine(exp == null);


		//	//var query = entity.Select(item => new AlarmedGpsDto
		//	//{


		//	//});


		//	//var query = entity.Select(
		//	//	item => new GpsDto
		//	//	{
		//	//		Id = item.ID,
		//	//		Description = item.DESCRIPCION_LARGA,
		//	//		//Alarms = item.ALARMAS.Select(x => new GpsAlarmDto
		//	//		//{
		//	//		//	DateTime = x.FECHAHORA,
		//	//		//	GeoFenceId = x.CERCA_ID,
		//	//		//	GeoFenceDescription = x.CERCA.DESCRIPCION_LARGA,
		//	//		//	AlarmDescription = x.ALARMA.DESCRIPCION_LARGA
		//	//		//}).ToList()
		//	//	}
		//	//);

		//	return query;
		//}
	}
}
