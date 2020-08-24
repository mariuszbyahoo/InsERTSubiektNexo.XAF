using InsERT.Moria.Klienci;
using InsERT.Moria.ModelDanych;
using InsERT.Moria.Sfera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsERTSubiektNexo.XAF.Module.Services
{
    public class SerwisPodmiotow : BazowySerwisModeluDanych
    {
        private readonly Uchwyt _sfera;

        public SerwisPodmiotow()
        {
            _sfera = PolaczZeSfera();
        }

        public Podmiot[] PodajWszystkiePodmioty()
        {
            var mgr = _sfera.PodajObiektTypu<IPodmioty>();

            return mgr.Dane.Wszystkie().ToArray();
        }

        public Podmiot[] PodajWszystkieFirmy()
        {
            var mgr = _sfera.PodajObiektTypu<IPodmioty>();

            return mgr.Dane.WszystkieFirmy().ToArray();
        }
    }
}
