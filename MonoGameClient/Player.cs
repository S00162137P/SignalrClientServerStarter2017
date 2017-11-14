
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameData;
using CameraNS;

namespace Sprites
{
    public class Player : AnimatedSprite
    {
        public enum DIRECTION { LEFT, RIGHT, UP, DOWN,STANDING };
        DIRECTION _direction = DIRECTION.STANDING;

        public DIRECTION Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        int _score;
        float _speed;
        Texture2D[] _textures;
        SoundEffect[] _directionSounds;
        SoundEffectInstance _soundPlayer;
        SpriteFont font;
        public PlayerData playerData;
        Position oldPosition;

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }
        int _health;
        public int Health { get { return _health; } set { _health = value; } }

        public Player(Game game, Texture2D[] tx, SoundEffect[] sounds,
            Vector2 pos, int frameCount, 
            int startScore, float speed) 
            : base(game,tx[4],pos,frameCount)
        {
            
            game.Components.Add(this);
            _speed = speed;
            _textures = tx;
            _directionSounds = sounds;
            _health = 100;
            // link in the playerData. Eventually this will be created on the server and sent down.
            playerData = new PlayerData { playerPosition = new Position { X = (int)pos.X, Y = (int)pos.Y } };
            oldPosition = playerData.playerPosition;

        }

        public override void Update(GameTime gameTime)
        {
            PreviousPosition = Position;
            base.Update(gameTime);
            // TODO: Add your update logic here
            _direction = DIRECTION.STANDING;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                _direction = DIRECTION.LEFT;
                base.Move(new Vector2(-1, 0) * _speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                _direction = DIRECTION.UP;
                base.Move(new Vector2(0, -1) * _speed);
            }
            if
            (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                _direction = DIRECTION.DOWN;
                base.Move(new Vector2(0, 1) * _speed);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                _direction = DIRECTION.RIGHT;
                base.Move(new Vector2(1, 0) * _speed);
            }
            
            SpriteImage = _textures[(int)_direction];
            // Update internal player data for messages
            oldPosition = playerData.playerPosition;
            playerData.playerPosition =  new Position { X = (int)Position.X, Y = (int)Position.Y };
            
        }


        //Public override draw
        public override void Draw(GameTime gameTime)
        {
            int imageWidth = BoundingRect.Width;
            string playerTag = playerData.GamerTag;
            SpriteFont font = Game.Services.GetService<SpriteFont>();
            SpriteBatch sp = Game.Services.GetService<SpriteBatch>();
            Vector2 fontSize = font.MeasureString(playerTag);
            Vector2 textPos = BoundingRect.Location.ToVector2() - new Vector2(imageWidth - fontSize.X / 2, fontSize.Y);
            sp.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.CurrentCameraTranslation);
            sp.DrawString(font, playerTag, textPos, Color.White);
            sp.End();
            


            base.Draw(gameTime);
        }
    }
}
