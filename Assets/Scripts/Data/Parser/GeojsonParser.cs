using System;
using SampleMapBuilder.Data.GeojsonCore;
using SimpleJSON;

namespace SampleMapBuilder.Data.Parser
{
    public static class GeojsonParser
    {
        public static Geojson Parse(string text)
        {
            var node = JSON.Parse(text);
            var result = new Geojson();
            result.type = node["type"].Value;
            result.features = ParseFeatures(node["features"].AsArray);
            return result;
        }

        public static Feature[] ParseFeatures(JSONArray array)
        {
            var features = new Feature[array.Count];

            for (var i = 0; i < array.Count; i++)
            {
                features[i] = ParseFeature(array[i].AsObject);
            }

            return features;
        }

        public static Feature ParseFeature(JSONObject obj)
        {
            var feature = new Feature();
            feature.type = obj["type"].Value;
            feature.geometry = ParseGeometry(obj["geometry"].AsObject);
            feature.properties = ParseProperties(obj["properties"].AsObject);
            return feature;
        }

        public static Geometry ParseGeometry(JSONObject obj)
        {
            var type = obj["type"].Value;
            var coordinatesArray = obj["coordinates"].AsArray;
            Geometry geometry;

            switch (type)
            {
                case "Point":
                    geometry = ParsePoint(type, coordinatesArray);
                    break;
                case "LineString":
                    geometry = ParseLineString(type, coordinatesArray);
                    break;
                case "Polygon":
                    geometry = ParsePolygon(type, coordinatesArray);
                    break;
                default:
                    throw new Exception("Cannot parse geometry type: " + type);
            }

            return geometry;
        }

        public static GeometryPoint ParsePoint(string type, JSONArray array)
        {
            return new GeometryPoint
            {
                type = type,
                coordinates = new LatLng
                {
                    longitude = array[0].AsDouble,
                    latitude = array[1].AsDouble,
                }
            };
        }

        public static GeometryLineString ParseLineString(string type, JSONArray array)
        {
            var latlngArray = new LatLng[array.Count];

            for (var i = 0; i < array.Count; i++)
            {
                var point = array[i].AsArray;
                latlngArray[i] = new LatLng
                {
                    longitude = point[0].AsDouble,
                    latitude = point[1].AsDouble,
                };
            }

            return new GeometryLineString
            {
                type = type,
                coordinates = latlngArray,
            };
        }

        public static GeometryPolygon ParsePolygon(string type, JSONArray array)
        {
            var latlngArray = new LatLng[array.Count][];

            for (var i = 0; i < array.Count; i++)
            {
                var points = array[i].AsArray;
                latlngArray[i] = new LatLng[points.Count];

                for (var j = 0; j < points.Count; j++)
                {
                    latlngArray[i][j] = new LatLng
                    {
                        longitude = points[0].AsDouble,
                        latitude = points[1].AsDouble,
                    };
                }
            }

            return new GeometryPolygon
            {
                type = type,
                coordinates = latlngArray,
            };
        }

        public static Properties ParseProperties(JSONObject obj)
        {
            var length = obj.Count;
            var keys = new string[length];
            var values = new string[length];

            var i = 0;
            foreach (var key in obj.Keys)
            {
                keys[i] = key;
                values[i] = obj[key].Value;
                i++;
            }

            return new Properties
            (
                keys: keys,
                values: values
            );
        }
    }
}