namespace LD54.Engine.Dev;

using Collision;
using Components;
using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

class DebugPlayer : GameObject
{
    Texture2D texture;
    public float Speed = 5f;
    ColliderComponent collider;
    RigidBodyComponent rb;

    public DebugPlayer(Texture2D texture, string name, Game appCtx) : base(name, appCtx)
    {
        this.texture = texture;

        Matrix pos = this.GetLocalTransform();
        pos.Translation = new Vector3(150, 150, 1);

        this.SetLocalTransform(pos);
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

        Vector3 colliderDimensions = new Vector3(this.texture.Width, this.texture.Height, 0);
        collider = new ColliderComponent(colliderDimensions, Vector3.Zero, "playerCollider", this.app);
        this.AddComponent(collider);

        rb = new RigidBodyComponent("rbPlayer", app);
        this.AddComponent(rb);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        rb.Velocity = Vector3.Zero;
        Vector3 preMovePosition = this.GetLocalPosition();

        Move();

        this.SetLocalPosition(preMovePosition + rb.Velocity);

        // this.app.Services.GetService<ICollisionSystemService>().RequestResolve(collider);
    }


    private void Move()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            rb.Velocity.X -= Speed;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            rb.Velocity.X += Speed;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            rb.Velocity.Y -= Speed;
        }
        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            rb.Velocity.Y += Speed;
        }
    }
}