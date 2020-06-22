using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class GeometryPoint : Geometry
    {
        public LatLng coordinates = null;
    }
}