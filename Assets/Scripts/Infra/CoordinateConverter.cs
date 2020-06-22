using System;
using SampleMapBuilder.Data;
using SampleMapBuilder.Data.GeojsonCore;
using UnityEngine;

namespace SampleMapBuilder.Infra
{
    public static class CoordinateConverter
    {
        // XXX: 雑に真球として計算
        // @return km distance
        public static double Measure(LatLng from, LatLng to)
        {
            var R = 6378.137; // radius of earth in km
            var dLat = to.latitude * Math.PI / 180.0 - from.latitude * Math.PI / 180.0;
            var dLon = to.longitude * Math.PI / 180.0 - from.longitude * Math.PI / 180.0;

            var a = Math.Sin(dLat * 0.5) * Math.Sin(dLat * 0.5) +
                    Math.Cos(from.latitude * Math.PI / 180) * Math.Cos(to.latitude * Math.PI / 180)
                                                            * Math.Sin(dLon * 0.5) * Math.Sin(dLon * 0.5);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var d = R * c;
            return d;
        }

        // @return km distance
        public static Vector2 LatLng2World(LatLng center, LatLng target)
        {
            var R = 6378.137; // radius of earth in km
            var y = 2 * Math.PI * R * (target.latitude - center.latitude) / 360;
            var x = 2 * Math.PI * R * (target.longitude - center.longitude) / 360;
            return new Vector2((float) x, (float) y);
        }
    }
}