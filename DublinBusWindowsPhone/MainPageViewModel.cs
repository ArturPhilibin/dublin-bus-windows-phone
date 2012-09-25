﻿//-------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="Artur Philibin E Silva">
//     Copyright (c) Artur Philibin E Silva All rights reserved.
// </copyright>
//-------------------------------------------------------------------------

namespace DublinBusWindowsPhone
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Net;
    using System.Windows;

    public class MainPageViewModel : DependencyObject
    {
        private readonly SimpleDelegateCommand searchAndDownloadData;

        public static readonly DependencyProperty ResultsProperty = DependencyProperty.Register("Results", typeof(string), typeof(MainPageViewModel), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SearchStringProperty = DependencyProperty.Register("SearchString", typeof(string), typeof(MainPageViewModel), new PropertyMetadata(string.Empty, (s, e) => ((MainPageViewModel)s).SearchAndDownloadData.RaiseCanExecuteChanged()));

        public MainPageViewModel()
        {
            this.searchAndDownloadData = new SimpleDelegateCommand(_ => this.SearchAndDownloadDataExecute(), _ => this.SearchAndDownloadDataCanExecute());
        }

        public string SearchString
        {
            get
            {
                return (string)this.GetValue(SearchStringProperty);
            }

            set
            {
                this.SetValue(SearchStringProperty, value);
            }
        }

        public string Results
        {
            get
            {
                return (string)this.GetValue(ResultsProperty);
            }

            set
            {
                this.SetValue(ResultsProperty, value);
            }
        }

        public SimpleDelegateCommand SearchAndDownloadData
        {
            get
            {
                return this.searchAndDownloadData;
            }
        }

        private bool SearchAndDownloadDataCanExecute()
        {
            int ignored;

            return int.TryParse(this.SearchString, out ignored);
        }

        private void SearchAndDownloadDataExecute()
        {
            var busStopNumber = int.Parse(this.SearchString).ToString(NumberFormatInfo.InvariantInfo).PadLeft(5, '0');
            var wc = new WebClient();
            wc.Headers[HttpRequestHeader.ContentType] = "text/xml";
            wc.UploadStringCompleted += this.WcOnUploadStringCompleted;
            wc.UploadStringAsync(new Uri("http://webservice.dublinbus.biznetservers.com/DublinBusRTPIService.asmx?op=GetRealTimeStopData"), this.post);
        }

        private void WcOnUploadStringCompleted(object sender, UploadStringCompletedEventArgs uploadStringCompletedEventArgs)
        {
            throw new NotImplementedException();
        }

        private string post = "<?xml version=\"1.0\" encoding=\"utf-8\"?><soap:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"><soap:Body><GetRealTimeStopData xmlns=\"http://dublinbus.ie/\"><stopId>1377</stopId><forceRefresh>false</forceRefresh></GetRealTimeStopData></soap:Body></soap:Envelope>";

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            Contract.Requires(propertyName != null);

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