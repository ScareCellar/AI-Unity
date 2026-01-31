using UnityEngine;

public abstract class Perception : MonoBehaviour
{
    [SerializeField] string info;

    [SerializeField] protected string tagName;
    [SerializeField] protected LayerMask layerMask = Physics.AllLayers;

    [SerializeField, Range(0, 10)] protected float maxDistance = 5;
    [SerializeField, Range(0,180)] public float fov;

    [SerializeField] public bool debug = false;
    [SerializeField] public Color debugColor = Color.white;
    public abstract GameObject[] GetGameObjects();

    public virtual GameObject GetGameObjectInDirection(Vector3 direction) { return null; }
    public virtual bool GetOpenDirection(ref Vector3 openDirection) { return false; }
}
