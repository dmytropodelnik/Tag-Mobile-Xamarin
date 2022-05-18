using System;
using System.Collections.Generic;
using System.Text;

namespace App1.Models
{
    public class CubeInfo
    {
        public string Text { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsFree { get; set; } = true;

        public CubeInfo() { }
        public CubeInfo(string text, int x, int y, bool isFree)
        {
            Text = text;
            X = x;
            Y = y;
            IsFree = isFree;
        }
    }
}
