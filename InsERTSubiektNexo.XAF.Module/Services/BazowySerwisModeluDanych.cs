using InsERT.Moria.Sfera;
using InsERT.Mox.Product;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsERTSubiektNexo.XAF.Module.Services
{
    public abstract class BazowySerwisModeluDanych
    {
        private string _connectionString;
        private string _dbName;
        private string _login;
        private string _password;

        public BazowySerwisModeluDanych()
        {
            var jSettings = JToken.Parse(File.ReadAllText("appsettings.json"));
            _connectionString = (string)jSettings["connectionString"];
            _dbName = (string)jSettings["DbName"];
            _login = (string)jSettings["Login"];
            _password = (string)jSettings["Password"];
        }

        private protected Uchwyt PolaczZeSfera()
        {
            DanePolaczenia danePolaczenia = DanePolaczenia.Jawne(_connectionString, _dbName, true);
            MenedzerPolaczen mp = new MenedzerPolaczen();
            Uchwyt sfera = mp.Polacz(danePolaczenia, ProductId.Subiekt);
            sfera.ZalogujOperatora(_login, _password);
            return sfera;
        }
    }
}
