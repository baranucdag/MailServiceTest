using EmailService.Entities;

namespace Infrastructure.Abstract
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
    }
}
