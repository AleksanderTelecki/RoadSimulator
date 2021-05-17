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


        public void MoveCarMatrix(Car car)
        {



            
            // Create a MatrixTransform. This transform
            // will be used to move the button.
            MatrixTransform ImageMatrixTransform = new MatrixTransform();


            car.CarImage.RenderTransform = ImageMatrixTransform;

            // Register the transform's name with the page
            // so that it can be targeted by a Storyboard.
            string id = Guid.NewGuid().ToString().Substring(0, 4);
            mainWindow.RegisterName($"ImageMatrixTransform{id}", ImageMatrixTransform);



            // Create the animation path.
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();

            pFigure.StartPoint = new Point(5, 201);
            PolyBezierSegment pBezierSegment = new PolyBezierSegment();

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

            pFigure.Segments.Add(pBezierSegment);
            animationPath.Figures.Add(pFigure);

            // Freeze the PathGeometry for performance benefits. Dont know for what 
            animationPath.Freeze();

            // Create a MatrixAnimationUsingPath to move the
            // button along the path by animating
            // its MatrixTransform.
            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = animationPath;

            // Set IsOffsetCumulative to true so that the animation
            // values accumulate when its repeats.
            matrixAnimation.IsOffsetCumulative = true;
            matrixAnimation.Duration = TimeSpan.FromSeconds(car.CarSpeed);
            //matrixAnimation.RepeatBehavior = new RepeatBehavior(2);

            // Set the animation to target the Matrix property
            // of the MatrixTransform named "ButtonMatrixTransform".
            Storyboard.SetTargetName(matrixAnimation, $"ImageMatrixTransform{id}");
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));

            
            // Create a Storyboard to contain and apply the animation.
            Storyboard pathAnimationStoryboard = new Storyboard();
            pathAnimationStoryboard.Children.Add(matrixAnimation);
            pathAnimationStoryboard.Completed += CarAnimationStoryboard_Completed;



            car.Animator = pathAnimationStoryboard;
            pathAnimationStoryboard.Begin(mainWindow,true);

        
            

        }

        private void CarAnimationStoryboard_Completed(object sender, EventArgs e)
        {
        
            foreach (var item in Manager.CarCollection)
            {
                var state = item.Animator.GetCurrentProgress(mainWindow);
                
                //Use progres value to turn car from right to left in future
                if (state.Value==1)
                {
                    item.Animator.Stop(mainWindow);
                    item.CarImage.Visibility = Visibility.Hidden;
                    Manager.CarCollection.Remove(item);
                    break;
                }
               

            }



        

        }

        public void MoveTrainMatrix(Train train)
        {




            // Create a MatrixTransform. This transform
            // will be used to move the button.
            MatrixTransform ImageMatrixTransform = new MatrixTransform();


            train.TrainImage.RenderTransform = ImageMatrixTransform;

            // Register the transform's name with the page
            // so that it can be targeted by a Storyboard.
            string id = Guid.NewGuid().ToString().Substring(0, 4);
            mainWindow.RegisterName($"ImageMatrixTransform{id}", ImageMatrixTransform);



            // Create the animation path.
            PathGeometry animationPath = new PathGeometry();
            PathFigure pFigure = new PathFigure();
            PolyBezierSegment pBezierSegment = new PolyBezierSegment();


            switch (train.TrainDirection)
            {
                case Train.Direction.None:

                    break;
                case Train.Direction.Left:
                    pFigure.StartPoint = new Point(800, 230);


                    pBezierSegment.Points.Add(new Point(700, 230));
                    pBezierSegment.Points.Add(new Point(300, 230));
                    pBezierSegment.Points.Add(new Point(-100, 230));

                    break;
                case Train.Direction.Right:

                    pFigure.StartPoint = new Point(13, 230);

                    pBezierSegment.Points.Add(new Point(100, 230));
                    pBezierSegment.Points.Add(new Point(300, 230));
                    pBezierSegment.Points.Add(new Point(950, 230));


                    break;
                default:
                    break;
            }

            

            pFigure.Segments.Add(pBezierSegment);
            animationPath.Figures.Add(pFigure);

            // Freeze the PathGeometry for performance benefits. Dont know for what 
            animationPath.Freeze();

            // Create a MatrixAnimationUsingPath to move the
            // button along the path by animating
            // its MatrixTransform.
            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = animationPath;

            // Set IsOffsetCumulative to true so that the animation
            // values accumulate when its repeats.
            matrixAnimation.IsOffsetCumulative = true;
            matrixAnimation.Duration = TimeSpan.FromSeconds(train.TrainSpeed);
            //matrixAnimation.RepeatBehavior = new RepeatBehavior(2);

            // Set the animation to target the Matrix property
            // of the MatrixTransform named "ButtonMatrixTransform".
            Storyboard.SetTargetName(matrixAnimation, $"ImageMatrixTransform{id}");
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));


            // Create a Storyboard to contain and apply the animation.
            Storyboard pathAnimationStoryboard = new Storyboard();
            pathAnimationStoryboard.Children.Add(matrixAnimation);
            pathAnimationStoryboard.Completed += TrainAnimationStoryboard_Completed;



            train.Animator = pathAnimationStoryboard;
            pathAnimationStoryboard.Begin(mainWindow, true);




        }

        private void TrainAnimationStoryboard_Completed(object sender, EventArgs e)
        {
            foreach (var item in Manager.TrainCollection)
            {
                var state = item.Animator.GetCurrentProgress(mainWindow);

                //Use progres value to turn car from right to left in future
                if (state.Value == 1)
                {
                    item.Animator.Stop(mainWindow);
                    item.TrainImage.Visibility = Visibility.Hidden;
                    Manager.TrainCollection.Remove(item);
                    break;
                }


            }

        }
    }
}
