using System;
using SampleMapBuilder.Data.Parser;
using SampleMapBuilder.Infra;
using UniRx;

namespace SampleMapBuilder.Domain.UseCase
{
    public class PBFMapUseCase
    {
        private VectorTileClient client = new VectorTileClient();
            
        public IObservable<Unit> Fetch(int x, int y)
        {
            return client.Fetch(16, x, y)
                .Select(it => MapboxTileParser.Parse(it))
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread();
        }
    }
}