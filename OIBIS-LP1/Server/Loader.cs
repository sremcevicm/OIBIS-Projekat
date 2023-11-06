using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace Server
{
    public class Loader
    {
        public X509Certificate2 LoadClientCertificate()
        {

            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindBySubjectName, "YourClientCertificate", false);

            store.Close();

            if (certificates.Count > 0)
            {
                return certificates[0];
            }
            else
            {
                return null;
            }
        }
    }
}
