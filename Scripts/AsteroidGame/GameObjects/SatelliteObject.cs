namespace LD54.AsteroidGame.GameObjects;

using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class SatelliteObject : GameObject
{
    private Texture2D? texture;

    public float Mass { get; set; } = 1;

    private Vector3 startingVelocity = Vector3.Zero;

    public SatelliteObject(float mass, Vector3 startingVelocity, Texture2D texture, string name, Game appCtx) : base(name, appCtx)
    {
        this.startingVelocity = startingVelocity;
        this.Mass = mass;

        this.texture = texture;
    }
    public override void OnLoad(GameObject? parentObject)
    {
        float scaleDivider = 4;

        SpriteRendererComponent src = new SpriteRendererComponent("Sprite1", this.app);
        Vector3 textureSize = new Vector3((this.texture.Width / scaleDivider), (this.texture.Height / scaleDivider), 0f);
        Matrix transform = this.GetGlobalTransform();
        transform.Translation -= textureSize / 2;
        src.LoadSpriteData(
            transform,
            new Vector2(textureSize.X, textureSize.Y),
            this.texture,
            null);
        this.AddComponent(src);

        RigidBodyComponent rb = new RigidBodyComponent("SatelliteRB", this.app);
        rb.Mass = this.Mass;
        rb.Velocity = this.startingVelocity;
        this.AddComponent(rb);
    }
}
