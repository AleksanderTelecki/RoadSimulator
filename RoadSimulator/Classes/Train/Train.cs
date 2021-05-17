using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace RoadSimulator.Classes.Train
{
    public class Train
    {
        private static Random rnd = new Random();

        public Storyboard Animator;

        public Image TrainImage { get; set; }

        public enum Direction { None,Left,Right};

        public Direction TrainDirection = Direction.None;

        public Point TrainPoint { get; set; }

        public int TrainSpeed { get; set; }


        public Train(Point CPoint)
        {
            this.TrainPoint = CPoint;
            RandomiseTrain();
        }

        private void RandomiseTrain()
        {



            string BodyName = $"Train" + Guid.NewGuid().ToString().Substring(0, 4);


            BitmapImage bitmapImage;
            switch (rnd.Next(0, 2))
            {
                case 0:
                    bitmapImage = new BitmapImage(new Uri("./Resources/TrainLeft.png", UriKind.Relative));
                    TrainDirection = Direction.Left;
                    break;
                case 1:
                    bitmapImage = new BitmapImage(new Uri("./Resources/TrainRight.png", UriKind.Relative));
                    TrainDirection = Direction.Right;
                    break;
                default:
                    throw new Exception();

            }



            TrainImage = new Image
            {
                Name = BodyName,
                Source = bitmapImage,
                Height = 70,
                Width = 60,
            };







            Thread.Sleep(20);

        }


    }
}
