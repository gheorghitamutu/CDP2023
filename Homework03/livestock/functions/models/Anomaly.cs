using System.Collections.Generic;

namespace IoTProcessing.Models;

public class Anomaly
{
    public string name { get; set; }
    public string description { get; set; }
    public string deviceId { get; set; }
    public string timestamp { get; set; }

    public List<DeviceIdAndSensorsData> observationsWhereHappened { get; set; }
}