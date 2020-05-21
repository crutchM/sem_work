using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;
namespace Canvas
{
    public class Sliders
    {
        private static MainWindow mw;

        public Sliders(MainWindow m)
        {
            mw = m;
        }

        public void MakeSlider(int delayTime, int pointX, int pointY, int deltaX, int type, int deathTime)
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
                Fill = Brushes.Black
            };

            /*slider.MouseLeftButtonDown += (sender, args) =>
            {
               
            };*/
            slider.MouseMove += (sender, args) =>
            {
                /*bool timeFlag = true;
                invalidate.Interval = TimeSpan.FromMilliseconds(deathTime);
                */

                switch (type)
                {
                    case 0:
                        canvas.SetLeft(slider, args.GetPosition(null).X);
                        canvas.SetTop(slider, pointY);
                    
                        break;
                    case 1:
                        canvas.SetLeft(slider, pointY);
                        canvas.SetTop(slider, args.GetPosition(null).Y);
                        break;
                    case 2:
                        canvas.SetLeft(slider, args.GetPosition(null).X);
                        canvas.SetTop(slider, args.GetPosition(null).Y);
                        break;
                }

                /*invalidate.Tick += (o, eventArgs) =>
                {
                    timeFlag = false;
                    mw.Canva.Children.Remove(target);
                    mw.changeHealth(mw.healthBar, -20);
                    invalidate.Stop();
                };*/
            };
            determinate.Interval = TimeSpan.FromMilliseconds(delayTime);
            determinate.Tick += (o, e) => {
                mw.Canva.Children.Add(target);
                mw.Canva.Children.Add(slider);
                canvas.SetLeft(target, pointX + deltaX);
                canvas.SetTop(target, pointY + deltaX);
                canvas.SetLeft(slider, pointX);
                canvas.SetTop(slider, pointY);
                determinate.Stop();
            };
        }
    }
}