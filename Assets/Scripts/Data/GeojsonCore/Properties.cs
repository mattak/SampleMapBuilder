using System;
using System.Collections.Generic;
using UnityEngine;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class Properties
    {
        [SerializeField] private string[] keys;
        [SerializeField] private string[] values;

        private IDictionary<string, string> map = new Dictionary<string, string>();

        public Properties(string[] keys, string[] values)
        {
            if (keys == null || values == null)
            {
                this.keys = new string[0];
                this.values = new string[0];
            }

            if (keys.Length != values.Length)
            {
                throw new Exception("keys.Length != values.Length");
            }

            this.keys = keys;
            this.values = values;

            for (var i = 0; i < keys.Length; i++)
            {
                this.map[keys[i]] = values[i];
            }
        }

        public bool Has(string key)
        {
            return this.map.ContainsKey(key);
        }

        public string GetValueString(string key, string defaultValue)
        {
            return this.map.ContainsKey(key) ? this.map[key] : defaultValue;
        }

        public int GetValueInt32(string key, int defaultValue)
        {
            if (!this.map.ContainsKey(key)) return defaultValue;

            try
            {
                return Int32.Parse(this.map[key]);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public long GetValueInt64(string key, long defaultValue)
        {
            if (!this.map.ContainsKey(key)) return defaultValue;

            try
            {
                return Int64.Parse(this.map[key]);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public float GetValueFloat(string key, float defaultValue)
        {
            if (!this.map.ContainsKey(key)) return defaultValue;

            try
            {
                return Single.Parse(this.map[key]);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }

        public double GetValueDouble(string key, double defaultValue)
        {
            if (!this.map.ContainsKey(key)) return defaultValue;

            try
            {
                return Double.Parse(this.map[key]);
            }
            catch (FormatException)
            {
                return defaultValue;
            }
        }
    }
}