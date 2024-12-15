using UnityEngine;

namespace Extensions
{
    public static class GOExtensions
    {
        public static T GetComponentInNeighborhood<T>(this GameObject gameObject)
        {
            if (gameObject.transform.parent == null)
                throw new UnityException("---- GetComponentInNeighborhood: The game object has no parent!");
        
            foreach (Transform neighbor in gameObject.transform.parent)
                if (neighbor.TryGetComponent(out T desiredComponent)) return desiredComponent;

            throw new UnityException("---- GetComponentInNeighborhood: Desired component has not found!");
        }
    }
}
