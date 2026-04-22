using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using System.Text;

namespace Equilibrium_App
{
    public class Item
    {
        public string Title { get; set; } = "Ein U-Boot";
        public ImageSource Image { get; set; }
        public string Description { get; set; } = "Hier sehen wir ein U-Boot";
    }
}