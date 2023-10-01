namespace LD54.AsteroidGame.GameObjects;

using Engine.Collision;
using Engine.Components;
using LD54.Engine.Leviathan;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class StaticSprite : GameObject
{
    private Texture2D? texture;
    private Vector2 position;
    private Vector2 size;
    private SpriteRendererComponent sr;
    private ILeviathanEngineService re;

    public StaticSprite(Texture2D texture, Vector2 position, Vector2 size  ,string name, Game appCtx) : base(name, appCtx)
    {
        this.texture = texture;
        this.position = position;
        this.size = size;
    }

    public override void OnLoad(GameObject? parentObject)
    {
        re = this.app.Services.GetService<ILeviathanEngineService>();
        sr = new SpriteRendererComponent("spriteRenderer",this.app);
        sr.LoadSpriteData(Matrix.CreateTranslation(position.X, position.Y, 0), this.size, texture,0f,null,1);
        this.AddComponent(sr);
    }
    public override void Update(GameTime gameTime)

    {
        base.Update(gameTime);
        //sr.Offset = new Vector3(-re.GetCameraPosition().X,- re.GetCameraPosition().Y, 0);
    }
}
