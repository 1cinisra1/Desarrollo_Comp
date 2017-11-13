using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace GPSMonitoreo.Dtos.Validation.Attributes
{
    public abstract class RulesContainerAttribute: ValidationAttribute
    {
		private Type _errorMessageresourceType;


		protected List<Rule> m_rules;

		public RulesContainerAttribute()
		{
			m_rules = new List<Rule>();
		}

		public new Type ErrorMessageResourceType
		{
			get { return _errorMessageresourceType; }
			set
			{
				_errorMessageresourceType = value;

				foreach (var rule in m_rules)
				{
					if(rule.attribute.ErrorMessage == null && rule.attribute.ErrorMessageResourceType == null)
					{
						if(typeof(RulesContainerAttribute).IsAssignableFrom(rule.attributeType))
						{
							((RulesContainerAttribute)rule.attribute).ErrorMessageResourceType = _errorMessageresourceType;
						}
						else
						{
							rule.attribute.ErrorMessageResourceType = _errorMessageresourceType;
							rule.attribute.ErrorMessageResourceName = rule.name;
						}
					}
				}
			}
		}

		public Rule AddRuleFromString(string rule)
		{
			var r = new Rule(rule, this);
			m_rules.Add(r);
			return r;
		}

	}
}
