using System;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

namespace SampleMapBuilder.Infra
{
    public class TileClient
    {
        // vector tiles experiment
        // - https://github.com/gsi-cyberjapan/vector-tile-experiment
        // - https://cyberjapandata.gsi.go.jp/xyz/experimental_rdcl/{z}/{x}/{y}.geojson
        // - https://maps.gsi.go.jp/xyz/experimental_rdcl/16/58412/25215.geojson
        // mapbox gl
        // - https://github.com/gsi-cyberjapan/gsivectortile-mapbox-gl-js
        // - https://cyberjapandata.gsi.go.jp/xyz/experimental_bvmap/{z}/{x}/{y}.pbf
        // tiles http://maps.gsi.go.jp/development/ichiran.html
        public IObservable<string> Fetch(int zoom, int x, int y)
        {
            var style = "experimental_rdcl";
            var extension = "geojson";
            var url = $"https://cyberjapandata.gsi.go.jp/xyz/{style}/{zoom}/{x}/{y}.{extension}";
            var request = UnityWebRequest.Get(url);
            return request.ToRequestTextObservable();
        }
    }
}