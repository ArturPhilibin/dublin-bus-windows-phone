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

namespace DublinBusWindowsPhone.Model
{
    public class BusStop
    {
        public int Number { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public string Address { get; set; }

        public int Hits { get; set; }
        public string Name { get; set; }
        public Color Color { get; set; }
    }
}
