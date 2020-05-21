using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace Canvas
{
    public class LevelPoints : IEnumerable<Tuple<int, int, int, int, int>>
    {

        private string filePath;
        private StreamReader sr;

        public LevelPoints(string fileName)
        {
            this.filePath = fileName;
            openFile();
        }

        private void openFile()
        {
            try
            {
                this.sr = new StreamReader(filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine("[E]: " + e);
                if (this.sr == null)
                {
                    MessageBox.Show(
                        "Возникла непредвиденная ошибка: отсутствует один или более файлов уровней",
                        "Ошибка!",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
        }
        
        public IEnumerator<Tuple<int, int, int, int, int>> GetEnumerator()
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if(line[0] == '#') continue;
                string[] values = line.Split(';');
                if(int.TryParse(values[0], out int type)
                   && int.TryParse(values[1], out int x)
                   && int.TryParse(values[2], out int y)
                   && int.TryParse(values[3], out int when)
                   && int.TryParse(values[4], out int duration))
                    yield return Tuple.Create(type, x, y, when, duration);
                else
                    yield break;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}