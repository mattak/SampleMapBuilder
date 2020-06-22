using System;
using System.Linq;
using SampleMapBuilder.Data.GeojsonCore;
using SampleMapBuilder.Data.Parser;
using SampleMapBuilder.Infra;
using UniRx;

namespace SampleMapBuilder.Domain.UseCase
{
    public class MapUseCase
    {
        private TileClient client = new TileClient();
            
        public IObservable<Geojson> FetchRoad(int x, int y)
        {
            return client.Fetch(16, x, y)
                .Select(it => GeojsonParser.Parse(it))
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread();
        }
        
        public LatLngRect FindCoveringLatLngRect(Geojson json)
        {
            var lines = json.features
                .Where(it => it.geometry.IsLineString())
                .Select(it => it.geometry.AsLineString());
            var latitudes = lines
                .SelectMany(line => line.coordinates.Select(it => it.latitude));
            var longitudes = lines
                .SelectMany(line => line.coordinates.Select(it => it.longitude));

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