
// Type: GameManager.StringBatchExtensions
// Assembly: BionicleRpg, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A3C16972-042F-4654-B76B-0749FB030FA7
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace GameManager
{
  public static class StringBatchExtensions
  {
    public static void DrawString(
      this SpriteBatch batch,
      SpriteFont font,
      string text,
      Vector2 position,
      Color color,
      TextAlignment alignment)
    {
      switch (alignment)
      {
        case TextAlignment.Center:
          position.X -= font.MeasureString(text).X / 2f;
          break;
        case TextAlignment.Left:
          position.X -= font.MeasureString(text).X;
          break;
      }
      batch.DrawString(font, text, position, color);
    }

    public static void DrawString(
      this SpriteBatch batch,
      SpriteFont font,
      string text,
      Vector2 position,
      Color color,
      float scale,
      TextAlignment alignment)
    {
      switch (alignment)
      {
        case TextAlignment.Center:
          position.X -= font.MeasureString(text).X / 2f * scale;
          break;
        case TextAlignment.Left:
          position.X -= font.MeasureString(text).X * scale;
          break;
      }
      batch.DrawString(font, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 1f);
    }
  }
}
