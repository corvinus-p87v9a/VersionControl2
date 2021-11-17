using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml;
using WebSzolg.Entities;
using WebSzolg.MnbServiceReference;

namespace WebSzolg
{
    public partial class Form1 : Form
    {
        BindingList<RateData> Rates = new BindingList<RateData>();
        BindingList<string> Currencies = new BindingList<string>();
        public Form1()
        {
            InitializeComponent();
            RefreshData();
            GetCurrencies();
            comboBox1.DataSource = Currencies;
        }

        public void GetCurrencies()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetCurrenciesRequestBody();
            var response = mnbService.GetCurrencies(request);
            var result = response.GetCurrenciesResult;
            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                string c;

                var childElement = (XmlElement)element.ChildNodes[0];
                c= childElement.InnerText;

                Currencies.Add(c);
            }
        }

        private void RefreshData()
        {
            Hivas();
            dataGridView1.DataSource = Rates;

            Rates.Clear();
        }

        public void Hivas()
        {
            var mnbService = new MNBArfolyamServiceSoapClient();
            var request = new GetExchangeRatesRequestBody();
                 request.currencyNames = comboBox1.SelectedItem.ToString();
                 request.startDate = dateTimePicker1.Value.ToString();
                 request.endDate = dateTimePicker2.Value.ToString();
            var response = mnbService.GetExchangeRates(request);
            var result = response.GetExchangeRatesResult;
            var xml = new XmlDocument();
            xml.LoadXml(result);

            foreach (XmlElement element in xml.DocumentElement)
            {
                var r = new RateData();
                Rates.Add(r);

                var childElement = (XmlElement)element.ChildNodes[0];

                r.Currency = childElement.GetAttribute("curr");

                r.Date = DateTime.Parse(element.GetAttribute("date"));

                var unit = decimal.Parse(childElement.GetAttribute("unit"));
                var value = decimal.Parse(childElement.InnerText);
                if (unit != 0)
                    r.Value = value / unit;
            }

            chartRateData.DataSource = Rates;

            var series = chartRateData.Series[0];

            series.ChartType = SeriesChartType.Line;
            series.XValueMember = "Date";
            series.YValueMembers = "Value";
            series.BorderWidth = 2;
            var legend = chartRateData.Legends[0];
            legend.Enabled = false;

            var chartArea = chartRateData.ChartAreas[0];
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.IsStartedFromZero = false;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }
    }
}
