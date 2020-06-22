using System.Linq;
using NUnit.Framework;
using SampleMapBuilder.Data.Parser;
using UnityEngine;

namespace Test.SampleMapBuilder
{
    public class GeojsonParseTest
    {
        [Test]
        public void ParseRoad()
        {
            var resource = Resources.Load<TextAsset>("geojson_test_001");
            Assert.NotNull(resource);
            Debug.Log("resource: " + resource.text);

            var json = GeojsonParser.Parse(resource.text);
            Assert.That(json.type, Is.EqualTo("FeatureCollection"));

            var points = json.features.Where(it => it.geometry.IsPoint()).Select(it => it.geometry.AsPoint());
            Assert.That(points.Count(), Is.EqualTo(12));

            var specificFeature = json.features
                .FirstOrDefault(it => it.properties.GetValueString("rID", "") == "50316-12843-i-2470");
            Assert.NotNull(specificFeature);
            Assert.That(specificFeature.properties.GetValueString("knj", ""), Is.EqualTo("日本橋駅"));
        }
    }
}