using System.Threading.Tasks;

namespace SmartBank.TransactionService.Messaging
{
    public interface IRabbitMQPublisher
    {
        Task PublishMoneyDepositedEventAsync(MoneyDepositedEvent request);
        Task PublishMoneyWithdrawnEventAsync(MoneyWithdrawnEvent request);
        Task PublishMoneyTransferredEventAsync(MoneyTransferredEvent request);
    }
}
