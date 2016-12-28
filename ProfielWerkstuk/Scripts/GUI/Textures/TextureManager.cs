using Microsoft.Xna.Framework.Graphics;

namespace ProfielWerkstuk.Scripts.GUI.Textures
{
	public class TextureManager
	{
		public Texture2D Play;
		public Texture2D Pause;
		public Texture2D FastForward;
		public Texture2D FastBackward;
		public Texture2D SkipEnd;
		public Texture2D SkipStart;

		public void Dispose()
		{
			Play.Dispose();
			Pause.Dispose();
			FastForward.Dispose();
			FastBackward.Dispose();
			SkipEnd.Dispose();
			SkipStart.Dispose();
		}
	}
}
