using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Lol_Decay_Reminder
{
    class Templates
    {
        public static Button buttonSetup(string content, int size, Brush background, Brush foreground, HorizontalAlignment contentALignment)
        {
            //Average template for creation of buttons
            Button button = new Button();
            button.Content = content;
            button.FontSize = size;
            button.Background = background;
            button.Foreground = foreground;
            button.HorizontalContentAlignment = contentALignment;
            return button;
        }
    }
}
