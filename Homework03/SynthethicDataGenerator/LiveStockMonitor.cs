// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: LiveStockMonitor.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Livestock {

  /// <summary>Holder for reflection information generated from LiveStockMonitor.proto</summary>
  public static partial class LiveStockMonitorReflection {

    #region Descriptor
    /// <summary>File descriptor for LiveStockMonitor.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static LiveStockMonitorReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChZMaXZlU3RvY2tNb25pdG9yLnByb3RvEglsaXZlc3RvY2sinQIKBkFuaW1h",
            "bBIKCgJpZBgBIAEoCRIUCgxQYXJ0aXRpb25LZXkYAiABKAkSEQoJYW5pbWFs",
            "X2lkGAMgASgJEhgKEGJvZHlfdGVtcGVyYXR1cmUYBCABKAESFgoOYWN0aXZp",
            "dHlfbGV2ZWwYBSABKAESEgoKaGVhcnRfcmF0ZRgGIAEoARIYChByZXNwaXJh",
            "dGlvbl9yYXRlGAcgASgBEiUKCGxvY2F0aW9uGAggASgLMhMubGl2ZXN0b2Nr",
            "LkxvY2F0aW9uEhMKC2ZlZWRfaW50YWtlGAkgASgBEhkKEXdhdGVyX2NvbnN1",
            "bXB0aW9uGAogASgBEhcKD21pbGtfcHJvZHVjdGlvbhgLIAEoARIOCgZ3ZWln",
            "aHQYDCABKAEiLwoITG9jYXRpb24SEAoIbGF0aXR1ZGUYASABKAESEQoJbG9u",
            "Z2l0dWRlGAIgASgBIi0KB0FuaW1hbHMSIgoHZW50cmllcxgBIAMoCzIRLmxp",
            "dmVzdG9jay5BbmltYWxiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Livestock.Animal), global::Livestock.Animal.Parser, new[]{ "Id", "PartitionKey", "AnimalId", "BodyTemperature", "ActivityLevel", "HeartRate", "RespirationRate", "Location", "FeedIntake", "WaterConsumption", "MilkProduction", "Weight" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Livestock.Location), global::Livestock.Location.Parser, new[]{ "Latitude", "Longitude" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Livestock.Animals), global::Livestock.Animals.Parser, new[]{ "Entries" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Animal : pb::IMessage<Animal> {
    private static readonly pb::MessageParser<Animal> _parser = new pb::MessageParser<Animal>(() => new Animal());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Animal> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Livestock.LiveStockMonitorReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Animal() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Animal(Animal other) : this() {
      id_ = other.id_;
      partitionKey_ = other.partitionKey_;
      animalId_ = other.animalId_;
      bodyTemperature_ = other.bodyTemperature_;
      activityLevel_ = other.activityLevel_;
      heartRate_ = other.heartRate_;
      respirationRate_ = other.respirationRate_;
      location_ = other.location_ != null ? other.location_.Clone() : null;
      feedIntake_ = other.feedIntake_;
      waterConsumption_ = other.waterConsumption_;
      milkProduction_ = other.milkProduction_;
      weight_ = other.weight_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Animal Clone() {
      return new Animal(this);
    }

    /// <summary>Field number for the "id" field.</summary>
    public const int IdFieldNumber = 1;
    private string id_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Id {
      get { return id_; }
      set {
        id_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "PartitionKey" field.</summary>
    public const int PartitionKeyFieldNumber = 2;
    private string partitionKey_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string PartitionKey {
      get { return partitionKey_; }
      set {
        partitionKey_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "animal_id" field.</summary>
    public const int AnimalIdFieldNumber = 3;
    private string animalId_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string AnimalId {
      get { return animalId_; }
      set {
        animalId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "body_temperature" field.</summary>
    public const int BodyTemperatureFieldNumber = 4;
    private double bodyTemperature_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double BodyTemperature {
      get { return bodyTemperature_; }
      set {
        bodyTemperature_ = value;
      }
    }

    /// <summary>Field number for the "activity_level" field.</summary>
    public const int ActivityLevelFieldNumber = 5;
    private double activityLevel_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double ActivityLevel {
      get { return activityLevel_; }
      set {
        activityLevel_ = value;
      }
    }

    /// <summary>Field number for the "heart_rate" field.</summary>
    public const int HeartRateFieldNumber = 6;
    private double heartRate_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double HeartRate {
      get { return heartRate_; }
      set {
        heartRate_ = value;
      }
    }

    /// <summary>Field number for the "respiration_rate" field.</summary>
    public const int RespirationRateFieldNumber = 7;
    private double respirationRate_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double RespirationRate {
      get { return respirationRate_; }
      set {
        respirationRate_ = value;
      }
    }

    /// <summary>Field number for the "location" field.</summary>
    public const int LocationFieldNumber = 8;
    private global::Livestock.Location location_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Livestock.Location Location {
      get { return location_; }
      set {
        location_ = value;
      }
    }

    /// <summary>Field number for the "feed_intake" field.</summary>
    public const int FeedIntakeFieldNumber = 9;
    private double feedIntake_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double FeedIntake {
      get { return feedIntake_; }
      set {
        feedIntake_ = value;
      }
    }

    /// <summary>Field number for the "water_consumption" field.</summary>
    public const int WaterConsumptionFieldNumber = 10;
    private double waterConsumption_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double WaterConsumption {
      get { return waterConsumption_; }
      set {
        waterConsumption_ = value;
      }
    }

    /// <summary>Field number for the "milk_production" field.</summary>
    public const int MilkProductionFieldNumber = 11;
    private double milkProduction_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double MilkProduction {
      get { return milkProduction_; }
      set {
        milkProduction_ = value;
      }
    }

    /// <summary>Field number for the "weight" field.</summary>
    public const int WeightFieldNumber = 12;
    private double weight_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Weight {
      get { return weight_; }
      set {
        weight_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Animal);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Animal other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Id != other.Id) return false;
      if (PartitionKey != other.PartitionKey) return false;
      if (AnimalId != other.AnimalId) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(BodyTemperature, other.BodyTemperature)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(ActivityLevel, other.ActivityLevel)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(HeartRate, other.HeartRate)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(RespirationRate, other.RespirationRate)) return false;
      if (!object.Equals(Location, other.Location)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(FeedIntake, other.FeedIntake)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(WaterConsumption, other.WaterConsumption)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(MilkProduction, other.MilkProduction)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Weight, other.Weight)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Id.Length != 0) hash ^= Id.GetHashCode();
      if (PartitionKey.Length != 0) hash ^= PartitionKey.GetHashCode();
      if (AnimalId.Length != 0) hash ^= AnimalId.GetHashCode();
      if (BodyTemperature != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(BodyTemperature);
      if (ActivityLevel != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(ActivityLevel);
      if (HeartRate != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(HeartRate);
      if (RespirationRate != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(RespirationRate);
      if (location_ != null) hash ^= Location.GetHashCode();
      if (FeedIntake != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(FeedIntake);
      if (WaterConsumption != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(WaterConsumption);
      if (MilkProduction != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(MilkProduction);
      if (Weight != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Weight);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Id.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Id);
      }
      if (PartitionKey.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(PartitionKey);
      }
      if (AnimalId.Length != 0) {
        output.WriteRawTag(26);
        output.WriteString(AnimalId);
      }
      if (BodyTemperature != 0D) {
        output.WriteRawTag(33);
        output.WriteDouble(BodyTemperature);
      }
      if (ActivityLevel != 0D) {
        output.WriteRawTag(41);
        output.WriteDouble(ActivityLevel);
      }
      if (HeartRate != 0D) {
        output.WriteRawTag(49);
        output.WriteDouble(HeartRate);
      }
      if (RespirationRate != 0D) {
        output.WriteRawTag(57);
        output.WriteDouble(RespirationRate);
      }
      if (location_ != null) {
        output.WriteRawTag(66);
        output.WriteMessage(Location);
      }
      if (FeedIntake != 0D) {
        output.WriteRawTag(73);
        output.WriteDouble(FeedIntake);
      }
      if (WaterConsumption != 0D) {
        output.WriteRawTag(81);
        output.WriteDouble(WaterConsumption);
      }
      if (MilkProduction != 0D) {
        output.WriteRawTag(89);
        output.WriteDouble(MilkProduction);
      }
      if (Weight != 0D) {
        output.WriteRawTag(97);
        output.WriteDouble(Weight);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Id.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Id);
      }
      if (PartitionKey.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(PartitionKey);
      }
      if (AnimalId.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(AnimalId);
      }
      if (BodyTemperature != 0D) {
        size += 1 + 8;
      }
      if (ActivityLevel != 0D) {
        size += 1 + 8;
      }
      if (HeartRate != 0D) {
        size += 1 + 8;
      }
      if (RespirationRate != 0D) {
        size += 1 + 8;
      }
      if (location_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Location);
      }
      if (FeedIntake != 0D) {
        size += 1 + 8;
      }
      if (WaterConsumption != 0D) {
        size += 1 + 8;
      }
      if (MilkProduction != 0D) {
        size += 1 + 8;
      }
      if (Weight != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Animal other) {
      if (other == null) {
        return;
      }
      if (other.Id.Length != 0) {
        Id = other.Id;
      }
      if (other.PartitionKey.Length != 0) {
        PartitionKey = other.PartitionKey;
      }
      if (other.AnimalId.Length != 0) {
        AnimalId = other.AnimalId;
      }
      if (other.BodyTemperature != 0D) {
        BodyTemperature = other.BodyTemperature;
      }
      if (other.ActivityLevel != 0D) {
        ActivityLevel = other.ActivityLevel;
      }
      if (other.HeartRate != 0D) {
        HeartRate = other.HeartRate;
      }
      if (other.RespirationRate != 0D) {
        RespirationRate = other.RespirationRate;
      }
      if (other.location_ != null) {
        if (location_ == null) {
          Location = new global::Livestock.Location();
        }
        Location.MergeFrom(other.Location);
      }
      if (other.FeedIntake != 0D) {
        FeedIntake = other.FeedIntake;
      }
      if (other.WaterConsumption != 0D) {
        WaterConsumption = other.WaterConsumption;
      }
      if (other.MilkProduction != 0D) {
        MilkProduction = other.MilkProduction;
      }
      if (other.Weight != 0D) {
        Weight = other.Weight;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Id = input.ReadString();
            break;
          }
          case 18: {
            PartitionKey = input.ReadString();
            break;
          }
          case 26: {
            AnimalId = input.ReadString();
            break;
          }
          case 33: {
            BodyTemperature = input.ReadDouble();
            break;
          }
          case 41: {
            ActivityLevel = input.ReadDouble();
            break;
          }
          case 49: {
            HeartRate = input.ReadDouble();
            break;
          }
          case 57: {
            RespirationRate = input.ReadDouble();
            break;
          }
          case 66: {
            if (location_ == null) {
              Location = new global::Livestock.Location();
            }
            input.ReadMessage(Location);
            break;
          }
          case 73: {
            FeedIntake = input.ReadDouble();
            break;
          }
          case 81: {
            WaterConsumption = input.ReadDouble();
            break;
          }
          case 89: {
            MilkProduction = input.ReadDouble();
            break;
          }
          case 97: {
            Weight = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Location : pb::IMessage<Location> {
    private static readonly pb::MessageParser<Location> _parser = new pb::MessageParser<Location>(() => new Location());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Location> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Livestock.LiveStockMonitorReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Location() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Location(Location other) : this() {
      latitude_ = other.latitude_;
      longitude_ = other.longitude_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Location Clone() {
      return new Location(this);
    }

    /// <summary>Field number for the "latitude" field.</summary>
    public const int LatitudeFieldNumber = 1;
    private double latitude_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Latitude {
      get { return latitude_; }
      set {
        latitude_ = value;
      }
    }

    /// <summary>Field number for the "longitude" field.</summary>
    public const int LongitudeFieldNumber = 2;
    private double longitude_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public double Longitude {
      get { return longitude_; }
      set {
        longitude_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Location);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Location other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Latitude, other.Latitude)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(Longitude, other.Longitude)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Latitude != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Latitude);
      if (Longitude != 0D) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(Longitude);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Latitude != 0D) {
        output.WriteRawTag(9);
        output.WriteDouble(Latitude);
      }
      if (Longitude != 0D) {
        output.WriteRawTag(17);
        output.WriteDouble(Longitude);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Latitude != 0D) {
        size += 1 + 8;
      }
      if (Longitude != 0D) {
        size += 1 + 8;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Location other) {
      if (other == null) {
        return;
      }
      if (other.Latitude != 0D) {
        Latitude = other.Latitude;
      }
      if (other.Longitude != 0D) {
        Longitude = other.Longitude;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 9: {
            Latitude = input.ReadDouble();
            break;
          }
          case 17: {
            Longitude = input.ReadDouble();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Animals : pb::IMessage<Animals> {
    private static readonly pb::MessageParser<Animals> _parser = new pb::MessageParser<Animals>(() => new Animals());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Animals> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Livestock.LiveStockMonitorReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Animals() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Animals(Animals other) : this() {
      entries_ = other.entries_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Animals Clone() {
      return new Animals(this);
    }

    /// <summary>Field number for the "entries" field.</summary>
    public const int EntriesFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Livestock.Animal> _repeated_entries_codec
        = pb::FieldCodec.ForMessage(10, global::Livestock.Animal.Parser);
    private readonly pbc::RepeatedField<global::Livestock.Animal> entries_ = new pbc::RepeatedField<global::Livestock.Animal>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Livestock.Animal> Entries {
      get { return entries_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Animals);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Animals other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!entries_.Equals(other.entries_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= entries_.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      entries_.WriteTo(output, _repeated_entries_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += entries_.CalculateSize(_repeated_entries_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Animals other) {
      if (other == null) {
        return;
      }
      entries_.Add(other.entries_);
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            entries_.AddEntriesFrom(input, _repeated_entries_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
