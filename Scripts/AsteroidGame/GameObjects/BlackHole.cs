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
        SpriteRendererComponent src = new SpriteRendererComponent("Sprite1", this.app);
        src.LoadSpriteData(
            this.GetGlobalTransform(),
            new Point(this.texture.Width, this.texture.Height),
            this.texture,
            null);
        this.AddComponent(src);

        RigidBodyComponent rb = new RigidBodyComponent("BlackHoleRB", this.app);
        rb.Mass = this.Mass;
        this.AddComponent(rb);
    }
}
