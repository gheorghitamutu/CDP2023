
namespace IoTProcessing.Models;
public class DeviceIdAndSensorsData
{
                //    message.deviceId = deviceId;
                //     message.id = Guid.NewGuid().ToString();
                //     message.dateTime = enqueuedTime;
                //     message.timestamp = timestamp;

    // Device metadata + DeviceSensorsData as in cosmos db

    /// Device metadata
    public string deviceId { get; set; }
    public string id { get; set; }
    public string dateTime { get; set; }
    public string timestamp { get; set; }

    public string processed { get; set; }



    /// DeviceSensorsData
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
