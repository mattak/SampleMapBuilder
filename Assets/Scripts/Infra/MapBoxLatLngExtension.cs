using SampleMapBuilder.Data.GeojsonCore;

namespace SampleMapBuilder.Infra
{
    public static class MapBoxLatLngExtension
    {
        public static LatLng ToGeojsonLatLng(this Mapbox.VectorTile.Geometry.LatLng latLng)
        {
            return new LatLng
            {
                latitude = latLng.Lat,
                longitude = latLng.Lng,
            };
        }
    }
}