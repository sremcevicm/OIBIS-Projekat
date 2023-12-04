using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IServis
    {
        [OperationContract]
        void DodajProjekciju(string imeProjekcije, string vremeRezervacije, int sala, double cenaKarte);
        [OperationContract]
        void IzmeniProjekciju();
        [OperationContract]
        void IzmeniPopust();
        [OperationContract]
        void NapraviRezervaciju();
        [OperationContract]
        void PlatiRezervaciju();
    }
}
