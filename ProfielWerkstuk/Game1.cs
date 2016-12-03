using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using ProfielWerkstuk.Scripts.Camera;
using ProfielWerkstuk.Scripts.Events;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI;
using ProfielWerkstuk.Scripts.Pathfinding;
using Color = Microsoft.Xna.Framework.Color;

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
		public Grid Grid;
		public AlgorithmManager AlgorithmManager;
		public UserInterfaceManager UserInterface;
		public CameraManager CameraManager;
		public InputManager InputManager;
		public CustomEvents CustomEvents;

		public Game1()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			Graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
		}

		private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
			//Setup antialiasing and monitor device
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
			CustomEvents = new CustomEvents();

			IsMouseVisible = true;
			Grid = new Grid(this, 64, 60, 40, 2);
			Grid.GenerateGrid();
			AlgorithmManager = new AlgorithmManager(this);

			// TODO: Add your initialization logic here
			base.Initialize();

			Form form = (Form)Control.FromHandle(Window.Handle);
			form.WindowState = FormWindowState.Maximized;

			Graphics.PreferredBackBufferWidth = form.ClientSize.Width;
			Graphics.PreferredBackBufferHeight = form.ClientSize.Height;
			form.MinimumSize = new Size(form.ClientSize.Width , form.ClientSize.Height);
			Graphics.ApplyChanges();

			CameraManager = new CameraManager(GraphicsDevice, this);
			UserInterface.Setup();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			InputManager = new InputManager(this);
			UserInterface = new UserInterfaceManager(this);

			// Create a new SpriteBatch, which can be used to draw textures.
			SpriteBatch = new SpriteBatch(GraphicsDevice);
			UserInterface.Font28 = Content.Load<SpriteFont>("Raleway28");
			UserInterface.Font24 = Content.Load<SpriteFont>("Raleway24");
			UserInterface.Font16 = Content.Load<SpriteFont>("Raleway16");
			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
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
			// TODO: Add your update logic here

			if(Form.ActiveForm == (Control.FromHandle(Window.Handle) as Form))
			{
				InputManager.Update();
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

			SpriteBatch.Begin(transformMatrix: CameraManager.Camera.GetViewMatrix());
			Grid.DrawGridLines(SpriteBatch);
			Grid.DrawGridSquares(SpriteBatch);
			AlgorithmManager.Draw(SpriteBatch, gameTime);
			Grid.DrawDragElement(SpriteBatch);
			SpriteBatch.End();

			//Draw UI
			SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
			UserInterface.Draw(SpriteBatch, gameTime);
			SpriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
