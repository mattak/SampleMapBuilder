using System;
using System.Collections.Generic;
using System.Linq;
using Mapbox.VectorTile;
using Mapbox.VectorTile.ExtensionMethods;
using Mapbox.VectorTile.Geometry;
using SampleMapBuilder.Data.GeojsonCore;
using SampleMapBuilder.Data.Parser;
using SampleMapBuilder.Infra;
using UniRx;
using LatLng = Mapbox.VectorTile.Geometry.LatLng;

namespace SampleMapBuilder.Domain.UseCase
{
    public class PBFMapUseCase
    {
        private VectorTileClient client = new VectorTileClient();

        // zoom: 16
        public IObservable<VectorTile> Fetch(int zoom, int x, int y)
        {
            return client.Fetch(zoom, x, y)
                .Select(it => MapboxTileParser.Parse(it));
        }

        public string[] SelectLayerNames(VectorTile tile)
        {
            // names: railway, road, building, label, symbol, contour, elevation, landforml, transl
            return tile.LayerNames().ToArray();
        }

        public VectorTileFeature[] SelectLayerFeatures(VectorTile tile, string layerName)
        {
            var layer = tile.GetLayer(layerName);

            var features = new VectorTileFeature[layer.FeatureCount()];

            for (var i = 0; i < features.Length; i++)
            {
                features[i] = layer.GetFeature(i);
            }

            return features;
        }

        public VectorTileFeature[] SelectRoadFeatures(VectorTile tile)
        {
            return SelectLayerFeatures(tile, "road");
        }

        public List<List<List<LatLng>>> SelectLines(IEnumerable<VectorTileFeature> features, ulong zoom, ulong x, ulong y)
        {
            return features
                .Select(it => it.GeometryAsWgs84(zoom, x, y))
                .ToList();
        }

        public LatLngRect FindCoveringLatLngRect(IEnumerable<VectorTileFeature> features, ulong zoom, ulong x, ulong y)
        {
            var latLngs = ListUpLatLng(features, zoom, x, y);
            var latitudes = latLngs.Select(it => it.Lat);
            var longitudes = latLngs.Select(it => it.Lng);
            
            return new LatLngRect
            {
                minLatitude = latitudes.Min(),
                maxLatitude = latitudes.Max(),
                minLongitude = longitudes.Min(),
                maxLongitude = longitudes.Max(),
            };
        }

        private List<LatLng> ListUpLatLng(IEnumerable<VectorTileFeature> features, ulong zoom, ulong x, ulong y)
        {
            var latlngArrayArray = features.Select(it => it.GeometryAsWgs84(zoom, x, y))
                .ToList();

            var result = new List<LatLng>();

            foreach (var arrayArray in latlngArrayArray)
            {
                foreach (var array in arrayArray)
                {
                    result.AddRange(array);
                }
            }

            return result;
        }
    }
}