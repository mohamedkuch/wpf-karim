using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Configurations;
using System.IO;


namespace Example_RealTime_Chart
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;
        private double _trend;
        double[] yWerte;
        private string dataNumber;
        private string anfang;
        private string ende;
        private string yWerte1;


        List<List<string>> data = new List<List<string>>();
        List<string> testingY;
        string dNumber;
        int laenge;


        public MainWindow()
        {
            

            InitializeComponent();

            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks)   //use DateTime.Ticks as X
                .Y(model => model.Value);           //use the value property as Y

            //lets save the mapper globally.
            Charting.For<MeasureModel>(mapper);

            //the values property will store our values array
            ChartValues = new ChartValues<MeasureModel>();

            //lets set how to display the X Labels
            DateTimeFormatter = value => new DateTime((long)value).ToString("hh:mm:ss");

            //AxisStep forces the distance between each separator in the X axis
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            //AxisUnit forces lets the axis know that we are plotting seconds
            //this is not always necessary, but it can prevent wrong labeling
            AxisUnit = TimeSpan.TicksPerSecond;

            SetAxisLimits(DateTime.Now);

            //The next code simulates data changes every 300 ms

            IsReading = false;

            DataContext = this;

            //yWerte = new double[10000];

            

            /*for (int i = 0; i < testingY.Count + 1; i++)
            {
                yWerte[i] = double.Parse(testingY[i]);
            }*/
        }

        private static readonly Random random = new Random();

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }


        public ChartValues<MeasureModel> ChartValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }

        public double AxisMax
        {
            get { return _axisMax; }
            set
            {
                _axisMax = value;
                OnPropertyChanged("AxisMax");
            }
        }
        public double AxisMin
        {
            get { return _axisMin; }
            set
            {
                _axisMin = value;
                OnPropertyChanged("AxisMin");
            }
        }
        public string DataNumber
        {
            get { return dataNumber; }
            set
            {
                int number;
                bool res = int.TryParse(value, out number);
                if (res) dataNumber = value;
                OnPropertyChanged("DataNumber");
            }
        }
        public string Anfang
        {
            get { return anfang; }
            set
            {
                int number;
                bool res = int.TryParse(value, out number);
                if (res) anfang = value;
                OnPropertyChanged("Anfang");
            }
        }
        public string Ende
        {
            get { return ende; }
            set
            {
                int number;
                bool res = int.TryParse(value, out number);
                if (res) ende = value;
                OnPropertyChanged("Ende");
            }
        }
        public string YWerte
        {
            get { return yWerte1; }
            set
            {
                yWerte1 = value;
                OnPropertyChanged("YWerte");
            }
        }
        public bool IsReading { get; set; }

        private void Read()
        {
            var r = new Random();
            int counter = 0;

            while (IsReading && counter < laenge + 1)
            {
                Thread.Sleep(150);
                var now = DateTime.Now;

                _trend += r.Next(-8, 10);

                ChartValues.Add(new MeasureModel
                {
                    DateTime = now,
                    Value = yWerte[counter]
                });
                counter++;

                SetAxisLimits(now);

                //lets only use the last 150 values
                //if (ChartValues.Count > 150) ChartValues.RemoveAt(0);
            }
        }

        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind
        }

        private void InjectStopOnClick(object sender, RoutedEventArgs e)
        {
            IsReading = !IsReading;
            if (IsReading) Task.Factory.StartNew(Read);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            dNumber = DataNumber;
            string path = @"..\..\ExcelData\HDM_CSV4WQM\" + dNumber + "\\" + dNumber + "_Measurements.csv";
            int counter = 0;
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                var lineArray = line.Split(';');

                for (int i = 0; i < lineArray.Length; i++)
                {
                    if (counter == 0)
                    {
                        data.Add(new List<string>());
                        data[i].Add(lineArray[i]);
                    }
                    else
                    {
                        data[i].Add(lineArray[i]);
                    }
                }
                counter++;
            }

            laenge = int.Parse(Ende) - int.Parse(Anfang);

            yWerte = new double[laenge+1];
            string eingabeY = YWerte;
            int place1 = 1;
            switch (eingabeY)
            {
                case "TS":
                    place1 = 0;
                    break;
                case "X1":
                    place1 = 1;
                    break;
                case "Y1":
                    place1 = 2;
                    break;
                case "Z1":
                    place1 = 3;
                    break;
                case "A1":
                    place1 = 4;
                    break;
                case "B1":
                    place1 = 5;
                    break;
                case "X2":
                    place1 = 6;
                    break;
                case "Y2":
                    place1 = 7;
                    break;
                case "Z2":
                    place1 = 8;
                    break;
                case "ZC":
                    place1 = 9;
                    break;
                case "SC":
                    place1 = 10;
                    break;
                case "SS":
                    place1 = 11;
                    break;
                case "LN":
                    place1 = 12;
                    break;
                case "FX":
                    place1 = 13;
                    break;
                case "FY":
                    place1 = 14;
                    break;
                case "FZ":
                    place1 = 15;
                    break;

            }


            testingY = data[place1];
            testingY.RemoveAt(0);
            int posY = 0;

            for (int i = int.Parse(Anfang); i < int.Parse(Ende) + 1; i++)
            {
                yWerte[posY] = double.Parse(testingY[i]);
                posY++;
            }


            /*for (int i = 0; i < testingY.Count; i++)
            {
                yWerte[i] = double.Parse(testingY[i]);
            }*/
        }
    }
}
