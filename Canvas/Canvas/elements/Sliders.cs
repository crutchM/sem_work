using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;
namespace OSU
{
    public class Sliders
    {
        private bool isMousePressed;  
        private MainWindow mw;
        private double constY = 50;
        public Sliders(MainWindow m)
        {
            mw = m;
        }

        public void MakeSlider(int delayTime, int pointX, int pointY, int deltaX, int deltaY, int type, int deathTime)
        {
            DispatcherTimer determinate = new DispatcherTimer();
            DispatcherTimer invalidate = new DispatcherTimer();
            Ellipse slider = new Ellipse()
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.White,
                StrokeThickness = 6,
                Fill = Brushes.Green
            };

            Ellipse target = new Ellipse()
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.Black,
                StrokeThickness = 6,
                Fill = Brushes.Blue
            };
            slider.MouseLeftButtonDown += (sender, args) =>
                isMousePressed = true;
            slider.MouseLeftButtonUp += (sender, args) =>
                isMousePressed = false;
            slider.MouseMove += (sender, args) =>
            {
                
                if (isMousePressed)
                    switch (type)
                    {
                        case 0:
                            if(IsOutOfRange(slider, pointX, pointY, deltaX, deltaY, 0))
                                MoveSlider(slider, args.GetPosition(null).X - 25, pointY);
                            else MoveSlider(slider, pointX, pointY); 
                            break;
                        case 1:
                            if(IsOutOfRange(slider, pointX, pointY, deltaX, deltaY, 1))
                                MoveSlider(slider, pointX, args.GetPosition(null).Y - 25);
                            else MoveSlider(slider, pointX, pointY);
                            break;
                        case 2:
                            if (IsOutOfRange(slider, pointX, pointY, deltaX, deltaY, 0))
                                MoveSlider(slider, args.GetPosition(null).X - 25, 
                                    canvas.GetTop(target) - constY * Math.Sin((args.GetPosition(null).X - pointX) / deltaX * Math.PI) + 25);
                            else MoveSlider(slider, pointX, pointY);
                            break;
                    }
                if (Math.Abs(canvas.GetLeft(slider) - canvas.GetLeft(target)) < 1)
                {
                    mw.Canva.Children.Remove(target);
                    mw.Canva.Children.Remove(slider);
                    mw.changeHealth(mw.healthBar, 10);
                    mw.upScore(mw.tb, 500);
                }
            };
            
            determinate.Interval = TimeSpan.FromMilliseconds(delayTime);
            determinate.Tick += (o, e) => {
                mw.Canva.Children.Add(target);
                mw.Canva.Children.Add(slider);
                canvas.SetLeft(target, pointX + deltaX);
                canvas.SetTop(target, pointY + deltaY);
                canvas.SetLeft(slider, pointX);
                canvas.SetTop(slider, canvas.GetTop(target));
                determinate.Stop();
                invalidate.Start();
            };
            
            determinate.Start();
            invalidate.Interval = TimeSpan.FromMilliseconds(deathTime);
            invalidate.Tick += (o, eventArgs) =>
            {
                mw.Canva.Children.Remove(target);
                mw.Canva.Children.Remove(slider);
                mw.changeHealth(mw.healthBar, -20);
                invalidate.Stop();
            };
        }

        private bool IsOutOfRange (Ellipse slider, double pointX, double pointY,  double dX, double dY, int type)
        {
            bool isOutOfRange = false;
            switch (type)
            {
                case 0:
                    if (dX > 0)
                        isOutOfRange = (Canvas.GetLeft(slider) >= pointX);
                    else isOutOfRange = (Canvas.GetLeft(slider) <= pointX);
                    break;
                case 1:
                    if (dY > 0)
                        isOutOfRange = (Canvas.GetTop(slider) >= pointY);
                    else isOutOfRange = (Canvas.GetTop(slider) <= pointY);
                    break;
            }

            return isOutOfRange;
        }

        private void MoveSlider(Ellipse slider, double posX, double posY)
        {
            Canvas.SetLeft(slider, posX);
            Canvas.SetTop(slider, posY);
        }
    }
}