using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Kirby_New_Adventure.Animacion._Manager
{
    public static class _ImputManager
    {
        private static Vector2 _direction;
        public static Vector2 Direction => _direction;
        public static bool Moving => _direction != Vector2.Zero;


        public static void Update()
        {
            _direction = Vector2.Zero;
            var keyboardState = Microsoft.Xna.Framework.Input.Keyboard.GetState();

            if (keyboardState.GetPressedKeyCount()> 0)
            {
                if (keyboardState.IsKeyDown(Keys.Left)) _direction.X--;
                if (keyboardState.IsKeyDown(Keys.Right)) _direction.X++;
                if (keyboardState.IsKeyDown(Keys.Up)) _direction.Y--;
                if (keyboardState.IsKeyDown(Keys.Down)) _direction.Y++;
            }
        }
    }
}
