using System;
using UnityEngine.Networking;

namespace SampleMapBuilder.Infra
{
    public class VectorTileClient
    {
        // e.g. https://cyberjapandata.gsi.go.jp/xyz/experimental_bvmap/16/58211/25808.pbf
        public IObservable<byte[]> Fetch(int zoom, int x, int y)
        {
            var style = "experimental_bvmap";
            var extension = "pbf";
            var url = $"https://cyberjapandata.gsi.go.jp/xyz/{style}/{zoom}/{x}/{y}.{extension}";
            var request = UnityWebRequest.Get(url);
            UnityEngine.Debug.Log("VectorTileClient.Start: " + url);
            return request.ToRequestBinaryObservable();
        }
    }
}