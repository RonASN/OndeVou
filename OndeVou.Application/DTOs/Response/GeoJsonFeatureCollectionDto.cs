namespace OndeVou.Application.DTOs.Response;

public class GeoJsonFeatureCollectionDto
{
    public string Type { get; set; } = "FeatureCollection";
    public List<GeoJsonFeatureDto> Features { get; set; } = new();
}

public class GeoJsonFeatureDto
{
    public string Type { get; set; } = "Feature";
    public GeoJsonGeometryDto Geometry { get; set; } = new();
    public Dictionary<string, object> Properties { get; set; } = new();
}

public class GeoJsonGeometryDto
{
    public string Type { get; set; } = "Point";
    public double[] Coordinates { get; set; } = Array.Empty<double>();
}
