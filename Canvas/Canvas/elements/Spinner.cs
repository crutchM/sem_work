using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;

namespace OSU
{
    public class Spinner
    {
        private MainWindow _mainWindow;
        private bool isMousePressed;

        public Spinner(MainWindow m)
        {
            _mainWindow = m;
        }

        public void MakeSpinner(int delayTime)
        {
            DispatcherTimer determinate = new DispatcherTimer();
            DispatcherTimer invalidate = new DispatcherTimer();
            int spinCount = 0;
            Ellipse spinner = new Ellipse()
            {
                Width = 400,
                Height = 400,
                Stroke = Brushes.White,
                StrokeThickness = 6,
                Fill = Brushes.Black
            };
            Ellipse holder = new Ellipse()
            {
                Width = 50,
                Height = 50,
                Stroke = Brushes.White,
                StrokeThickness = 6,
                Fill = Brushes.Black
            };
            Ellipse target = new Ellipse()
            {
                Width = 100,
                Height = 100,
                Stroke = Brushes.Black,
                StrokeThickness = 6,
                Fill = Brushes.Green
            };

            holder.MouseLeftButtonDown += (sender, args) => isMousePressed = true;
            holder.MouseLeftButtonUp += (sender, args) => isMousePressed = false;
            holder.MouseMove += (sender, args) =>
            {
                if (isMousePressed)
                {
                    canvas.SetLeft(holder, args.GetPosition(null).X - 25);
                    canvas.SetTop(holder, args.GetPosition(null).Y - 25);
                    if (holderInTarget(holder, target))
                    {
                        spinCount++;
                        if(spinCount / 2 == 0)
                            canvas.SetTop(target, 600);
                        else canvas.SetTop(target, 200);
                    }

                    if (spinCount == 6)
                    {
                        _mainWindow.Canva.Children.Remove(target);
                        _mainWindow.Canva.Children.Remove(holder);
                        _mainWindow.Canva.Children.Remove(spinner);
                        
                    }
                }
            };
            determinate.Interval = TimeSpan.FromMilliseconds(delayTime);
            determinate.Tick += (o, EventArgs) =>
            {
                _mainWindow.Canva.Children.Add(target);
                _mainWindow.Canva.Children.Add(spinner);
                _mainWindow.Canva.Children.Add(holder);
                Canvas.SetLeft(spinner, 400);
                Canvas.SetTop(spinner, 400);
                canvas.SetLeft(holder, 400);
                canvas.SetTop(holder, 600);
                Canvas.SetTop(target, 200);
                canvas.SetLeft(target, 400);
                determinate.Stop();
                invalidate.Start();
            };
            determinate.Start();
            invalidate.Interval = TimeSpan.FromMilliseconds(1000);
            invalidate.Tick += (sender, args) =>
            {
                _mainWindow.Canva.Children.Remove(spinner);
                _mainWindow.Canva.Children.Remove(holder);
                _mainWindow.Canva.Children.Remove(target);
                _mainWindow.changeHealth(_mainWindow.healthBar, -20);
                invalidate.Stop();
            };
        }

        private bool holderInTarget(Ellipse holder, Ellipse target)
        {
            return (Canvas.GetLeft(holder) == Canvas.GetLeft(target) && Canvas.GetTop(holder) == Canvas.GetTop(target))
                   
        }
    }
}