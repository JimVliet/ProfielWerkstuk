using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;

namespace ProfielWerkstuk.Scripts.Camera
{
	public class CameraManager
	{
		public Camera2D Camera;
		public ProfielWerkstuk MainGame;

		public CameraManager(GraphicsDevice graphicsDevice, ProfielWerkstuk game)
		{
			//Create camera
			Camera = new Camera2D(graphicsDevice)
			{
				MinimumZoom = 0.3f,
				MaximumZoom = 3f,
				Position = new Vector2(-game.Graphics.PreferredBackBufferWidth/2f, -game.Graphics.PreferredBackBufferHeight/2f)
			};

			MainGame = game;
		}

		public void CheckPanning(MouseState mouseState, MouseState oldState)
		{
			Vector2 previousMouse = new Vector2(oldState.X, oldState.Y);

			Vector2 updatedPosition = (previousMouse - new Vector2(mouseState.X, mouseState.Y)) / Camera.Zoom + GetCameraCenterInWorld();
			RectangleF bounds = MainGame.Grid.GridBounds;
			updatedPosition.X = MathHelper.Clamp(updatedPosition.X, bounds.Left, bounds.Right);
			updatedPosition.Y = MathHelper.Clamp(updatedPosition.Y, bounds.Top, bounds.Bottom);

			Camera.Move(updatedPosition - GetCameraCenterInWorld());
		}

		public void CheckScroll(int deltaScroll)
		{
			if (deltaScroll < 0)
				Camera.ZoomOut(0.1f * (-deltaScroll / 120f));
			else
				Camera.ZoomIn(0.1f * (deltaScroll / 120f));
		}

		public Vector2 GetCameraCenterInWorld() => 
			Camera.ScreenToWorld(MainGame.GraphicsDevice.DisplayMode.Width / 2f, MainGame.GraphicsDevice.DisplayMode.Height / 2f);
	}
}
