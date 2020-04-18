using Core.Abstractions;
using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Services.Characters
{
    public class Block : Image, ICharacter
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Block(string imagePath, int height, int width, int x, int y)
        {
            this.Source = new BitmapImage(new Uri(imagePath));
            Height = height;
            Width = width;
            X = x;
            Y = y;
        }
    }
}
