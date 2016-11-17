using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Windows.Forms;
using ProfielWerkstuk.Scripts.Camera;
using ProfielWerkstuk.Scripts.Grid;
using Button = ProfielWerkstuk.Scripts.GUI.Button;


//Mijn profielwerkstuk
namespace ProfielWerkstuk
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		public GraphicsDeviceManager Graphics;
		public SpriteBatch SpriteBatch;
		private MouseState _oldState;
		public Vector2 PreviousMouse;
		public Grid Grid;
		public SpriteFont Font28;
		public SpriteFont Font24;
		public Scripts.GUI.Menu TestMenu;
		public CameraManager CameraManager;

		public Game1()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			Graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
		}

		private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
			//Form form = (Form)Control.FromHandle(Window.Handle);

			//Setup antialiasing and 
			Graphics.PreferMultiSampling = true;
			Graphics.GraphicsProfile = GraphicsProfile.HiDef;
			Graphics.SynchronizeWithVerticalRetrace = true;
			Graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
			e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 16;
			Window.AllowUserResizing = true;
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
			Grid = new Grid(this, 64, 30, 20, 3);

			// TODO: Add your initialization logic here
			Grid.GenerateTextures();
			base.Initialize();

			Form form = (Form)Control.FromHandle(Window.Handle);
			form.WindowState = FormWindowState.Maximized;

			Graphics.PreferredBackBufferWidth = form.ClientSize.Width;
			Graphics.PreferredBackBufferHeight = form.ClientSize.Height;
			Graphics.ApplyChanges();

			CameraManager = new CameraManager(GraphicsDevice, this);
			TestMenu = new Scripts.GUI.Menu(new Vector2(form.ClientSize.Width/2, form.ClientSize.Height/2));

			Button exitButton = new Scripts.GUI.Button(Font28, new Vector2(), "Exit")
			{
				ButtonSize = {X = 250f}
			};

			TestMenu.AddButton(exitButton);
			TestMenu.AddButton(new Scripts.GUI.Button(Font28, new Vector2(), "Back"));
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			Font28 = Content.Load<SpriteFont>("Raleway28");
			Font24 = Content.Load<SpriteFont>("Raleway24");
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
			Grid.Dispose();
			SpriteBatch.Dispose();
			Graphics.Dispose();
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

			// TODO: Add your update logic here

			if(Form.ActiveForm == (Control.FromHandle(Window.Handle) as Form))
			{
				CameraManager.CheckPanning(mouseState, _oldState, PreviousMouse);
				CameraManager.CheckScroll(mouseState, _oldState);

				PreviousMouse.X = mouseState.X;
				PreviousMouse.Y = mouseState.Y;
				//Update old mousestate
				_oldState = mouseState;
			}
			
			if (state.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
				Exit();
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

			SpriteBatch.Begin(transformMatrix: CameraManager.Camera.GetViewMatrix());
			Grid.DrawGridLines(SpriteBatch);
			Grid.DrawGridSquares(SpriteBatch);
			SpriteBatch.End();

			//Draw UI
			SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			TestMenu.Draw(SpriteBatch);
			SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
