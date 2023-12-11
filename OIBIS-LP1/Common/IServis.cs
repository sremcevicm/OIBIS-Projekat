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
        void DodajProjekciju(string imeProjekcije, DateTime vremeProjekcije, int sala, double cenaKarte);
        [OperationContract]
        void IzmeniProjekciju(string imeProjekcije, DateTime novoVremeProjekcije, int novaSala, double novaCenaKarte);
        [OperationContract]
        void IzmeniPopust(int noviPopust);
        [OperationContract]
        void NapraviRezervaciju(string imeProjekcije, int brojKarata);
        [OperationContract]
        void PlatiRezervaciju(string idRezervacije);
    }
}
