namespace LD54.Engine;

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

public abstract class GameObject : EngineObject, IUpdateable
{
    private bool initalized = false;

    public bool Enabled { get; }
    public int UpdateOrder { get; }
    public event EventHandler<EventArgs>? EnabledChanged;
    public event EventHandler<EventArgs>? UpdateOrderChanged;

    private GameObject? _parent;

    // self initialization
    protected GameObject? parent;

    protected List<GameObject> children = new();

    protected Matrix transform;
    protected List<Component> components = new();

    public GameObject(string name, Game appCtx) : base(name, appCtx)
    {
        this.transform = Matrix.Identity;
    }

    public virtual void Update(GameTime gameTime)
    {
        this.UpdateComponents(gameTime);
    }

    public override void OnUnload()
    {
        this.UnloadComponents();
    }

    #region SCENE_GRAPH
    public Matrix GetLocalTransform()
    {
        return this.transform;
    }

    public void SetLocalTransform(Matrix transform)
    {
        this.transform = transform;
    }

    public Matrix GetGlobalTransform()
    {
        //                                  |-  Statement null if there is no parent.
        //                                  |                       |-  Null-coalescing operator makes these parenthesis
        //                                  V                       V   evaluate to the identity matrix if the above is null.
        return this.transform * (this.parent?.GetGlobalTransform() ?? Matrix.Identity);
    }

    /// <summary>
    /// Parents the a child to this gameObject.
    /// </summary>
    /// <param name="gameObject"></param>
    public void AddChild(GameObject gameObject)
    {
        gameObject.parent?.RemoveChild(gameObject);
        gameObject.parent = this;
        this.children.Add(gameObject);
    }

    public List<GameObject> GetChildren()
    {
        return this.children;
    }

    public void RemoveChild(GameObject gameObject)
    {
        this.children.Remove(gameObject);
        gameObject.parent = null;
    }

    public void ClearChildren()
    {
        foreach (GameObject child in this.children)
        {
            child.parent = null;
        }
        this.children.Clear();
        GC.Collect();
    }
    #endregion

    #region COMPONENT_SYSTEM
    public void AddComponent(Component component)
    {
        this.components.Add(component);
        component.OnLoad(this);
    }

    public void RemoveComponent(Component component)
    {
        component.OnUnload();
        this.components.Remove(component);
    }

    protected void UpdateComponents(GameTime gameTime)
    {
        foreach (Component c in this.components)
        {
            if (c.Enabled)
            {
                c.Update(gameTime);
            }
        }
    }

    protected void UnloadComponents()
    {
        foreach (Component c in this.components)
        {
            if (c.Enabled)
            {
                c.OnUnload();
            }
        }
    }

    /// <summary>
    /// This will return the FIRST component of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public Component? GetComponent<T>() where T : Component
    {
        foreach (Component c in this.components)
        {
            if (c.GetType() == typeof(T))
            {
                return c;
            }
        }

        return null;
    }

    /// <summary>
    /// This will return ALL components of type T
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public List<Component> GetAllComponents<T>() where T : Component
    {
        List<Component> typeComponents = new();
        foreach (Component c in this.components)
        {
            if (c.GetType() == typeof(T))
            {
                typeComponents.Add(c);
            }
        }

        return typeComponents;
    }

    public Component GetComponent<T>(string componentName) where T : Component
    {
        List<Component> typeComponents = this.GetAllComponents<T>();
        foreach (Component c in typeComponents)
        {
            if (c.GetName() == componentName)
            {
                return c;
            }
        }

        throw new ArgumentException("No component with that name on this GameObject");
    }
    #endregion
}
