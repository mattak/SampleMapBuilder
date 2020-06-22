using System;

namespace SampleMapBuilder.Data.GeojsonCore
{
    [Serializable]
    public class Geometry
    {
        public string type;

        public bool IsPoint()
        {
            return string.Equals(this.type, "Point");
        }

        public bool IsLineString()
        {
            return string.Equals(this.type, "LineString");
        }

        public bool IsMultiLineString()
        {
            return string.Equals(this.type, "MultiLineString");
        }

        public bool IsPolygon()
        {
            return string.Equals(this.type, "Polygon");
        }

        public bool IsMultiPolygon()
        {
            return string.Equals(this.type, "MultiPolygon");
        }

        public bool IsGeometryCollection()
        {
            return string.Equals(this.type, "GeometryCollection");
        }

        public GeometryPoint AsPoint()
        {
            return (GeometryPoint) this;
        }
        
        public GeometryLineString AsLineString()
        {
            return (GeometryLineString) this;
        }
        
        public GeometryPolygon AsPolygon()
        {
            return (GeometryPolygon) this;
        }
    }
}