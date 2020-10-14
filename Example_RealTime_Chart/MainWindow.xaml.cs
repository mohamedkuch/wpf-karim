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
using Microsoft.Win32;

namespace Example_RealTime_Chart
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //Variables real time 
        private double _axisMax;
        private double _axisMin;
        private double _trend;
        double[] yValues_rt;
        private string _dataName_RT;
        private string anfang_rt;
        private string ende_rt;
        private string yAxis_rt;

        List<List<string>> data_rt = new List<List<string>>();
        List<string> testingY_rt;
        string filePath_rt;

        int laenge_rt;

        //Variables XY-plot
        double[] yValues_xy;
        string[] xValues_xy;
        private string _dataName_xy;
        private string anfang_xy;
        private string ende_xy;

        List<List<string>> data_xy = new List<List<string>>();
        string filePath_xy;
        int laenge_xy;
        List<string> testingY_xy;
        List<string> testingX_xy;



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

            // initializing Data
            SeriesCollection = new SeriesCollection();
            DataName_RT = "keine Datei";
            DataName_XY = "keine Datei";

            ComboBox_X.Items.Add(new ComboBoxItem { Content = "keine Datei" });
            ComboBox_X.SelectedIndex = 0;

            ComboBox_Y.Items.Add(new ComboBoxItem { Content = "keine Datei" });
            ComboBox_Y.SelectedIndex = 0;

            ComboBox_Y_RT.Items.Add(new ComboBoxItem { Content = "keine Datei" });
            ComboBox_Y_RT.SelectedIndex = 0;

            DataContext = this;


        }
        /*
        private static readonly Random random = new Random();

        private static double RandomNumberBetween(double minValue, double maxValue)
        {
            var next = random.NextDouble();

            return minValue + (next * (maxValue - minValue));
        }
        */

        public ChartValues<MeasureModel> ChartValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }


        //Properties real time plot
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
        public string DataName_RT
        {
            get { return _dataName_RT; }
            set
            {
                _dataName_RT = value;
                OnPropertyChanged("DataName_RT");
            }
        }

        public void OpenFile_Click_RT(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "_Measurements"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV files (*.csv)|*.csv|XML files (*.xml)|*.xml"; // Filter files by extension
            dlg.InitialDirectory = Environment.CurrentDirectory ;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.SafeFileName;
                DataName_RT = filename;
                filePath_rt = dlg.FileName;

                get_Entries_RT();

            }
        }

        public void get_Entries_RT()
        {
            string path = filePath_rt;
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                var lineArray = line.Split(';');
                ComboBox_Y_RT.Items.Remove(ComboBox_Y_RT.Items.GetItemAt(0));

                for (int i = 0; i < lineArray.Length; i++)
                {
                    ComboBox_Y_RT.Items.Add(new ComboBoxItem { Content = lineArray[i] });
                }
                ComboBox_Y_RT.SelectedIndex = 0;
                break;
            }
        }

        public string Anfang_RT
        {
            get { return anfang_rt; }
            set
            {
                int number;
                bool res = int.TryParse(value, out number);
                if (res) anfang_rt = value;
                OnPropertyChanged("Anfang_RT");
            }
        }
        public string Ende_RT
        {
            get { return ende_rt; }
            set
            {
                int number;
                bool res = int.TryParse(value, out number);
                if (res) ende_rt = value;
                OnPropertyChanged("Ende_RT");
            }
        }
        public string YAxis_RT
        {
            get { return yAxis_rt; }
            set
            {
                yAxis_rt = value;
                OnPropertyChanged("YAxis_RT");
            }
        }
        //Properties xy plot
        public string DataName_XY
        {
            get { return _dataName_xy; }
            set
            {
                _dataName_xy = value;
                OnPropertyChanged("DataName_XY");
            }
        }

        public void OpenFile_Click_XY(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "_Measurements"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "CSV files (*.csv)|*.csv|XML files (*.xml)|*.xml"; // Filter files by extension
            dlg.InitialDirectory = Environment.CurrentDirectory ;

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dlg.SafeFileName;
                DataName_XY = filename;
                filePath_xy = dlg.FileName;
                // get X-Axis , Y-Axis
                get_Entries_XY();

            }
        }

        public void get_Entries_XY()
        {
            string path = filePath_xy;
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                var lineArray = line.Split(';');
                ComboBox_X.Items.Remove(ComboBox_X.Items.GetItemAt(0));
                ComboBox_Y.Items.Remove(ComboBox_Y.Items.GetItemAt(0));

                for (int i = 0; i < lineArray.Length; i++)
                {
                    ComboBox_X.Items.Add(new ComboBoxItem { Content = lineArray[i] });
                    ComboBox_Y.Items.Add(new ComboBoxItem { Content = lineArray[i] });
                }
                ComboBox_X.SelectedIndex = 0;
                ComboBox_Y.SelectedIndex = 1;

                break;
            }
        }

        public string Anfang_XY
        {
            get { return anfang_xy; }
            set
            {
                int number;
                bool res = int.TryParse(value, out number);
                if (res) anfang_xy = value;
                OnPropertyChanged("Anfang_XY");
            }
        }
        public string Ende_XY
        {
            get { return ende_xy; }
            set
            {
                int number;
                bool res = int.TryParse(value, out number);
                if (res) ende_xy = value;
                OnPropertyChanged("Ende_XY");
            }
        }
       
        //Real-time-plot functions
        public bool IsReading { get; set; }

        private void Read()
        {
            var r = new Random();
            int counter = 0;

            while (IsReading && counter < laenge_rt + 1)
            {
                Thread.Sleep(150);
                var now = DateTime.Now;

                _trend += r.Next(-8, 10);

                ChartValues.Add(new MeasureModel
                {
                    DateTime = now,
                    Value = yValues_rt[counter]
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

        private void Generate_Click_realtime(object sender, RoutedEventArgs e)
        {
            string path = filePath_rt;
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
                        data_rt.Add(new List<string>());
                        data_rt[i].Add(lineArray[i]);
                    }
                    else
                    {
                        data_rt[i].Add(lineArray[i]);
                    }
                }
                counter++;
            }

            laenge_rt = int.Parse(Ende_RT) - int.Parse(Anfang_RT);

            yValues_rt = new double[laenge_rt + 1];
            ComboBoxItem selected_Y_RT = (ComboBoxItem)ComboBox_Y_RT.SelectedItem;
            string eingabeY_rt = selected_Y_RT.Content.ToString();
            int placeY_rt = 1;
            switch (eingabeY_rt)
            {
                case "TS":
                    placeY_rt = 0;
                    break;
                case "X1":
                    placeY_rt = 1;
                    break;
                case "Y1":
                    placeY_rt = 2;
                    break;
                case "Z1":
                    placeY_rt = 3;
                    break;
                case "A1":
                    placeY_rt = 4;
                    break;
                case "B1":
                    placeY_rt = 5;
                    break;
                case "X2":
                    placeY_rt = 6;
                    break;
                case "Y2":
                    placeY_rt = 7;
                    break;
                case "Z2":
                    placeY_rt = 8;
                    break;
                case "ZC":
                    placeY_rt = 9;
                    break;
                case "SC":
                    placeY_rt = 10;
                    break;
                case "SS":
                    placeY_rt = 11;
                    break;
                case "LN":
                    placeY_rt = 12;
                    break;
                case "FX":
                    placeY_rt = 13;
                    break;
                case "FY":
                    placeY_rt = 14;
                    break;
                case "FZ":
                    placeY_rt = 15;
                    break;

            }


            testingY_rt = data_rt[placeY_rt];
            testingY_rt.RemoveAt(0);
            int posY_rt = 0;

            for (int i = int.Parse(Anfang_RT); i < int.Parse(Ende_RT) + 1; i++)
            {
                yValues_rt[posY_rt] = double.Parse(testingY_rt[i]);
                posY_rt++;
            }



        }

        //XY-plot functions
        private void Generate_Click_xy(object sender, RoutedEventArgs e)
        {
            string path = filePath_xy;
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
                        data_xy.Add(new List<string>());
                        data_xy[i].Add(lineArray[i]);
                    }
                    else
                    {
                        data_xy[i].Add(lineArray[i]);
                    }
                }
                counter++;
            }

            laenge_xy = int.Parse(Ende_XY) - int.Parse(Anfang_XY);

            yValues_xy = new double[laenge_xy + 1];
            xValues_xy = new string[laenge_xy + 1];

            ComboBoxItem selected_Y = (ComboBoxItem)ComboBox_Y.SelectedItem;
            string eingabeY_xy = selected_Y.Content.ToString();

            ComboBoxItem selected_X = (ComboBoxItem)ComboBox_X.SelectedItem;
            string eingabeX_xy = selected_X.Content.ToString();

            int placeY_xy = 99;
            int placeX_xy = 99;
            switch (eingabeY_xy)
            {
                case "TS":
                    placeY_xy = 0;
                    break;
                case "X1":
                    placeY_xy = 1;
                    break;
                case "Y1":
                    placeY_xy = 2;
                    break;
                case "Z1":
                    placeY_xy = 3;
                    break;
                case "A1":
                    placeY_xy = 4;
                    break;
                case "B1":
                    placeY_xy = 5;
                    break;
                case "X2":
                    placeY_xy = 6;
                    break;
                case "Y2":
                    placeY_xy = 7;
                    break;
                case "Z2":
                    placeY_xy = 8;
                    break;
                case "ZC":
                    placeY_xy = 9;
                    break;
                case "SC":
                    placeY_xy = 10;
                    break;
                case "SS":
                    placeY_xy = 11;
                    break;
                case "LN":
                    placeY_xy = 12;
                    break;
                case "FX":
                    placeY_xy = 13;
                    break;
                case "FY":
                    placeY_xy = 14;
                    break;
                case "FZ":
                    placeY_xy = 15;
                    break;

            }
            switch (eingabeX_xy)
            {
                case "TS":
                    placeX_xy = 0;
                    break;
                case "X1":
                    placeX_xy = 1;
                    break;
                case "Y1":
                    placeX_xy = 2;
                    break;
                case "Z1":
                    placeX_xy = 3;
                    break;
                case "A1":
                    placeX_xy = 4;
                    break;
                case "B1":
                    placeX_xy = 5;
                    break;
                case "X2":
                    placeX_xy = 6;
                    break;
                case "Y2":
                    placeX_xy = 7;
                    break;
                case "Z2":
                    placeX_xy = 8;
                    break;
                case "ZC":
                    placeX_xy = 9;
                    break;
                case "SC":
                    placeX_xy = 10;
                    break;
                case "SS":
                    placeX_xy = 11;
                    break;
                case "LN":
                    placeX_xy = 12;
                    break;
                case "FX":
                    placeX_xy = 13;
                    break;
                case "FY":
                    placeX_xy = 14;
                    break;
                case "FZ":
                    placeX_xy = 15;
                    break;

            }
            try
            {
                testingY_xy = data_xy[placeY_xy];
                testingY_xy.RemoveAt(0);
                testingX_xy = data_xy[placeX_xy];
                testingX_xy.RemoveAt(0);
                int posY_xy = 0;
                int posX_xy = 0;


                for (int i = int.Parse(Anfang_XY); i < int.Parse(Ende_XY) + 1; i++)
                {
                    yValues_xy[posY_xy] = double.Parse(testingY_xy[i]);
                    posY_xy++;
                }

                for (int i = int.Parse(Anfang_XY); i < int.Parse(Ende_XY) + 1; i++)
                {
                    xValues_xy[posX_xy] = testingX_xy[i];
                    posX_xy++;
                }
            }
            catch (Exception)
            {

                MessageBox.Show("invalid eingaben!");
            }
            

        }
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SeriesCollection.Add(
                new LineSeries
                {
                    Title = DataName_XY.Remove(DataName_XY.IndexOf("_")),
                    Values = new ChartValues<double> (yValues_xy),
                    PointGeometry = null

                }
            );

            Labels = xValues_xy;
            YFormatter = value => value.ToString();

        }
    }
}
