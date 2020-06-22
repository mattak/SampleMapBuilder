using System.Collections.Generic;
using System.Linq;
using SampleMapBuilder.Data.GeojsonCore;
using SampleMapBuilder.Domain.UseCase;
using SampleMapBuilder.Infra;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SampleMapBuilder.UI
{
    public class GeojsonTileRenderer : MonoBehaviour
    {
        [SerializeField] private Button RefreshButton = default;
        [SerializeField] private Material Material = default;
        [SerializeField] private int x = 58412;
        [SerializeField] private int y = 25215;

        private MapUseCase mapUseCase = new MapUseCase();

        void Start()
        {
            this.RefreshButton.OnClickAsObservable().Subscribe(this.Fetch).AddTo(this);
        }

        void Fetch(Unit _)
        {
            ClearLines();

            mapUseCase.FetchRoad(x, y)
                .Subscribe(this.Render, Debug.LogError)
                .AddTo(this);
        }

        void Render(Geojson json)
        {
            var latLngRect = mapUseCase.FindCoveringLatLngRect(json);
            var latLngCenter = latLngRect.getCenter();

            var lines = json.features
                .Where(it => it.geometry.IsLineString())
                .Select(it => it.geometry.AsLineString());
    
            foreach (var line in lines)
            {
                var from = line.coordinates[0];
                var to = line.coordinates[1];
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