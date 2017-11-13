using GPSMonitoreo.Services.Enums;
using GPSMonitoreo.Services.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPSMonitoreo.Services
{
    public class OperationResult: BaseResult
    {
		public OperationStatus Status;
		public ErrorCode ErrorCode;
		public string MessageName;
		public string Message;
		public string WarningMessageName;
		public string WarningMessage;
		
		public List<string> ExtraMessages;
		public string InternalErrorMessage;

		public virtual void SetSuccess()
		{
			this.Succeeded = true;
			this.Status = OperationStatus.Success;
		}

		public virtual void SetSuccess(string message)
		{
			this.SetSuccess();
			this.Message = message;
		}

		public virtual void SetSuccessByMessageName(ServicesMessagesNames messageName)
		{
			this.SetSuccess();
			this.SetMessageByMessageName(messageName);
		}

		public virtual void SetSuccessByMessageName(ServicesMessagesNames messageName, string[] resourceArgs)
		{
			this.SetSuccess();
			this.SetMessageByMessageName(messageName, resourceArgs);
		}

		public virtual void SetSuccessAndWarning()
		{
			this.Succeeded = true;
			this.Status = OperationStatus.SuccessAndWarning;
		}

		public virtual void SetSuccessAndWarningByMessageName(ServicesMessagesNames messageName, ServicesMessagesNames warningMessageName)
		{
			this.SetSuccessAndWarning();
			this.SetMessageByMessageName(messageName);
			this.SetWarningMessageByMessageName(warningMessageName);
		}


		public virtual void SetError()
		{
			this.Succeeded = false;
			this.Status = OperationStatus.Error;
		}

		public virtual void SetError(string message, List<string> extraMessages = null)
		{
			this.SetError();
			this.Message = message;

			if(extraMessages != null)
			{
				this.ExtraMessages = extraMessages;
			}
		}


		public virtual void SetErrorByMessageName(ServicesErrorMessagesNames messageName)
		{
			this.SetError();
			this.SetErrorMessageByMessageName(messageName);
		}

		public virtual void SetErrorByMessageName(ServicesErrorMessagesNames messageName, string[] resourceArgs)
		{
			this.SetError();
			this.SetErrorMessageByMessageName(messageName, resourceArgs);
		}

		public virtual void SetMessageByMessageName(ServicesMessagesNames messageName)
		{
			var resourceMessageName = messageName.ToString();
			var resourceStr = Resources.ServicesMessages.ResourceManager.GetString(resourceMessageName);
			this.MessageName = resourceMessageName;
			this.Message = resourceStr;
		}

		public virtual void SetMessageByMessageName(ServicesMessagesNames messageName, string[] resourceArgs)
		{
			var resourceMessageName = messageName.ToString();
			var resourceStr = Resources.ServicesMessages.ResourceManager.GetString(resourceMessageName);
			this.MessageName = resourceMessageName;
			this.Message = string.Format(resourceStr, resourceArgs);
		}

		public virtual void SetWarningMessageByMessageName(ServicesMessagesNames messageName)
		{
			var resourceMessageName = messageName.ToString();
			var resourceStr = Resources.ServicesMessages.ResourceManager.GetString(resourceMessageName);
			this.WarningMessageName = resourceMessageName;
			this.WarningMessage = resourceStr;
		}

		public virtual void SetErrorMessageByMessageName(ServicesErrorMessagesNames messageName)
		{
			var resourceMessageName = messageName.ToString();
			var resourceStr = Resources.ServicesErrorMessages.ResourceManager.GetString(resourceMessageName);
			this.MessageName = resourceMessageName;
			this.Message = resourceStr;
		}

		public virtual void SetErrorMessageByMessageName(ServicesErrorMessagesNames messageName, string[] resourceArgs)
		{
			var resourceMessageName = messageName.ToString();
			var resourceStr = Resources.ServicesErrorMessages.ResourceManager.GetString(resourceMessageName);
			this.MessageName = resourceMessageName;
			this.Message = string.Format(resourceStr, resourceArgs);
		}
	}
}
