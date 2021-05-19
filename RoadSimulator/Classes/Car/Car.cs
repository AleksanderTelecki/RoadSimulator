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

        public Image RightImage { get; set; }

        public Image LeftImage { get; set; }

        public Point CarPoint { get; set; }

        public double CarSpeed { get; set; }


        public Car()
        {
            RandomiseCar();
        }


        private void RandomiseCar()
        {



            string BodyName = $"Car"+Guid.NewGuid().ToString().Substring(0,4);


            BitmapImage bitmapImage;
            switch (rnd.Next(0,3))
            {
                case 0:
                    bitmapImage = new BitmapImage(new Uri("./Resources/BlueCar.png", UriKind.Relative));
                    break;
                case 1:
                    bitmapImage = new BitmapImage(new Uri("./Resources/RedCar.png", UriKind.Relative));
                    break;
                case 2:
                    bitmapImage = new BitmapImage(new Uri("./Resources/YellowCar.png", UriKind.Relative));
                    break;
                default:
                    throw new Exception();
                    
            }



            CarImage = new Image
            {
                Name = BodyName,
                Source = bitmapImage,
                Height = 50,
                Width = 40,
            };



   


            Thread.Sleep(20);

        }


       

       
        





    }
}
