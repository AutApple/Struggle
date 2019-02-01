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
        private bool lmb_Hold;
        private Vector2f lmb_Hold_Coords; 

        public bool LeftMouseButton_Hold
        {
            get
            {
                return lmb_Hold; 
            }
        }

        public Vector2f LeftMouseButton_Hold_Coords
        {
            get
            {
                return lmb_Hold_Coords;
            }
        }
        
        public EventHandler(ref Fraction fr)
        {
            this.fr = fr;
            lmb_Hold = false;
            lmb_Hold_Coords = new Vector2f(-1, -1);
        }

        public void Window_Closed(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public void Mouse_Pressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                lmb_Hold = true;
                lmb_Hold_Coords = new Vector2f(e.X, e.Y);

                fr.SelectEntity(new Vector2f(e.X, e.Y));
            } else if (e.Button == Mouse.Button.Right)
            {
                fr.UnlockEntities();
                fr.UpdateEntitiesMovement(new Vector2f(e.X, e.Y));
                fr.LockEntities();
            }
        }
        
        public void Mouse_Released(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                RectangleShape rect = new RectangleShape();
                rect.Position = lmb_Hold_Coords;
                rect.Size = new Vector2f(e.X - lmb_Hold_Coords.X, e.Y - lmb_Hold_Coords.Y);
                fr.SelectEntityRectangle(rect);

                lmb_Hold = false;
                lmb_Hold_Coords = new Vector2f(-1, -1);
            }
        }

        public void Mouse_Moved(object sender, MouseMoveEventArgs e)
        {
            
        }

        public void Key_Pressed(object sender, KeyEventArgs e)
        {
            switch (e.Code)
            {
                case Keyboard.Key.Escape:
                    fr.DeselectAll();
                    break;
                case Keyboard.Key.D:
                    fr.UnlockEntities();
                    break;
            }
        }
    }
}
