using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class GeometryPolygon : Geometry
    {
        public LatLng[][] coordinates = null;
    }
}