﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace RoadSimulator.Classes.TrafficTools
{  
    /// <summary>
    /// klasa zawiera metody zmieniajace obrazek rogatek z rogatek zamknietych na rogatki otwarte i z na odwrot
    /// </summary>
    public static class RailwayGates
    {      
        public static MainWindow MainWindow { get; set; }
        //zmienna boolowska okreslajaca czy rogatki sa opuszczone
        public static bool IsClosed;

        /// <summary>
        /// metoda zmienia obrazek rogatek otwartych na obrazek z rogatkami zamknietymi, wywoluje metode zmieniajaca kolor sygnalizatora na zielony 
        /// oraz ustawia wartosc true dla zmiennej IsClosed
        /// </summary>
        public static void CloseGates()
        {  
            MainWindow.RightGates.Source = new BitmapImage(new Uri("./Resources/GateCloseRight.png", UriKind.Relative));
            MainWindow.LeftGates.Source = new BitmapImage(new Uri("./Resources/GateCloseLeft.png", UriKind.Relative));
            TrafficLights.SetRed();            
            IsClosed = true;

        }

        /// <summary>
        /// metoda zmienia obrazek rogatek zamknietych na rogatki otwarte, wywoluje metode zmieniajaca kolor sygnalizatora na zielony
        /// oraz ustawia wartosc false dla zmiennej IsClosed
        /// </summary>
        public static void OpenGates()
        {
            MainWindow.RightGates.Source = new BitmapImage(new Uri("./Resources/GateOpenRight.png", UriKind.Relative));
            MainWindow.LeftGates.Source = new BitmapImage(new Uri("./Resources/GateOpenLeft.png", UriKind.Relative));
            TrafficLights.SetGreen();
            IsClosed = false;
        }
    }
}
