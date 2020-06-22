using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class Geojson
    {
        public string type;
        public Feature[] features;
    }
}