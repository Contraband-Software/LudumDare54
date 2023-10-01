namespace LD54.Engine.Components;

using Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SpriteRendererComponent : Component
{
    private LeviathanSprite? sprite;

    private int spriteID;

    public Vector3 Offset = Vector3.Zero;

    public SpriteRendererComponent(string name, Game appCtx) : base(name, appCtx)
    {

    }

    public void LoadSpriteData(Matrix transform, Vector2 size, Texture2D colorTexture, Texture2D? normalTexture = null)
    {
        sprite = new(this.app, transform,0 , size, colorTexture, normalTexture);

        spriteID = this.app.Services.GetService<ILeviathanEngineService>().addSprite(sprite);
    }

    public override void OnLoad(GameObject? parentObject)
    {
        gameObject = parentObject;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        Matrix transform = gameObject.GetGlobalTransform();
        transform.Translation += this.Offset;
        sprite.SetTransform(transform);
    }

    public override void OnUnload()
    {
        this.app.Services.GetService<ILeviathanEngineService>().removeSprite(spriteID);
        // PrintLn("OnUnload: SpriteRendererComponent");
    }
}
