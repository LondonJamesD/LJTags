using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LJ.Tags
{
    public static class LJTagUtilities
    {
        public static Dictionary<LJTag, List<LJTagInstance>> registry = new();
        public static void Register(LJTag tag, LJTagInstance instance)
        {
            if (!registry.ContainsKey(tag))
            {
                registry[tag] = new List<LJTagInstance>();
            }
            if (!registry[tag].Contains(instance))
            {
                registry[tag].Add(instance);
            }
        }
        public static LJTagInstance Get(LJTag tag)
        {
            return registry[tag].FirstOrDefault();
        }
        public static Transform FindWithLJTag(LJTag tag)
        {
            LJTagInstance[] instances = Object.FindObjectsByType<LJTagInstance>(FindObjectsSortMode.InstanceID);
            if (instances.Length == 0)
            {
                Debug.Log("No LJTagInstance found in the scene.");
                return null;
            }
            return instances.Where(instance => instance.transform.HasTag(tag)).FirstOrDefault()?.transform;
        }
    }

}
