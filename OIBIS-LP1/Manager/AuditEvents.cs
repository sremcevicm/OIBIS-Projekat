using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
	public enum AuditEventTypes
	{
		AuthenticationSuccess = 0,
		AuthorizationSuccess = 2,
		AuthorizationFailed = 1,
		DataBaseWriteSuccess = 6,
		DataBaseWriteFailed = 5,
		DataBaseReadSuccess = 4,
		DataBaseReadFailed = 3
	}
	public class AuditEvents
    {
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string AuthenticationSuccess
		{
			get
			{
				// TO DO
				return ResourceMgr.GetString(AuditEventTypes.AuthenticationSuccess.ToString());
			}
		}

		public static string AuthorizationSuccess
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationSuccess.ToString());
			}
		}

		public static string AuthorizationFailed
		{
			get
			{
				//TO DO
				return ResourceMgr.GetString(AuditEventTypes.AuthorizationFailed.ToString());
			}
		}

		public static string DataBaseWriteSuccess
        {
            get
            {
				return ResourceMgr.GetString(AuditEventTypes.DataBaseWriteSuccess.ToString());
            }

        }

		public static string DataBaseReadSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.DataBaseReadSuccess.ToString());
			}

		}
	}
}
