using System.Collections.Generic;

namespace IoTProcessing.Models;

public class AnomaliesForDevice
{
    public string deviceId { get; set; }
    public List<Anomaly> anomalies { get; set; }
}