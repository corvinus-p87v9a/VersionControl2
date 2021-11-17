using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WebSzolg.Entities;
using WebSzolg.MnbServiceReference;

namespace WebSzolg
{
    public partial class Form1 : Form
    {
        new BindingList<RateData> Rates;
        public Form1()
        {
            InitializeComponent();
            Hivas();
            dataGridView1.DataSource = Rates;
        }

        public void Hivas()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();

            var request = new GetExchangeRatesRequestBody();
                 request.currencyNames = "EUR";
                 request.startDate = "2020-01-01";
                 request.endDate = "2020-06-30";

            var response = mnbService.GetExchangeRates(request);

            var result = response.GetExchangeRatesResult;

            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                RateData r = new RateData();

                Rates.Add(r);
            }
        }
    }
}
