using QRCoder;

namespace Project.SaaS.Certfy.Core.Helpers;

public static class QrCodeHelper
{
    public static byte[] Generate(string content)
    {
        using var generator = new QRCodeGenerator();
        using var qrData = generator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
        var qrCode = new PngByteQRCode(qrData);

        return qrCode.GetGraphic(20);
    }
}
