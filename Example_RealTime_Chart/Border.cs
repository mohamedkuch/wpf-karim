using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Example_RealTime_Chart
{
    public class Border : INotifyPropertyChanged
    {
        private string anfang;
        private string ende;
        private string dataNumber;
        private string yWerte;

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
        
        public string YWerte
        {
            get { return yWerte; }
            set
            {
                yWerte = value;
                OnPropertyChanged("YWerte");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}
