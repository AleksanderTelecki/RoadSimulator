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
    /// <summary>
    /// klasa definiujaca pociag
    /// </summary>
    public class Train
    {
        private static Random rnd = new Random();

        public Storyboard Animator;

        public Image TrainImage { get; set; }

        public enum Direction { None,Left,Right};

        public Direction TrainDirection = Direction.None;

        public Point TrainPoint { get; set; }

        public double TrainSpeed { get; set; }

        public Train()
        {
            RandomiseTrain();
        }

        /// <summary>
        /// metoda tworzy obiekt klasy Train
        /// </summary>
        private void RandomiseTrain()
        {
            //nadanie ID obrazkowi instancji klasy Car
            string BodyName = $"Train" + Guid.NewGuid().ToString().Substring(0, 4);

            //losowe wybranie kierunku w ktorym porusza sie pociag
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
        }
    }
}
