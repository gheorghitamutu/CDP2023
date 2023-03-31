
namespace IoTProcessing.Models;
public class DeviceSensorsData
{
    public string animal_id { get; set; }
    public double body_temperature { get; set; }
    public int activity_level { get; set; }
    public double heart_rate { get; set; }
    public double respiration_rate { get; set; }
    public Location location { get; set; }
    public double feed_intake { get; set; }
    public double water_consumption { get; set; }
    public double milk_production { get; set; }
    public double weight { get; set; }
}

public class Location
{
    public double latitude { get; set; }
    public double longitude { get; set; }
}
