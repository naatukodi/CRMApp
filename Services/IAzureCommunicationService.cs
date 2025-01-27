namespace CRMApp.Services
{
    public interface IAzureCommunicationService
    {
        Task SendWhatsAppMessage(string phoneNumber, string messageText);
        Task SendInteractiveWhatsAppMessage(object messagePayload);
    }
}
