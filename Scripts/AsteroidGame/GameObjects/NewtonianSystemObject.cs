namespace LD54.Scripts.AsteroidGame.GameObjects;

using Engine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class NewtonianSystemObject : GameObject
{
    private NewtonianGravity2DControllerComponent gravitySim;
    public float GravitationalConstant { get; private set; } = -1;

    public NewtonianSystemObject(float gravitationalConstant, string name, Game appCtx) : base(name, appCtx)
    {
        GravitationalConstant = gravitationalConstant;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void OnLoad(GameObject? parentObject)
    {
        gravitySim = new NewtonianGravity2DControllerComponent(GravitationalConstant, "Simulation", this.app);
        this.AddComponent(gravitySim);
    }

    // expose method to be called when all children are set
}
