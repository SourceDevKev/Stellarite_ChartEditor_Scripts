using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonParser : MonoBehaviour
{
    public static ChartObject ParseChart(string chartPath)
    {
        ChartObject chartData = JsonUtility.FromJson<ChartObject>(File.ReadAllText(chartPath));
        return chartData;
    }

    public static MetaObject ParseMeta(string metaPath)
    {
        MetaObject metadata = JsonUtility.FromJson<MetaObject>(File.ReadAllText(metaPath));
        return metadata;
    }
}
