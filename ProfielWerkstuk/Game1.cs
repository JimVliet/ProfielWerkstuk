using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using ProfielWerkstuk.Scripts;
using System.Windows.Forms;


//Mijn profielwerkstuk
namespace ProfielWerkstuk
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Camera2D camera;
		private MouseState oldState;
		Vector2 previousMouse = new Vector2();
		Grid grid;
		SpriteFont font;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			IsMouseVisible = true;
			grid = new Grid(this, 64, 30, 20, 3);

			// TODO: Add your initialization logic here
			grid.generateTextures();
			base.Initialize();

			Form form = (Form)Form.FromHandle(Window.Handle);
			form.WindowState = System.Windows.Forms.FormWindowState.Maximized;

			//Match the resolution to the resolution of the screen.
			graphics.PreferredBackBufferWidth = form.ClientSize.Width;
			graphics.PreferredBackBufferHeight = form.ClientSize.Height;
			this.Window.AllowUserResizing = true;
			graphics.ApplyChanges();

			//Create camera
			camera = new Camera2D(GraphicsDevice);
			camera.MinimumZoom = 0.7f;
			camera.MaximumZoom = 3f;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
			font = Content.Load<SpriteFont>("Calibri16");
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
			grid.Dispose();
			spriteBatch.Dispose();
			graphics.Dispose();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			KeyboardState state = Keyboard.GetState();
			MouseState mouseState = Mouse.GetState();
			if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
				Exit();

			// TODO: Add your update logic here

			if(System.Windows.Forms.Form.ActiveForm == (System.Windows.Forms.Control.FromHandle(Window.Handle) as System.Windows.Forms.Form))
			{
				if(oldState != null)
				{
					if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
					{
						if(oldState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)
						{
							previousMouse.X = mouseState.X;
							previousMouse.Y = mouseState.Y;
						}
						else if(oldState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
						{
							camera.Move((previousMouse - new Vector2(mouseState.X, mouseState.Y))/camera.Zoom);
							previousMouse.X = mouseState.X;
							previousMouse.Y = mouseState.Y;
						}
					}

					int deltaScroll = mouseState.ScrollWheelValue - oldState.ScrollWheelValue;
					if (deltaScroll != 0)
						if (deltaScroll < 0)
							camera.ZoomOut(0.1f*(-deltaScroll/120f));
						else
							camera.ZoomIn(0.1f*(deltaScroll/120f));
				}
				//Update old mousestate
				oldState = mouseState;
			}
			

			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.WhiteSmoke);
			// TODO: Add your drawing code 

			Vector2 screenOffset = new Vector2(GraphicsDevice.DisplayMode.Width / 2, GraphicsDevice.DisplayMode.Height/2);

			spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());
			grid.DrawGridLines(spriteBatch);
			grid.DrawGridSquares(spriteBatch);
			spriteBatch.End();

			//Draw UI
			spriteBatch.Begin();
			UI.DrawButton(spriteBatch, "Button", 28f, new Vector2(100, 50), font);
			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
