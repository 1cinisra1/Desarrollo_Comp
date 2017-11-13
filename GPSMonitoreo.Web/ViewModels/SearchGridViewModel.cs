using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Web.ViewModels
{
    public class SearchGridViewModel
    {
        public string MyVar { get; set; }
        public ViewComponents.jqxGrid.jqxGrid Grid { get; set; }

        public string TargetContainer { get; set; }

        public string SourceUrl { get; set; }
		public string SearchUrl { get; set; }

		public string GridID { get; set; }

		public string FormID { get; set; }

		public string WindowID { get; set; }

		public string Title { get; set; }

		public bool EditButton { get; set; }

		public string EditMethod { get; set; }

		public bool AddButton { get; set; }

		public string AddMethod { get; set; }

		public bool QuickViewButton { get; set; }

		


		private string m_scriptWrapper;

		public string ScriptWrapper
		{
			set
			{
				m_scriptWrapper = value;
			}


			get
			{
				if (m_scriptWrapper == null)
				{
					return $"RemoteContent.hook({TargetContainer}, '{SourceUrl}', function($container){{{{\r\n\tvar target = this.target;\r\n\tvar grid = {{}};{{0}}}}}};";
				}
				else
					return m_scriptWrapper;
			}
		}

        public SearchGridViewModel()
        {

			



		}

    }


}
