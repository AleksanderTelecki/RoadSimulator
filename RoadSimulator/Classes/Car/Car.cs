using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RoadSimulator
{
    public class Car
    {
        private static Random rnd = new Random();

        public Storyboard Animator;

        public MatrixAnimationUsingPath matrixTransform;

        public Image CarImage { get; set; }

        public bool IsRight { get; set; }

        public string Color { get; set; }

        public Point CarPoint { get; set; }

        public double CarSpeed { get; set; }

        public bool Stopped = false;


        public Car()
        {
            IsRight = true;
            RandomiseCar();
        }

        /// <summary>
        /// metoda sluzy losowemu wybraniu i stworzeniu obrazka dla instancji klasy Car
        /// </summary>
        private void RandomiseCar()
        {
            //nadanie ID obrazkowi instancji klasy Car
            string BodyName = $"Car"+Guid.NewGuid().ToString().Substring(0,4);

            BitmapImage bitmapImage;
            //losowanie koloru samochodzika
            switch (rnd.Next(0,3))
            {
                case 0:
                    bitmapImage = new BitmapImage(new Uri("./Resources/BlueCarRight.png", UriKind.Relative));
                    Color = "Blue";
                    break;
                case 1:
                    bitmapImage = new BitmapImage(new Uri("./Resources/RedCarRight.png", UriKind.Relative));
                    Color = "Red";
                    break;
                case 2:
                    bitmapImage = new BitmapImage(new Uri("./Resources/YellowCarRight.png", UriKind.Relative));
                    Color = "Yellow";
                    break;
                default:
                    throw new Exception();
                    
            }
            //ustalenie parametrow obrazka dla instancji klasy Car
            CarImage = new Image
            {
                Name = BodyName,
                Source = bitmapImage,
                Height = 50,
                Width = 40,
            };

            //uspienie watku by losowanie koloru samochodzika mialo sens
            Thread.Sleep(20);
        }
        /// <summary>
        /// metoda zamienia obrazek samochodzika na jego lustrzane odbicie
        /// </summary>
        public void FlipCar()
        {
            BitmapImage bitmapImage;
            switch (IsRight)
            {
                case true:
                    bitmapImage = new BitmapImage(new Uri($"./Resources/{Color}CarLeft.png", UriKind.Relative));
                    IsRight = false;
                    break;
                case false:
                    bitmapImage = new BitmapImage(new Uri($"./Resources/{Color}CarRight.png", UriKind.Relative));
                    IsRight = true;
                    break;
                default:
                    bitmapImage = new BitmapImage(new Uri("./Resources/Mapa.png", UriKind.Relative));
                    break;
            }

            CarImage.Source = bitmapImage;
        }      

    }
}
