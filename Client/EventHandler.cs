using System;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;

namespace Struggle
{
    class EventHandler
    {
        private Fraction fr;

        public EventHandler(ref Fraction fr)
        {
            this.fr = fr;
        }
        public void Window_Closed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public void Mouse_Pressed(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("click");
            fr.SelectEntity(new Vector2f(e.X, e.Y));
        }

        public void Mouse_Moved(object sender, MouseMoveEventArgs e)
        {
             
        }
    }
}
