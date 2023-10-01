namespace LD54.AsteroidGame.GameObjects;

using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class BlackHole : GameObject
{
    private Texture2D? texture;

    public float Mass { get; set; } = 1000;

    public BlackHole(float mass, Texture2D texture, string name, Game appCtx) : base(name, appCtx)
    {
        this.Mass = mass;

        this.texture = texture;
    }
    public override void OnLoad(GameObject? parentObject)
    {
        float scaleDivider = 1f;

        SpriteRendererComponent src = new SpriteRendererComponent("Sprite1", this.app);
        Vector3 textureSize = new Vector3((float)this.texture.Width / scaleDivider, (float)this.texture.Height / scaleDivider, 0f);
        Matrix transform = this.GetGlobalTransform();
        PrintLn(transform.Translation.ToString());
        transform.Translation -= textureSize / 2f;
        PrintLn(textureSize.ToString());
        PrintLn(transform.Translation.ToString());
        src.LoadSpriteData(
            transform,
            new Vector2(textureSize.X, textureSize.Y),
            this.texture,
            null);
        this.AddComponent(src);

        RigidBodyComponent rb = new RigidBodyComponent("BlackHoleRB", this.app);
        rb.Mass = this.Mass;
        rb.Static = true;
        this.AddComponent(rb);
    }
}
