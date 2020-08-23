using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Deepwell;
using UnityEngine;

[Serializable]
public struct DWTagValue
{
    [SerializeField] public int value;

    public static implicit operator DWTagValue(int value)
    {
        return new DWTagValue { value = value };
    }

    public static implicit operator int(DWTagValue value)
    {
        return value.value;
    }

    public string ToString<T>() where T : DWTags
    {
        var tagNames = DWTags.TagNamesForClass(typeof(T));
        var value = this.value;
        var tagsThatAreSet = tagNames.Where((name, index) => (value & 1<<index) != 0);
        var output = tagsThatAreSet.Aggregate("", (aggr, val) => aggr.Length > 0 ? $"{aggr}|{val}":val);
        return output;
    }
}

namespace Deepwell
{
    public class DWTags : MonoBehaviour
    {
        public DWTagValue tags;

        public void AddTag(int tag)
        {
            tags |= tag;
        }

        public void RemoveTag(int tag)
        {
            tags &= ~tag;
        }

        public bool MatchesTagMask(int mask)
        {
            return (tags & mask) != 0;
        }

        public bool MatchesAllTagMask(int mask)
        {
            return (tags & mask) == mask;
        }

        public static List<String> TagNamesForClass(Type type)
        {
            var names = new List<String>();
            // var type = typeof(DWTags);
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var aField in fields)
            {
                if (aField.FieldType == typeof(int))
                {
                    var tagName = aField.Name;
                    var tagValue = aField.GetValue(null); // int
                    if (!tagName.StartsWith("Unused"))
                    {
                        names.Add(tagName);
                    }
                }
            }
            return names;
        }

        public List<String> TageNamesForInstance()
        {
            return DWTags.TagNamesForClass(this.GetType());
        }
        
        public void listTags()
        {
            var fields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (var aField in fields)
            {
                if (aField.FieldType == typeof(int))
                {
                    var tagName = aField.Name;
                    var tagValue = aField.GetValue(null); // int
                    if (!tagName.StartsWith("Unused"))
                    {
                        Debug.Log($"{tagName}: {tagValue}");
                    }
                }
            }
        }

        public static implicit operator String(DWTags value)
        {
            return "Hello world DWTags";
        }
    }

    public static class DWTags_GameObject_Extensions
    {
        private static T getOrCreateTagsOfType<T>(GameObject go) where T : DWTags
        {
            var tags = go.GetComponent(typeof(T)) as T;
            if (tags == null)
            {
                tags = go.AddComponent(typeof(T)) as T;
            }
            return tags;
        }

        public static void AddTag<T>(this GameObject go, int tag) where T : DWTags
        {
            getOrCreateTagsOfType<T>(go).AddTag(tag);
        }

        public static void RemoveTag<T>(this GameObject go, int tag) where T : DWTags
        {
            getOrCreateTagsOfType<T>(go).RemoveTag(tag);
        }

        public static bool MatchesTagMask<T>(this GameObject go, int mask) where T : DWTags
        {
            return getOrCreateTagsOfType<T>(go).MatchesTagMask(mask);
        }

        public static bool MatcheAllTagMask<T>(this GameObject go, int mask) where T : DWTags
        {
            return getOrCreateTagsOfType<T>(go).MatchesAllTagMask(mask);
        }
    }
}
