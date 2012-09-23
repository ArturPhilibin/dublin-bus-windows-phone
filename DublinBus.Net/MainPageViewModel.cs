﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace DublinBus.Net
{
    public class MainPageViewModel : DependencyObject
    {
        private readonly SimpleDelegateCommand searchAndDownloadData;

        public static readonly DependencyProperty ResultsProperty = DependencyProperty.Register("Results", typeof(string), typeof(MainPageViewModel), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SearchStringProperty = DependencyProperty.Register("SearchString", typeof(string), typeof(MainPageViewModel), new PropertyMetadata(string.Empty, (s, e) => ((MainPageViewModel)s).SearchAndDownloadData.RaiseCanExecuteChanged()));

        public MainPageViewModel()
        {
            this.searchAndDownloadData = new SimpleDelegateCommand(_ => SearchAndDownloadDataExecute(), _ => this.SearchAndDownloadDataCanExecute());
        }

        public string SearchString
        {
            get
            {
                return (string)GetValue(SearchStringProperty);
            }

            set
            {
                SetValue(SearchStringProperty, value);
            }
        }

        public string Results
        {
            get
            {
                return (string)GetValue(ResultsProperty);
            }

            set
            {
                SetValue(ResultsProperty, value);
            }
        }

        public SimpleDelegateCommand SearchAndDownloadData
        {
            get
            {
                return searchAndDownloadData;
            }
        }

        private bool SearchAndDownloadDataCanExecute()
        {
            int ignored;

            return int.TryParse(this.SearchString, out ignored);
        }

        private void SearchAndDownloadDataExecute()
        {
            var busStopNumber = int.Parse(this.SearchString).ToString().PadLeft(5, '0');
            var wc = new WebClient();
            wc.DownloadStringCompleted += DownloadStringCompleted;
            wc.DownloadStringAsync(new Uri("http://gormcito.com/api.php?n=" + busStopNumber));
        }

        private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            this.Results = string.Concat(Helpers.ExtractTimes(e.Result));
        }
    
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            propertyName.ThrowIfNull(propertyName);

            var handler = this.PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Error
        {
            get { throw new NotImplementedException(); }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "SearchString")
                {
                    int ignored;

                    if (!int.TryParse(this.SearchString, out ignored))
                    {
                        return "Numeric only! IMPROVE ME";
                    }
                }

                return string.Empty;
            }
        }
    }
}