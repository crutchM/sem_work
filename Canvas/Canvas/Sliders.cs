using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;
namespace Canvas
{
    public class Sliders
    {
        private bool isMpusePressed;  
            
        private MainWindow mw;

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
                Fill = Brushes.Blue
            };
            slider.MouseLeftButtonDown += (sender, args) =>
                isMpusePressed = true;
            slider.MouseLeftButtonUp += (sender, args) =>
                isMpusePressed = false;
            slider.MouseMove += (sender, args) =>
            {
                
                if (isMpusePressed)
                    switch (type)
                    {
                        case 0:
                            canvas.SetLeft(slider, args.GetPosition(null).X - 25);
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
                canvas.SetTop(target, pointY);
                canvas.SetLeft(slider, pointX);
                canvas.SetTop(slider, pointY);
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
    }
}