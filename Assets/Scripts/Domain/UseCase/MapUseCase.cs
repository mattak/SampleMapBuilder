using System.Linq;
using SampleMapBuilder.Data;

namespace SampleMapBuilder.Domain.UseCase
{
    public class MapUseCase
    {
        public LatLngRect FindCoveringLatLngRect(RoadGeojson json)
        {
            var latitudes = json.features
                .SelectMany(feature => feature.geometry.coordinates.Select(it => it.latitude));
            var longitudes = json.features
                .SelectMany(feature => feature.geometry.coordinates.Select(it => it.longitude));

            return new LatLngRect
            {
                minLatitude = latitudes.Min(),
                maxLatitude = latitudes.Max(),
                minLongitude = longitudes.Min(),
                maxLongitude = longitudes.Max(),
            };
        }
    }
}