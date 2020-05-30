using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;

namespace OSU
{
    public class Dot
    {
        private MainWindow _mainWindow;

        public Dot(MainWindow m)
        {
            _mainWindow = m;
        }

        public void MakeDot(int posX, int posY, int delayTime, int deathTime)
        {
            DispatcherTimer determinate = new DispatcherTimer();
            DispatcherTimer invalidate = new DispatcherTimer();

            Ellipse target = new Ellipse()
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.White,
                StrokeThickness = 6,
                Fill = Brushes.Black
            };
            target.MouseLeftButtonDown += (sender, args) =>
            {
                _mainWindow.upScore(_mainWindow.tb, 500);
                _mainWindow.Canva.Children.Remove(target); 
                _mainWindow.changeHealth(_mainWindow.healthBar, 10);
            };
            determinate.Interval = TimeSpan.FromMilliseconds(delayTime);
            determinate.Tick += (o, e) => {
                _mainWindow.Canva.Children.Add(target);
                canvas.SetLeft(target, posX);
                canvas.SetTop(target, posY);
                invalidate.Start();
                determinate.Stop();
            };
            determinate.Start();
            invalidate.Interval = TimeSpan.FromMilliseconds(deathTime);
            invalidate.Tick += (o, e) => {
                _mainWindow.Canva.Children.Remove(target);
                _mainWindow.changeHealth(_mainWindow.healthBar, -20);
                invalidate.Stop();
            };
        }
    }
}