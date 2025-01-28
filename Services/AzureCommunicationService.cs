using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using Azure.Communication.Messages;  // <-- Important: Use the Messages namespace now
using Microsoft.Extensions.Configuration;

namespace CRMApp.Services
{
    public class AzureCommunicationService : IAzureCommunicationService
    {
        private readonly NotificationMessagesClient _notificationMessagesClient;
        private readonly Guid _channelRegistrationId;

        public AzureCommunicationService(IConfiguration configuration)
        {
            // Retrieve the connection string from appsettings.json (e.g., "ACS:ConnectionString")
            var connectionString = configuration["ACS:ConnectionString"];
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(
                    nameof(connectionString), 
                    "ACS connection string is missing in appsettings.json.");
            }

            // Initialize the NotificationMessagesClient
            _notificationMessagesClient = new NotificationMessagesClient(connectionString);

            // Retrieve the channel registration ID from config (e.g., "ACS:ChannelRegistrationId")
            var channelRegistrationIdString = configuration["ACS:ChannelRegistrationId"];
            if (string.IsNullOrEmpty(channelRegistrationIdString))
            {
                throw new ArgumentNullException(
                    nameof(channelRegistrationIdString), 
                    "Channel registration ID is missing in appsettings.json.");
            }

            if (!Guid.TryParse(channelRegistrationIdString, out Guid parsedChannelId))
            {
                throw new ArgumentException(
                    "Invalid ChannelRegistrationId format. Please ensure it's a valid GUID.", 
                    nameof(channelRegistrationIdString));
            }

            _channelRegistrationId = parsedChannelId;
        }

        /// <summary>
        /// Sends a WhatsApp message using a predefined template.
        /// </summary>
        /// <param name="phoneNumber">Recipient's phone number (e.g., +1xxxxxxxxxx format)</param>
        /// <param name="messageText">Currently unused in the basic template approach but available if you want to add template variables.</param>
        public async Task SendWhatsAppMessage(string phoneNumber, string messageText)
        {
            // Define the recipient list
            var recipientList = new List<string> { phoneNumber };

            // Define your template name and language
            // (Replace these with the actual template name and language code you have in your WhatsApp templates)
            string templateName = "welcome";
            string templateLanguage = "te";

            // Create the message template
            var messageTemplate = new MessageTemplate(templateName, templateLanguage);

            // If your template supports parameters, you can set them using messageTemplate.TemplateParameters
            // e.g.: messageTemplate.TemplateParameters = new List<string> { messageText };

            // Assemble the template content
            var templateContent = new TemplateNotificationContent(
                _channelRegistrationId,
                recipientList,
                messageTemplate
            );

            // Send the template message
            Response<SendMessageResult> sendTemplateMessageResult =
                await _notificationMessagesClient.SendAsync(templateContent);

            // Check for any errors
            if (sendTemplateMessageResult.GetRawResponse().IsError)
            {
                throw new Exception(
                    $"Failed to send WhatsApp message: {sendTemplateMessageResult.GetRawResponse().ReasonPhrase}");
            }

            Console.WriteLine($"WhatsApp message sent successfully to {phoneNumber}");
        }

        /// <summary>
        /// Placeholder for sending interactive WhatsApp messages if your use case requires it.
        /// </summary>
        public async Task SendInteractiveWhatsAppMessage(object messagePayload)
        {
            // Implement interactive message logic here if needed.
            // This might involve setting up interactive message templates and custom payloads.
            await Task.CompletedTask;
            throw new NotImplementedException("Interactive WhatsApp messages are not yet implemented.");
        }
    }
}
