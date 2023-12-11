using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Audit : IDisposable
    {
        private static EventLog eventLog = null;
        const string SourceName = "Manager.Audit";
        const string LogName = "OibisProj";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                eventLog = new EventLog(LogName, Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                eventLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);

                Console.WriteLine(e.StackTrace);
            }
        }

        public static void AuthenticationSuccess(string userName)
        {
            //TO DO

            if (eventLog != null)
            {
                string UserAuthenticationSuccess =
                    AuditEvents.AuthenticationSuccess;
                string message = String.Format(UserAuthenticationSuccess,
                    userName);
                eventLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthenticationSuccess));
            }
        }

        public static void AuthorizationSuccess(string userName, string serviceName)
        {
            //TO DO
            if (eventLog != null)
            {
                string AuthorizationSuccess =
                    AuditEvents.AuthorizationSuccess;
                string message = String.Format(AuthorizationSuccess,
                    userName, serviceName);
                eventLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationSuccess));
            }
        }

        public static void AuthorizationFailed(string userName, string serviceName, string reason)
        {
            if (eventLog != null)
            {
                string AuthorizationFailed =
                    AuditEvents.AuthorizationFailed;
                string message = String.Format(AuthorizationFailed,
                    userName, serviceName, reason);
                eventLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AuthorizationFailed));
            }
        }

        public static void DataBaseWriteSuccess(string userName)
        {
            if (eventLog != null)
            {
                string DataBaseWriteSuccess = AuditEvents.DataBaseWriteSuccess;
                string message = String.Format(DataBaseWriteSuccess, userName);
                eventLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.DataBaseWriteSuccess));
            }
        }

        public static void DataBaseReadSuccess(string tableName)
        {
            if (eventLog != null)
            {
                string DataBaseReadSuccess = AuditEvents.DataBaseWriteSuccess;
                string message = String.Format(DataBaseReadSuccess, tableName);
                eventLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.DataBaseReadSuccess));
            }
        }

        public void Dispose()
        {
            if (eventLog != null)
            {
                eventLog.Dispose();
                eventLog = null;
            }
        }
    }
}
