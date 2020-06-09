using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;

namespace OSU
{
    public class Spinner : IElement 
    {
        private Ellipse spinner = new Ellipse()                            
                {
                    Width = 600,
                    Height = 600,
                    Stroke = Brushes.Black,
                    StrokeThickness = 6
                };
        private Ellipse holder = new Ellipse()                            
                {
                    Width = 50,
                    Height = 50,
                    Stroke = Brushes.White,
                    StrokeThickness = 6,
                    Fill = Brushes.LightBlue
                };
                private Ellipse target = new Ellipse()                            
                {
                    Width = 100,
                    Height = 100,
                    Stroke = Brushes.White,
                    StrokeThickness = 6,
                    Fill = Brushes.Green
                };

        private MainWindow _mainWindow;
        private bool isMousePressed;
        private int spinCount;
        public int PosX { get; }
        public int PosY { get; }
        public int DelayTime { get; }
        public int DeathTime { get; }

        public Spinner(MainWindow m, int pX, int pY, int dT, int deT)
        {
            PosX = pX;
            PosY = pY;
            DelayTime = dT;
            DeathTime = deT;
            _mainWindow = m;
        }

        private void MakeSpinner()
        {
            _mainWindow.Canva.Children.Add(spinner);
            _mainWindow.Canva.Children.Add(holder);
            _mainWindow.Canva.Children.Add(target);
            Canvas.SetLeft(spinner, 100);
            Canvas.SetTop(spinner, 100);
            canvas.SetLeft(holder, Canvas.GetLeft(spinner) + 280);
            canvas.SetTop(holder, 600);
            Canvas.SetTop(target, Canvas.GetTop(spinner) + 75);
            canvas.SetLeft(target, Canvas.GetLeft(spinner) + 250);
            holder.MouseLeftButtonDown += (sender, args) => isMousePressed = true;
            holder.MouseLeftButtonUp += (sender, args) => isMousePressed = false;
            holder.MouseMove += (sender, args) =>
            {
                if (isMousePressed)
                {
                    canvas.SetLeft(holder, args.GetPosition(null).X - 25);
                    canvas.SetTop(holder, args.GetPosition(null).Y - 25);
                }
            };
            target.MouseEnter += (o, eventArgs) =>
            {
                spinCount++;
                if (spinCount % 2 == 0 )
                    Canvas.SetTop(target, Canvas.GetTop(spinner) + 75);
                else Canvas.SetTop(target, (Canvas.GetTop(spinner) + spinner.Height / 2) + 150);
                _mainWindow.UpScore(_mainWindow.Tb, 100);
            };
        }

        public void Show() =>
            MakeSpinner(); 

        public void Remove()
        {
            _mainWindow.Canva.Children.Remove(spinner);                   
            _mainWindow.Canva.Children.Remove(holder);
            _mainWindow.Canva.Children.Remove(target);
        }
    }
}