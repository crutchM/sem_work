using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using canvas = System.Windows.Controls.Canvas;

namespace OSU
{
    public class Dot : IElement
    {
        private MainWindow _mainWindow;
        public int PosX { get; }
        public int PosY { get; }
        public int DelayTime { get; }
        public int DeathTime { get; }
        
        private Ellipse target = new Ellipse()                                         
                {
                    Width = 50,
                    Height = 50,
                    Stroke = Brushes.Cyan,
                    StrokeThickness = 6,//
                    Fill = Brushes.Black
                };

        public Dot(MainWindow m, int pX, int pY, int dT, int deT)
        {
            PosX = pX;
            PosY = pY;
            DelayTime = dT;
            DeathTime = deT;
            _mainWindow = m;
        }

        private void MakeDot()
        {
            target.MouseLeftButtonDown += (sender, args) =>
            {
                _mainWindow.UpScore(_mainWindow.Tb, 500);
                _mainWindow.Canva.Children.Remove(target); 
                _mainWindow.ChangeHealth(_mainWindow.HealthBar, 10);
            };
            _mainWindow.Canva.Children.Add(target);
            canvas.SetLeft(target, PosX);
            canvas.SetTop(target, PosY);
        }

        

        public void Show() =>
            MakeDot(); 
        
        public void Remove()
        {
            _mainWindow.Canva.Children.Remove(target);                            
            _mainWindow.ChangeHealth(_mainWindow.HealthBar, -20);
            
        }
    }
}