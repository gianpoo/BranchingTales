using StoryTeller.Core.Interfaces;

namespace StoryTeller.Infrastructure.Email;

public class MimeKitEmailSender(ILogger<MimeKitEmailSender> logger,
  IOptions<MailserverConfiguration> mailserverOptions) : IEmailSender
{
  private readonly ILogger<MimeKitEmailSender> _logger = logger;
  private readonly MailserverConfiguration _mailserverConfiguration = mailserverOptions.Value!;


  //must become async when functional
  public Task SendEmailAsync(string to, string from, string subject, string body)
  {
    _logger.LogWarning("Sending email to {to} from {from} with subject {subject} using {type}.", to, from, subject, this.ToString());
    /*
    using var client = new MailKit.Net.Smtp.SmtpClient();
    await client.ConnectAsync(_mailserverConfiguration.Hostname,
      _mailserverConfiguration.Port, false);
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress(from, from));
    message.To.Add(new MailboxAddress(to, to));
    message.Subject = subject;
    message.Body = new TextPart("plain") { Text = body };

    await client.SendAsync(message);

    // Corrected cancellation token
    await client.DisconnectAsync(true, CancellationToken.None);
    */
   return Task.CompletedTask;
  }
}
