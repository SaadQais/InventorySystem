using InventorySystem.Application.Models;

namespace InventorySystem.Application.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
