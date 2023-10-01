namespace LD54.Engine.Components;

using System;
using System.Collections.Generic;
using System.Linq;
using AsteroidGame.Scenes;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;

public class NewtonianGravity2DControllerComponent : Component
{
    public float GravitationalConstant { get; protected set; } = 1;

    public float ForceLaw { get; set; } = 2.5f;

    public List<RigidBodyComponent> Satellites { get; protected set; } = new List<RigidBodyComponent>();


    public NewtonianGravity2DControllerComponent(float gravitationalConstant, string name, Game appCtx) : base(name, appCtx)
    {
        GravitationalConstant = gravitationalConstant;
    }
    public override void OnLoad(GameObject? parentObject)
    {
        // iterate over children, getting all RigidBody and adding them to a list

        this.gameObject = parentObject;

        ForceLaw = GameScene.FORCE_LAW;

        this.gameObject.ChildAttachedEvent += (gameObject) =>
        {
            PrintLn("NewtonianGravity2DControllerComponent: new object added to newtonian sim.");
            this.RegisterSystemRigidBodies();
        };
    }

    /// <summary>
    /// Iterates over all direct 1st level children and integrates their rigidbodys into the simulation
    /// </summary>
    public void RegisterSystemRigidBodies()
    {
        this.Satellites.Clear();

        foreach (GameObject child in this.gameObject.GetChildren())
        {
            Component? c = child.GetComponent<RigidBodyComponent>();
            if (c is not null)
            {
                this.Satellites.Add((RigidBodyComponent)c);
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Parallel.ForEach(Satellites,
            rb =>
            {
                List<RigidBodyComponent> otherObjects = Satellites.ToList();
                otherObjects.Remove(rb);
                Vector3 acceleration = ResolveGravityAcceleration(rb, otherObjects, this.ForceLaw, this.GravitationalConstant);

                rb.Velocity += acceleration;
            });
    }
    private static Vector3 ResolveGravityAcceleration(RigidBodyComponent rb, List<RigidBodyComponent> otherObjects, float forceLaw, float gravitationalConstant)
    {
        Vector3 sumAcceleration = Vector3.Zero;
        Vector3 position = rb.ContainingGameObject.GetGlobalPosition();

        Parallel.ForEach(otherObjects,
            other =>
            {
                Vector3 otherPosition = other.ContainingGameObject.GetGlobalPosition();

                float distance = Vector3.Distance(position, otherPosition);

                if (distance > 10)
                {
                    Vector3 directionOfSeparation = (otherPosition - position);
                    directionOfSeparation.Normalize();
                    sumAcceleration += gravitationalConstant * other.Mass / (MathF.Pow(distance, forceLaw)) * directionOfSeparation;
                }
            });

        return new Vector3(sumAcceleration.X, sumAcceleration.Y,0);//?
    }

    public override void OnUnload()
    {
        Satellites.Clear();
    }
}
