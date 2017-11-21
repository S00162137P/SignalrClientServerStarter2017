using System;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;

using Sprites;
using Microsoft.Xna.Framework.Audio;
using CameraNS;
using GameData;

namespace MonoGameClient
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Server Variables
        HubConnection serverConnection;
        IHubProxy proxy;


        Vector2 worldCoords;
        SpriteFont messageFont;
        Texture2D backGround;
        private string connectionMessage;
        private bool Connected;
        private Rectangle worldRect;
        private bool Joined;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

    
        protected override void Initialize()
        {

            //
            serverConnection = new HubConnection("http://s00162137pctestgameserver1.azurewebsites.net/");

            // serverConnection = new HubConnection("http://localhost:3566/"); //Port used
            serverConnection.StateChanged += ServerConnection_StateChanged;
            proxy = serverConnection.CreateHubProxy("GameHub"); // creates proxy to enable us to call methods from gamehub
            connectionMessage = string.Empty;
            serverConnection.Start(); // starts server conection
            base.Initialize();
        }

        private void ServerConnection_StateChanged(StateChange State)
        {
            switch (State.NewState)
            {
                case ConnectionState.Connected:
                    connectionMessage = "Connected......";
                    Connected = true;
                    startGame();
                    break;

                case ConnectionState.Disconnected:
                    connectionMessage = "Disconnected.....";
                    if (State.OldState == ConnectionState.Connected)
                        connectionMessage = "Lost Connection....";
                    Connected = false;
                    break;

                case ConnectionState.Connecting:
                    connectionMessage = "Connecting.....";
                    Connected = false;
                    break;
            }
        }

        private void startGame()
        {
            //gets called once connected (valid conection)

            Action<int, int> joined = cJoined; // delegate pattern
            proxy.On("joined", joined); // join message
            proxy.Invoke("join"); // runs join comand above
                        
        }

        private void cJoined(int arg1, int arg2)
        {
            worldCoords = new Vector2(arg1, arg2); // sets to co-ords passed down
            // Setup Camera
            //create world Rect, does clamping

            worldRect = new Rectangle(new Point(0, 0), worldCoords.ToPoint());
            new Camera(this, Vector2.Zero, worldCoords);

            Joined = true;
            // Setup Player / Delegate code 
            //Load each texture assosiated with each movement
            new Player(this, new Texture2D[] {
                            Content.Load<Texture2D>(@"Textures\left"),
                            Content.Load<Texture2D>(@"Textures\right"),
                            Content.Load<Texture2D>(@"Textures\up"),
                            Content.Load<Texture2D>(@"Textures\down"),
                            Content.Load<Texture2D>(@"Textures\stand"),
                        }, new SoundEffect[] { }, GraphicsDevice.Viewport.Bounds.Center.ToVector2(),
                        8, 0, 5.0f);

            //invoke joinplayer method, expects 2 arguments
            //Invokes joinplayer message, must have same signature as above
            proxy.Invoke<PlayerData>("JoinPlayer",
                new Position {X = GraphicsDevice.Viewport.Bounds.Center.X,
                             Y = GraphicsDevice.Viewport.Bounds.Center.Y})
                            .ContinueWith(
                //starts new thread
                            (p) => {
                                if (p.Result == null)
                                    connectionMessage = "No PlayerData returned";
                                else
                                {
                                    Player player;
                                    player = (Player)Components.FirstOrDefault(p1 =>p1.GetType() == typeof(Player));
                                    if (player != null)
                                    {
                                        player.playerData = p.Result;

                                    }

                                    //change the player draw method to display the name/id
                                }

                            }); 

        }

      
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            messageFont = Content.Load<SpriteFont>(@"Fonts\ScoreFont");
            Services.AddService(Content.Load<SpriteFont>(@"Fonts\PlayerFont"));
            backGround = Content.Load<Texture2D>(@"Textures\background");
        }

      
        protected override void UnloadContent()
        {

        }

    
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!Connected && !Joined) return;

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

         
            base.Update(gameTime);
        }

      
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (Connected && Joined) // ensure connected
            {
                DrawPlay();
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(messageFont, connectionMessage,
                                new Vector2(20, 20), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        private void DrawPlay()
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Camera.CurrentCameraTranslation);
            spriteBatch.Draw(backGround, worldRect, Color.White); // draw background using co-ords
            spriteBatch.End();
        }
    }
}
