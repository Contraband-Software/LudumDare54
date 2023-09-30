namespace LD54.Engine.Leviathan;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class LeviathanSprite
{
    public Texture2D color;
    public Texture2D? normal;
    private Matrix transform;
    public Point size;
    public bool useNormal = false;
    private Game game;
    public LeviathanSprite(Game game, Matrix transform, Point size, Texture2D colorTexture, Texture2D? normalTexture = null) {
        this.color = colorTexture;
        this.normal = normalTexture;
        this.useNormal = normalTexture != null;
        this.transform = transform;
        this.game = game;
        this.size = size;
    }

    public void setTransform(Matrix transform)
    {
        this.transform = transform;
    }
    public Vector3 getPosition()
    {
        return this.transform.Translation;
    }
    public Vector2 getPositionXY()
    {
        return new Vector2(this.transform.Translation.X, this.transform.Translation.Y);
    }
    public void setPosition(Vector3 position) {
        this.transform.Translation = position;
    }
    public void translatePosition(Vector3 translation)
    {
        this.transform.Translation += translation;
    }
}
