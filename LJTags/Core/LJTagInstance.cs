using System.Collections.Generic;
using UnityEngine;


namespace LJ.Tags
{
    public class LJTagInstance : MonoBehaviour
    {
        [SerializeField] List<LJTag> startTags;
        HashSet<LJTag> activeTags;

        private void Awake()
        {
            activeTags = new HashSet<LJTag>();
            foreach (LJTag tag in startTags)
                AddTag(tag);
        }
        public void AddTag(LJTag tag)
        {
            activeTags.Add(tag);
            LJTagUtilities.Register(tag, this);
        }
        public bool TagValid(LJTag tag, Transform contextTransform = null)
        {
            bool contextTagTrue = false;
            if (contextTransform != null)
            {
                LJTagContext context = contextTransform.LJTagContext();
                if (context != null)
                {
                    if (context.ModifiedContainsTag(tag, this))
                    {
                        contextTagTrue = true;
                    }
                }
            }
            return contextTagTrue || activeTags.Contains(tag);
        }
        public void RemoveTag(LJTag tag)
        {
            if (!TagValid(tag)) return;
            activeTags.Remove(tag); 
        }
    }
}