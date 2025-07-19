using Confluent.Kafka;
using Domain.Models;
using Domain.Repositories;
using System.Text.Json;

namespace TransactionApi.Services
{
    public class TransactionService(IUnitOfWork unitOfWork, IRepository<Transaction> transactionRepository, IRepository<Account> accountRepository, IProducer<Null, string> kafkaProducer)
    {
        private const string newTransactionsTopic = "new-transactions";

        public async Task<Records.Transaction?> GetTransactionById(Guid id)
        {
            var entity = await transactionRepository.GetById(id);
            return entity switch
            {
                null => null,
                _ => Records.Transaction.FromDomainModel(entity),
            };
        }
        public async Task<IEnumerable<Records.Transaction>> GetAllTransactions(CancellationToken cancellationToken = default)
        {
            var transactions = await transactionRepository.GetAll(cancellationToken);
            return transactions.Select(Records.Transaction.FromDomainModel);
        }
        public async Task AddTransaction(Records.Transaction transaction, CancellationToken cancellationToken = default)
        {
            var sourceAccount = await accountRepository.GetById(transaction.SourceAccountId) ?? throw new ArgumentException($"Source account with ID {transaction.SourceAccountId} does not exist.");
            var targetAccount = await accountRepository.GetById(transaction.TargetAccountId) ?? throw new ArgumentException($"Target account with ID {transaction.TargetAccountId} does not exist.");
            var domainTransaction = transaction.ToDomainModel(sourceAccount, targetAccount);
            await transactionRepository.Add(domainTransaction, cancellationToken);
            await unitOfWork.SaveChanges(cancellationToken);
            await kafkaProducer.ProduceAsync(newTransactionsTopic, new Message<Null, string> { Value = JsonSerializer.Serialize(transaction) }, cancellationToken);
        }
    }
}
