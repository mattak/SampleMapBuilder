using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class Feature
    {
        public string type;
        public Geometry geometry;
        public Properties properties;
    }
}