using SimpleJSON;

namespace SampleMapBuilder.Data
{
    public static class RoadGeojsonParser
    {
        public static RoadGeojson Parse(string text)
        {
            var node = JSON.Parse(text);
            var result = new RoadGeojson();
            result.type = node["type"].Value;
            result.features = ParseFeatures(node["features"].AsArray);
            return result;
        }

        private static RoadFeature[] ParseFeatures(JSONArray array)
        {
            var features = new RoadFeature[array.Count];

            for (var i = 0; i < array.Count; i++)
            {
                features[i] = ParseFeature(array[i].AsObject);
            }

            return features;
        }

        private static RoadFeature ParseFeature(JSONObject obj)
        {
            var feature = new RoadFeature();
            feature.type = obj["type"].Value;
            feature.geometry = ParseGeometry(obj["geometry"].AsObject);
            feature.properties = ParseProperties(obj["properties"].AsObject);
            return feature;
        }

        private static Geometry ParseGeometry(JSONObject obj)
        {
            var geometry = new Geometry();
            geometry.type = obj["type"].Value;

            var coordinatesArray = obj["coordinates"].AsArray;
            geometry.coordinates = new LatLng[coordinatesArray.Count];

            for (var i = 0; i < coordinatesArray.Count; i++)
            {
                var coordinates = coordinatesArray[i].AsArray;
                geometry.coordinates[i] = new LatLng()
                {
                    longitude = coordinates[0].AsDouble,
                    latitude = coordinates[1].AsDouble,
                };
            }

            return geometry;
        }

        private static Properties ParseProperties(JSONObject obj)
        {
            return new Properties();
        }
    }
}