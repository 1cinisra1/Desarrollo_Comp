using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web
{
	public class NotificationsAlarmsTree: NotificationsAlarmCat
	{

		public Dictionary<int, NotificationsAlarmCat> AllCats;

		public NotificationsAlarmsTree()
		{
			AllCats = new Dictionary<int, NotificationsAlarmCat>();
		}

		public void AddCat(int parentId, int id, string description)
		{
			NotificationsAlarmCat parent;

			if (parentId > 0)
			{
				parent = AllCats[parentId];
			}
			else
			{
				parent = this;
			}

			var cat = new NotificationsAlarmCat()
			{
				Id = id,
				Description = description,
				Parent = parent,
				Level = parent.Level + 1
			};

			parent.Subcategories.Add(cat);

			AllCats[id] = cat;
		}

		public void AddAlarm(int parentId, NotificationsAlarm alarm)
		{
			var parent = AllCats[parentId];

			parent.Alarms.Add(alarm);

			alarm.Parent = parent;

			if (!parent.Display)
			{
				parent.Display = true;
			}
		}
	}

	public class NotificationsAlarmNode
	{
		public int Id;

		public string Description;

		public int Level;

	}

    public class NotificationsAlarmCat : NotificationsAlarmNode
	{
		public List<NotificationsAlarmCat> Subcategories;

		public List<NotificationsAlarm> Alarms;

		public NotificationsAlarmsTree Tree;

		public NotificationsAlarmCat Parent;


		private bool _display;

		public bool Display
		{
			get
			{
				return _display;
			}

			set
			{
				_display = value;

				if (value && this.Parent != null && !this.Parent._display)
				{
					this.Parent.Display = true;
				}
			}
		}

		public NotificationsAlarmCat()
		{
			Subcategories = new List<NotificationsAlarmCat>();
			Alarms = new List<NotificationsAlarm>();
		}

		public void Render(System.Text.StringBuilder tableHtml)
		{
			if (this.Display)
			{
				if(this.Level > 0)
				{
					//Console.WriteLine("desc: " + this.Level);

					string backgroundColor = "";

					switch (this.Level)
					{
						case 1:
							backgroundColor = "#d1d1d1";
							break;

						case 2:
							backgroundColor = "#f1f1f1";
							break;
					}

					tableHtml.Append($"<tr><td style=\"background-color:{backgroundColor}\" colspan=\"13\" class=\"category\">" + string.Concat(Enumerable.Repeat("&nbsp;", (this.Level - 1) * 5)) + this.Description + "</td></tr>");
					//tableHtml.Append(string.Concat(Enumerable.Repeat("<td>&nbsp;</td>", 11)));

				}

				foreach (var subcat in Subcategories)
				{
					subcat.Render(tableHtml);
				}

				foreach (var alarm in Alarms)
				{
					alarm.Render(tableHtml);
				}
			}
		}
	}

	public class NotificationsAlarm
	{
		public int Id;
		public int AlarmId;
		public string AlarmDescription;
		
		public DateTime Date;
		public string GeofenceAuxiliarId;
		public string RouteAuxiliarId;
		public string SectionAuxiliarId;
		public string SegmentAuxiliarId;
		public double StandardValue;
		public double ActualValue;
		public double Excess;
		public int Duration;
		public int DriverId;

		public NotificationsAlarmCat Parent;

		public static List<int> AlarmsForDuration = new List<int> { 3, 4, 5, 6, 7, 8, 9, 15, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31 };

		public static List<int> AlarmsForValues = new List<int> { 6, 7, 8, 9, 10, 15, 17, 18, 19, 20, 21, 23, 24, 25, 26, 27 };

		public static Dictionary<int, string> AlarmUnits = new Dictionary<int, string>
		{
			{ 6, "kms/h" },
			{ 7, "kms/h" },
			{ 8, "kms/h" },
			{ 9, "kms/h" },
			{ 15, "hrs" },
			{ 17, "hrs" },
			{ 18, "hrs" },
			{ 19, "hrs" },
			{ 20, "hrs" },
			{ 21, "hrs" },
			{ 23, "hrs" },
			{ 24, "hrs" },
			{ 25, "hrs" },
			{ 26, "hrs" },
			{ 27, "hrs" }
		};

		public void Render(System.Text.StringBuilder tableHtml)
		{
			string unitStr = "";
			string standardValueStr = "";
			string actualValueStr = "";
			string excessStr = "";
			string durationStr = "";


			AlarmUnits.TryGetValue(this.AlarmId, out unitStr);

			if (AlarmsForDuration.Contains(this.AlarmId))
			{
				durationStr = (this.Duration / 60.0 / 60.0).ToString("F4");
			}

			


			if (AlarmsForValues.Contains(this.AlarmId))
			{
				excessStr = this.Excess.ToString();
				standardValueStr = this.StandardValue.ToString();
				actualValueStr = this.ActualValue.ToString();
			}


			tableHtml.Append($"<tr><td style=\"width:250px\">" + string.Concat(Enumerable.Repeat("&nbsp;", (this.Parent.Level - 1) * 5)) + this.AlarmDescription + "</td>");

			tableHtml.Append("<td style=\"width:75px\">" + this.Date.ToString("dd/MM/yyyy") + "</td>");
			tableHtml.Append("<td style=\"width:45px\">" + this.Date.ToString("HH:mm") + "</td>");
			tableHtml.Append("<td align=\"center\" style=\"width:30px\">" + this.DriverId + "</td>");
			tableHtml.Append("<td align=\"center\" style=\"width:30px\">" + this.RouteAuxiliarId + "</td>");
			tableHtml.Append("<td align=\"center\" style=\"width:30px\">" + this.SectionAuxiliarId + "</td>");
			tableHtml.Append("<td align=\"center\" style=\"width:30px\">" + this.SegmentAuxiliarId + "</td>");
			tableHtml.Append("<td align=\"center\" style=\"width:30px\">" + this.GeofenceAuxiliarId + "</td>");


			tableHtml.Append("<td style=\"width:45px\" align=\"center\" >" + unitStr + "</td>");
			tableHtml.Append("<td style=\"width:45px\" align=\"center\" >" + standardValueStr + "</td>");
			tableHtml.Append("<td style=\"width:45px\" align=\"center\" >" + actualValueStr + "</td>");
			tableHtml.Append("<td style=\"width:45px\" align=\"center\" >" + excessStr + "</td>");
			tableHtml.Append("<td style=\"width:45px\" align=\"center\" >" + durationStr + "</td>");

			tableHtml.Append("</tr>");
			//Console.WriteLine(this.Description);
		}
	}
}
