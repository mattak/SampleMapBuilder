using System;
using System.Collections.Generic;
using SampleMapBuilder.Data.GeojsonCore;
using SampleMapBuilder.Domain.UseCase;
using SampleMapBuilder.Infra;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace SampleMapBuilder.UI
{
    public class PBFTileRenderer : MonoBehaviour
    {
        [SerializeField] private Button RefreshButton = default;
        [SerializeField] private Material Material = default;
        [SerializeField] private int x = 58211; //58412;
        [SerializeField] private int y = 25808; //25215;

        private PBFMapUseCase mapUseCase = new PBFMapUseCase();

        void Start()
        {
            this.RefreshButton.OnClickAsObservable().Subscribe(this.Fetch).AddTo(this);
        }

        void Fetch(Unit _)
        {
            ClearLines();

            var zoom = 16LU;
            var _x = Convert.ToUInt64(x);
            var _y = Convert.ToUInt64(y);
            mapUseCase.Fetch((int) zoom, x, y)
                .SubscribeOn(Scheduler.ThreadPool)
                .ObserveOnMainThread()
                .Subscribe(tile =>
                {
                    var features = mapUseCase.SelectRoadFeatures(tile);
                    var latLngRect = mapUseCase.FindCoveringLatLngRect(features, zoom, _x, _y);
                    var linesArray = mapUseCase.SelectLines(features, zoom, _x, _y);
                    UnityEngine.Debug.Log("roads: " + features.Length);
                    this.Render(latLngRect.getCenter(), linesArray);
                }, Debug.LogError)
                .AddTo(this);
        }


        void Render(LatLng latLngCenter, List<List<List<Mapbox.VectorTile.Geometry.LatLng>>> linesArray)
        {
            foreach (var lines in linesArray)
            {
                foreach (var line in lines)
                {
                    var from = line[0];
                    var to = line[1];
                    var xyFrom = CoordinateConverter.LatLng2World(latLngCenter, from.ToGeojsonLatLng()) * 10;
                    var xyTo = CoordinateConverter.LatLng2World(latLngCenter, to.ToGeojsonLatLng()) * 10;
                    CreateLine(new[]
                    {
                        xyFrom, xyTo
                    });
                }
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