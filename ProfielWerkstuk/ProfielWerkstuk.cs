using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;
using ProfielWerkstuk.Scripts.Camera;
using ProfielWerkstuk.Scripts.Events;
using ProfielWerkstuk.Scripts.GridManagement;
using ProfielWerkstuk.Scripts.GUI;
using ProfielWerkstuk.Scripts.GUI.Textures;
using ProfielWerkstuk.Scripts.Pathfinding;
using Color = Microsoft.Xna.Framework.Color;

//Mijn profielwerkstuk
namespace ProfielWerkstuk
{
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class ProfielWerkstuk : Game
	{
		public readonly GraphicsDeviceManager Graphics;
		public readonly EventHandlers EventHandlers;
		public readonly TextureManager TextureManager = new TextureManager();
		public Grid Grid;
		public AlgorithmManager AlgorithmManager;
		public UserInterfaceManager UserInterface;
		public CameraManager CameraManager;
		public InputManager InputManager;

		private SpriteBatch _spriteBatch;

		public ProfielWerkstuk()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			Graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
			EventHandlers = new EventHandlers();
		}

		private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
		{
			//Setup antialiasing and monitor device
			Graphics.PreferMultiSampling = true;
			Graphics.GraphicsProfile = GraphicsProfile.HiDef;
			Graphics.SynchronizeWithVerticalRetrace = true;
			Graphics.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
			e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 8;
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
			Grid = new Grid(this, 64, 30, 20, 2);
			Grid.GenerateGrid();

			base.Initialize();

			AlgorithmManager = new AlgorithmManager(this);
			InputManager = new InputManager(this);

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
			UserInterface = new UserInterfaceManager(this);

			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);
			UserInterface.Font28 = Content.Load<SpriteFont>("Raleway28");
			UserInterface.Font24 = Content.Load<SpriteFont>("Raleway24");
			UserInterface.Font16 = Content.Load<SpriteFont>("Raleway16");

			//Load controlMenu textures
			TextureManager.Play = Content.Load<Texture2D>("Play");
			TextureManager.Pause = Content.Load<Texture2D>("Pause");
			TextureManager.FastForward = Content.Load<Texture2D>("FastForward");
			TextureManager.FastBackward = Content.Load<Texture2D>("FastBackward");
			TextureManager.SkipEnd = Content.Load<Texture2D>("SkipEnd");
			TextureManager.SkipStart = Content.Load<Texture2D>("SkipStart");
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			_spriteBatch.Dispose();
			Graphics.Dispose();
			TextureManager.Dispose();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{

			if(Form.ActiveForm == (Control.FromHandle(Window.Handle) as Form))
			{
				InputManager.Update();
				UserInterface.Update(gameTime);
				AlgorithmManager.Update(gameTime);
				Grid.Update(gameTime);
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

			_spriteBatch.Begin(transformMatrix: CameraManager.Camera.GetViewMatrix());
			Grid.DrawGridLines(_spriteBatch);
			Grid.DrawGridSquares(_spriteBatch);
			AlgorithmManager.Draw(_spriteBatch, gameTime);
			_spriteBatch.End();

			//Draw UI
			_spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend);
			UserInterface.Draw(_spriteBatch, gameTime);
			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
