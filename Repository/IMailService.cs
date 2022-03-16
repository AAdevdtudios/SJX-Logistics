using SjxLogistics.Models.Request;
using System.Threading.Tasks;

namespace SjxLogistics.Repository
{
    public interface IMailService
    {
        Task SendEmailAsync(EmailRequest mailRequest);
    }
}
