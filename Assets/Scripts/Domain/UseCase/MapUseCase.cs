using System;
using System.Linq;
using SampleMapBuilder.Data;
using SampleMapBuilder.Infra;
using UniRx;

namespace SampleMapBuilder.Domain.UseCase
{
    public class MapUseCase
    {
        private TileClient client = new TileClient();
            
        public IObservable<RoadGeojson> FetchRoad(int x, int y)
        {
            return client.Fetch(16, x, y)
                .Select(it => RoadGeojsonParser.Parse(it))
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread();
        }
        
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