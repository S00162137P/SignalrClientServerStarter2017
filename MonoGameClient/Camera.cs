using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CameraNS
{
    class Camera : GameComponent
    {


        static Vector2 _camPos = Vector2.Zero;
        Vector2 _worldBound;
        static public Matrix CurrentCameraTranslation { get
            {
                return Matrix.CreateTranslation(new Vector3(
                    -CamPos,
                    0));
            } }

        static public Vector2 CamPos
        {
            get
            {
                return _camPos;
            }

            set
            {
                _camPos = value;
            }
        }

        public Camera(Game game, Vector2 startPos, Vector2 bound): base(game)
        {
            game.Components.Add(this); // uses component style
            CamPos = startPos; // set based on parameters passed to it
            _worldBound = bound;
        }

        public override void Update(GameTime gameTime)
        {
            Player p;
            p = (Player)Game.Components.FirstOrDefault(pl => pl.GetType() == typeof(Player)); // if theres a player component in components, it returns that component
            if (p != null) // prevenets an error crash
                follow(p.Position,Game.GraphicsDevice.Viewport);

            base.Update(gameTime);
        }

        public void move(Vector2 delta, Viewport v)
        {
            CamPos += delta;
            CamPos = Vector2.Clamp(CamPos, Vector2.Zero, _worldBound - new Vector2(v.Width, v.Height));
        }

        public void follow(Vector2 followPos, Viewport v)
        {
            _camPos = followPos - new Vector2(v.Width / 2, v.Height / 2); // sets position
            _camPos = Vector2.Clamp(_camPos, Vector2.Zero, _worldBound - new Vector2(v.Width, v.Height)); // clamp new position
        }

    }
}
