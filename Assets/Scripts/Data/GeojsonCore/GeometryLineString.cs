using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class GeometryLineString : Geometry
    {
        public LatLng[] coordinates = null;
    }
}