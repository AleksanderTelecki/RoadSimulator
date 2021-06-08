using RoadSimulator.Classes.Train;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RoadSimulator
{
    public class PathBuilder
    {
        public MainWindow mainWindow;

        public Canvas MapCanvas;

        public PathBuilder(MainWindow mainwindow,Canvas canvas)
        {
            MapCanvas = canvas;
            this.mainWindow = mainwindow;
        }

        /// <summary>
        /// metoda przemiesczajaca obiekty klasy Car po Canvasie
        /// </summary>
        /// <param name="car"></param>
        public void MoveCarMatrix(Car car)
        {
            //stworzenie MatrixTransform przy uzyciu ktoego samochodziki beda przemiszczaly sie po Canvasie
            MatrixTransform ImageMatrixTransform = new MatrixTransform(); 
            car.CarImage.RenderTransform = ImageMatrixTransform;

            //nadanie ID samochodzikowi by Storyboard mogl sie do niego odwolac
            string id = Guid.NewGuid().ToString().Substring(0, 4);
            mainWindow.RegisterName($"ImageMatrixTransform{id}", ImageMatrixTransform);

            //stworzenie sciezki po ktorej beda poruszaly sie samochodziki
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();

            //punkt w ktorym pojawiaja sie samochodziki po stworzeniu
            pFigure.StartPoint = new Point(5, 201);
            //PolyBezierSegment dla poruszania sie samochodziku po wyznaczonej sciezce
            PolyBezierSegment pBezierSegment = new PolyBezierSegment();

            // TODO: poprawic troche punkty, zeby autka sie nie zderzaly

            //punkty do ktorych kolejno beda dazyly samochodziki z punktu startowego            
            pBezierSegment.Points.Add(new Point(10, 201));
            pBezierSegment.Points.Add(new Point(300, 201));
            pBezierSegment.Points.Add(new Point(620, 201));
            
            pBezierSegment.Points.Add(new Point(640, 219));
            pBezierSegment.Points.Add(new Point(690, 242));
            pBezierSegment.Points.Add(new Point(645, 335));
            
            pBezierSegment.Points.Add(new Point(535, 345));
            pBezierSegment.Points.Add(new Point(300, 345));
            pBezierSegment.Points.Add(new Point(160, 345));
            
            pBezierSegment.Points.Add(new Point(80, 370));
            pBezierSegment.Points.Add(new Point(70, 450));
            pBezierSegment.Points.Add(new Point(120, 540));
            
            pBezierSegment.Points.Add(new Point(184, 566));
            pBezierSegment.Points.Add(new Point(300, 566));
            pBezierSegment.Points.Add(new Point(920, 566));

            //dodanie punktow do sciezki
            pFigure.Segments.Add(pBezierSegment);
            //dodanie sciezki do sciezki animacji
            animationPath.Figures.Add(pFigure);
           
            //stworzenie instancji klasy MatrixAnimationUsingPath, kotra przemieszcza samochodzik
            //po sciezce animacji             
            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = animationPath;

            //przesunięcie wygenerowane przez animowaną matrycę beda sumowane z każdym powtórzeniem            
            matrixAnimation.IsOffsetCumulative = true;
            // TODO: potestowac predkosc
            //vvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvvv
            matrixAnimation.Duration = TimeSpan.FromSeconds(8); // z 9
            //^^^^^predkosc do testowania

            //ustawienie animacji dla wlasciwosci MatrixTransform obiektu o wskazanym ID
            Storyboard.SetTargetName(matrixAnimation, $"ImageMatrixTransform{id}");
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

            //Storyboard kontroluje wszystkie animacje dodane jako jego dzieci
            Storyboard pathAnimationStoryboard = new Storyboard();
            pathAnimationStoryboard.Children.Add(matrixAnimation);
            
            //aktualny progres animacji
            pathAnimationStoryboard.Completed += CarAnimationStoryboard_Completed;

            car.Animator = pathAnimationStoryboard;
            car.matrixTransform = matrixAnimation;

            pathAnimationStoryboard.Begin(mainWindow,true);
            //ustalenie predkosci animacji dla animowanego samochodziku
            pathAnimationStoryboard.SetSpeedRatio(mainWindow, car.CarSpeed);
        }

        /// <summary>
        /// metoda zawierajaca akcje dla samochodziku, ktory przebyl cala sciezke animacji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarAnimationStoryboard_Completed(object sender, EventArgs e)
        {   //dla kazdego itemu w kolekcji sprawdzany jest stan animacji
            foreach (var item in Manager.CarCollection)
            {
                var state = item.Animator.GetCurrentProgress(mainWindow);
                
                //jezeli animacja dobiegla konca samochodzik jest usuniety z kolekcji oraz z Canvasu
                if (state.Value==1)
                {
                    item.Animator.Stop(mainWindow);
                    MapCanvas.Children.Remove(item.CarImage);                   
                    Manager.CarCollection.Remove(item);   
                    break;
                }               
            }
        }

        /// <summary>
        /// metoda animujaca obiekty klasy Train
        /// </summary>
        /// <param name="train"></param>
        public void MoveTrainMatrix(Train train)
        {
            //stworzenie MatrixTransform przy uzyciu ktorego pociag bedzie poruszal sie na Canvasie
            MatrixTransform ImageMatrixTransform = new MatrixTransform();
            train.TrainImage.RenderTransform = ImageMatrixTransform;

            //nadanie ID pociagowi by Storyboard mogl sie do niego odwolac
            string id = Guid.NewGuid().ToString().Substring(0, 4);
            mainWindow.RegisterName($"ImageMatrixTransform{id}", ImageMatrixTransform);

            //stworzenie sciezki po ktorej bedzie poruszal sie pociag
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();

            //PolyBezierSegment do przemieszczania pociagu po wskazanych punktach
            PolyBezierSegment pBezierSegment = new PolyBezierSegment();

            //punkty po ktorych bedzie poruszal sie pociag w zaleznosci od wylosowanego dla niego kierunku
            switch (train.TrainDirection)
            {
                case Train.Direction.None:

                    break;
                case Train.Direction.Left:
                    //punkt w ktorym pojawi sie pociag zaraz po stworzeniu
                    pFigure.StartPoint = new Point(800, 230);

                    pBezierSegment.Points.Add(new Point(700, 230));
                    pBezierSegment.Points.Add(new Point(400, 230));
                    pBezierSegment.Points.Add(new Point(-100, 230));

                    break;
                case Train.Direction.Right:
                    //punkt w ktorym pojawi sie pociag zaraz po stworzeniu
                    pFigure.StartPoint = new Point(13, 230);

                    pBezierSegment.Points.Add(new Point(100, 230));
                    pBezierSegment.Points.Add(new Point(300, 230));
                    pBezierSegment.Points.Add(new Point(950, 230));

                    break;
                default:
                    break;
            }

            //dodanie punktow do sciezki
            pFigure.Segments.Add(pBezierSegment);
            //dodanie sciezki do sciezki animacji
            animationPath.Figures.Add(pFigure);

            //stworzenie instancji klasy MatrixAnimationUsingPath, ktora przesuwa pociag 
            //po sciezce animacji  
            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = animationPath;

            //przesunięcie wygenerowane przez animowaną matrycę beda sumowane z każdym powtórzeniem   
            matrixAnimation.IsOffsetCumulative = true;
            // TODO: predkosc do adjustacji
            matrixAnimation.Duration = TimeSpan.FromSeconds(8);
            //^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
                        
            //ustawienie animacji dla wlasciwosci MatrixTransform obiektu o wskazanym ID
            Storyboard.SetTargetName(matrixAnimation, $"ImageMatrixTransform{id}");
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

            //Storyboard kontroluje wszystkie animacje dodane jako jego dzieci
            Storyboard pathAnimationStoryboard = new Storyboard();
            pathAnimationStoryboard.Children.Add(matrixAnimation);
            pathAnimationStoryboard.Completed += TrainAnimationStoryboard_Completed;

            train.Animator = pathAnimationStoryboard;
            pathAnimationStoryboard.Begin(mainWindow, true);
            //ustawienie predkosci animacji dla pociagu
            pathAnimationStoryboard.SetSpeedRatio(mainWindow, train.TrainSpeed);
        }

        /// <summary>
        /// metoda zawierajaca akcje dla obiektu klasy Train, ktory pokonal cala sciezke animacji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrainAnimationStoryboard_Completed(object sender, EventArgs e)
        {  
            //dla kazdego itemu w kolekcji sprawdzany jest stan animacji
            foreach (var item in Manager.TrainCollection)
            {
                var state = item.Animator.GetCurrentProgress(mainWindow);

                //jezeli animacja dobiegla konca pociag jest usuniety z kolekcji oraz z Canvasu
                if (state.Value == 1)
                {
                    item.Animator.Stop(mainWindow);                   
                    Manager.TrainCollection.Remove(item);
                    MapCanvas.Children.Remove(item.TrainImage);                 
                    break;
                }
            }
        }
    }
}
