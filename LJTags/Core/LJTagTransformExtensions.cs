using System.ComponentModel.Design.Serialization;
using System.Linq;
using UnityEngine;

namespace LJ.Tags
{
    public static class LJTagTransformExtensions
    {
        public static LJTagInstance LJTagInstance(this Transform transform)
        {
            return transform.GetComponent<LJTagInstance>();
        }
        public static void AddTag(this Transform transform, LJTag tag)
        {
            LJTagInstance instance = transform.LJTagInstance();
            if (instance == null)
                instance = transform.gameObject.AddComponent<LJTagInstance>();
            instance.AddTag(tag);
        }
        public static void RemoveTag(this Transform transform, LJTag tag)
        {
            LJTagInstance instance = transform.LJTagInstance();
            if (instance != null)
            {
                instance.RemoveTag(tag);
            }
        }
        public static void ChangeTagContext(this Transform transform, LJTag from, LJTag to)
        {
            LJTagContext context = transform.LJTagContext();
            if (context == null)
                context = transform.gameObject.AddComponent<LJTagContext>();
            LJTagPair pair = new LJTagPair(from, to);
            context.AddContext(pair);
        }
        public static bool HasTag(this Transform transform, LJTag tag, Transform contextTransform = null)
        {
            LJTagInstance instance = transform.LJTagInstance();
            if (instance == null)
            {
                Debug.LogWarning("No Tag Instance on object");
                return false;
            }
            if (contextTransform != null)
            {
                LJTagContext context = contextTransform.LJTagContext();
                if (context != null)
                {
                    if (context.ModifiedContainsTag(tag,instance)) return true;
                }
            }
            return instance.TagValid(tag);
        }
        public static LJTagContext LJTagContext(this Transform transform)
        {
            return transform.GetComponent<LJTagContext>();
        }
        public static LJTag WithContext(this LJTag tag, Transform contextTransform)
        {
            LJTagContext context = contextTransform.LJTagContext();
            if (context == null)
            {
                Debug.LogWarning("LJ Tag Context not found on transform " + contextTransform);
                return tag;
            }
            return context.ModifiedTag(tag);
        }
        public static Transform FindWithLJTagInHierarchy(this Transform transform, LJTag tag)
        {
            Transform root = transform.root;
            LJTagInstance[] instances = root.GetComponentsInChildren<LJTagInstance>();
            return instances.Where(inst => inst.TagValid(tag)).First().transform;
        }
        public static Transform[] LJTagTransformsInSphere(this Transform transform, LJTag target, float radius)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            return colliders.Select(item => transform)
                .Where(x => x != null && x.HasTag(target, transform))
                .ToArray();
        }
    }
}
