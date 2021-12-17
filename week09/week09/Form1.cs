using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using week09.Entities;

namespace week09
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<BirthProbability> BirthProbabilities = new List<BirthProbability>();
        List<DeathProbability> DeathProbabilities = new List<DeathProbability>();
         
        public Form1()
        {
            InitializeComponent();

            Population = GetPopulation(@"C:\temp\nép.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\temp\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\temp\halál.csv");
        }

        private List<DeathProbability> GetDeathProbabilities(string csvpath)
        {
            List<DeathProbability> deaths = new List<DeathProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    DeathProbability dp = new DeathProbability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        ProbabilityOfDeath = double.Parse(line[2], System.Globalization.CultureInfo.GetCultureInfo("hu-HU"))
                    };
                    deaths.Add(dp);
                }

        private List<BirthProbability> GetBirthProbabilities(string csvpath)
        {
            List<BirthProbability> births = new List<BirthProbability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(';');
                    BirthProbability bp = new BirthProbability()
                    {
                        Age = int.Parse(line[0]),
                        NumberOfChildren = int.Parse(line[1]),
                        ProbabilityOfBirth = double.Parse(line[2], System.Globalization.CultureInfo.GetCultureInfo("hu-HU"))
                    };
                    births.Add(bp);
                }
            }
            return births;
        }

        private List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }
    }
}
