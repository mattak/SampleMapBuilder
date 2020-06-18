using System.Collections.Generic;
using SampleMapBuilder.Data;
using SampleMapBuilder.Domain.UseCase;
using SampleMapBuilder.Infra;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SampleMapBuilder.UI
{
    public class Fetcher : MonoBehaviour
    {
        [SerializeField] private Button RefreshButton = default;
        [SerializeField] private Material Material = default;
        [SerializeField] private int x = 58412;
        [SerializeField] private int y = 25215;

        private TileClient client = new TileClient();

        void Start()
        {
            this.client = new TileClient();
            this.RefreshButton.OnClickAsObservable().Subscribe(this.Fetch).AddTo(this);
        }

        void Fetch(Unit _)
        {
            ClearLines();
            client.Run(16, x, y)
                .Select(it => RoadGeojsonParser.Parse(it))
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread()
                .Subscribe(this.Render, Debug.LogError)
                .AddTo(this);
        }

        void Render(RoadGeojson json)
        {
            var mapUseCase = new MapUseCase();
            var latLngRect = mapUseCase.FindCoveringLatLngRect(json);
            var latLngCenter = latLngRect.getCenter();

            for (var i = 0; i < json.features.Length; i++)
            {
                var latlngArray = json.features[i].geometry.coordinates;
                var from = latlngArray[0];
                var to = latlngArray[1];
                var xyFrom = CoordinateConverter.LatLng2World(latLngCenter, from) * 10;
                var xyTo = CoordinateConverter.LatLng2World(latLngCenter, to) * 10;
                CreateLine(new[]
                {
                    xyFrom, xyTo
                });
            }
        }

        void ClearLines()
        {
            var children = new List<Transform>();
            for (var i = 0; i < this.transform.childCount; i++)
            {
                children.Add(this.transform.GetChild(i));
            }

            foreach (var child in children)
            {
                Destroy(child.gameObject);
            }
        }

        void CreateLine(Vector2[] points)
        {
            var _object = new GameObject("PolygonLine");
            var meshFilter = _object.AddComponent<MeshFilter>();
            var meshRenderer = _object.AddComponent<MeshRenderer>();

            meshFilter.sharedMesh = LinePolygon.CreateMesh(points, 0.01f);
            meshRenderer.material = this.Material;
            _object.transform.SetParent(this.transform);
        }
    }
}