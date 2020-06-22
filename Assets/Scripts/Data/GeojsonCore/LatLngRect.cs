using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class LatLngRect
    {
        public double maxLatitude;
        public double minLatitude;
        public double maxLongitude;
        public double minLongitude;

        public LatLng getCenter()
        {
            var lat = (maxLatitude + minLatitude) * 0.5;
            var lng = (maxLongitude + minLongitude) * 0.5;

            return new LatLng()
            {
                latitude = lat,
                longitude = lng,
            };
        }

        public LatLng getMin()
        {
            return new LatLng()
            {
                latitude = minLatitude,
                longitude = minLongitude,
            };
        }

        public LatLng getMax()
        {
            return new LatLng()
            {
                latitude = maxLatitude,
                longitude = maxLongitude,
            };
        }
    }
}