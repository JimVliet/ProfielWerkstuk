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
		public Game1 MainGame;

		public CameraManager(GraphicsDevice graphicsDevice, Game1 game)
		{
			//Create camera
			Camera = new Camera2D(graphicsDevice)
			{
				MinimumZoom = 0.3f,
				MaximumZoom = 3f
			};
			
			MainGame = game;
		}

		public void CheckPanning(MouseState mouseState, MouseState oldState, Vector2 previousMouse)
		{
			if (mouseState.LeftButton == ButtonState.Pressed && 
				oldState.LeftButton == ButtonState.Pressed)
			{
				Vector2 updatedPosition = (previousMouse - new Vector2(mouseState.X, mouseState.Y))/Camera.Zoom + GetCameraCenterInWorld();
				RectangleF bounds = MainGame.Grid.GridBounds;
				updatedPosition.X = MathHelper.Clamp(updatedPosition.X, bounds.Left, bounds.Right);
				updatedPosition.Y = MathHelper.Clamp(updatedPosition.Y, bounds.Top, bounds.Bottom);

				Camera.Move(updatedPosition - GetCameraCenterInWorld());
			}
		}

		public void CheckScroll(MouseState mouseState, MouseState oldState)
		{
			int deltaScroll = mouseState.ScrollWheelValue - oldState.ScrollWheelValue;
			if (deltaScroll == 0) return;
			if (deltaScroll < 0)
				Camera.ZoomOut(0.1f * (-deltaScroll / 120f));
			else
				Camera.ZoomIn(0.1f * (deltaScroll / 120f));
		}

		public Vector2 GetCameraCenterInWorld() => 
			Camera.ScreenToWorld(MainGame.GraphicsDevice.DisplayMode.Width / 2f, MainGame.GraphicsDevice.DisplayMode.Height / 2f);
	}
}
