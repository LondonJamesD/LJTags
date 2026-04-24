using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace LJ.Tags
{
    [System.Serializable]
    public class LJTagPair
    {
        public LJTag from;
        public LJTag to;

        public LJTagPair(LJTag from, LJTag to)
        {
            this.from = from;
            this.to = to;
        }
    }

    public class LJTagContext : MonoBehaviour
    {
        [SerializeField] List<LJTagPair> tagPairs;
        Dictionary<LJTag, LJTag> pairDictionary;
        private void Start()
        {
            pairDictionary = new Dictionary<LJTag, LJTag>();
            foreach (LJTagPair pair in tagPairs)
            {
                pairDictionary.Add(pair.from, pair.to);
            }
        }
        public LJTag ModifiedTag(LJTag input)
        {
            if (pairDictionary.TryGetValue(input, out LJTag output))
                return output;
            Debug.LogWarning("Tag Context returned null, input: " + input);
            return input;
        }
        public bool ModifiedContainsTag(LJTag input, LJTagInstance instance)
        {
            for (int i = 0; i < tagPairs.Count; i++)
            {
                if (tagPairs[i].to == input && instance.TagValid(tagPairs[i].from))
                    return true;
            }
            return false;
        }
        public void AddContext(LJTagPair pair)
        {
            LJTagPair sameFrom = tagPairs.Where(t => t.from == pair.from).FirstOrDefault();
            if (sameFrom != null)
            {
                sameFrom.to = pair.to;
                return;
            }
            tagPairs.Add(pair);
            pairDictionary.Add(pair.from, pair.to);
        }
    }
}