using System;
using System.Collections.Generic;

namespace SampleMapBuilder.Data
{
    [Serializable]
    public class RoadGeojson
    {
        public string type;
        public RoadFeature[] features;
    }

    [Serializable]
    public class RoadFeature
    {
        public string type;
        public Geometry geometry;
        public Properties properties;
    }

    [Serializable]
    public class Geometry
    {
        public string type;
        public LatLng[] coordinates = new LatLng[0];
    }

    [Serializable]
    public class Properties
    {
        public int Width;
    }

    [Serializable]
    public class LatLng
    {
        public double latitude;
        public double longitude;
    }
}