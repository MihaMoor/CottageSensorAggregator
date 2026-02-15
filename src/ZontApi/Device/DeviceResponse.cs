using CottageSensorAggregator.ZontApi.Device.Dto;

namespace CottageSensorAggregator.ZontApi.Device;

internal interface IResponseVisitor
{
    void Visit(SuccessDeviceResponse response);
    void Visit(ErrorDeviceResponse response);
}

internal abstract class DeviceResponse
{
    public bool Ok { get; set; }

    public abstract void Accept(IResponseVisitor visitor);
}

internal class SuccessDeviceResponse : DeviceResponse
{
    public List<DeviceDto> Devices { get; set; }

    public override void Accept(IResponseVisitor visitor)
    => visitor.Visit(this);
}

internal class ErrorDeviceResponse : DeviceResponse
{
    public string? Error { get; set; }
    public string? ErrorUi { get; set; }

    public override void Accept(IResponseVisitor visitor)
    => visitor.Visit(this);
}
