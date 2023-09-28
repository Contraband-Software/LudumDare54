namespace LD54.Scripts.Engine;

using System;
using System.Collections.Generic;

public abstract class GameObject : EngineObject
{
    protected GameObject? parent;
    protected List<GameObject> children = new();

    protected List<Component> components = new();

    protected GameObject(string name) : base(name) { }

    #region SCENE_GRAPH
    public void AddChild(GameObject gameObject)
    {
        gameObject.parent.RemoveChild(gameObject);
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
        List<Component> coms = new();
        foreach (Component c in this.components)
        {
            if (c.GetType() == typeof(T))
            {
                coms.Add(c);
            }
        }

        return coms;
    }

    public Component GetComponent<T>(string name) where T : Component
    {
        List<Component> components = this.GetAllComponents<T>();
        foreach (Component c in components)
        {
            if (c.GetName() == name)
            {
                return c;
            }
        }

        throw new ArgumentException("No component with that name on this GameObject");
    }
    #endregion
}
