using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class LatLng
    {
        public double latitude;
        public double longitude;

        public Mapbox.VectorTile.Geometry.LatLng ToMapBoxLatLng()
        {
            return new Mapbox.VectorTile.Geometry.LatLng()
            {
                Lat = latitude,
                Lng = longitude,
            };
        }
    }
}