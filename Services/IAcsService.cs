namespace CRMApp.Services
{
    public interface IAcsService
    {
        Task SendWhatsAppMessage(string phoneNumber, string messageText);
        Task SendInteractiveWhatsAppMessage(object messagePayload);
    }
}
