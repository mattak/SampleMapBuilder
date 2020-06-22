using Mapbox.VectorTile;
using UniRx;
using UnityEngine;

namespace SampleMapBuilder.Data.Parser
{
    public static class MapboxTileParser
    {
        public static VectorTile Parse(byte[] data)
        {
            VectorTile vt = new VectorTile(data);
            return vt;
            //
            // //get available layer names
            // foreach (var layerName in vt.LayerNames())
            // {
            //     // get layer by name
            //     VectorTileLayer layer = vt.GetLayer(layerName);
            //
            //     // iterate through all features
            //     for (int i = 0; i < layer.FeatureCount(); i++)
            //     {
            //         Debug.LogFormat("layer:{0} feat:{1}", layer.Name, i);
            //
            //         //get the feature
            //         VectorTileFeature feat = layer.GetFeature(i);
            //
            //         //get feature properties
            //         var properties = feat.GetProperties();
            //         foreach (var prop in properties)
            //         {
            //             Debug.LogFormat("key:{0} value:{1}", prop.Key, prop.Value);
            //         }
            //
            //         //or get property value if you already know the key
            //         //object value = feat.GetValue(prop.Key);
            //         //iterate through all geometry parts
            //         //requesting coordinates as ints
            //         foreach (var part in feat.Geometry<int>())
            //         {
            //             //iterate through coordinates of the part
            //             foreach (var geom in part)
            //             {
            //                 Debug.LogFormat("geom.X:{0} geom.Y:{1}", geom.X, geom.Y);
            //             }
            //         }
            //     }
            // }
        }
    }
}