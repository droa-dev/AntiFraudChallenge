using Domain.Models;
using Domain.Repositories;

namespace TransactionApi.Services
{
    public class AccountService(IUnitOfWork unitOfWork, IRepository<Account> accountRepository)
    {
        public async Task<Records.Account?> GetAccountById(Guid id)
        {
            var entity = await accountRepository.GetById(id);
            return entity switch
            {
                null => null,
                _ => Records.Account.FromDomainModel(entity),
            };
        }
        public async Task<IEnumerable<Records.Account>> GetAllAccounts(CancellationToken cancellationToken = default)
        {
            var accounts = await accountRepository.GetAll(cancellationToken);
            return accounts.Select(Records.Account.FromDomainModel);
        }
        public async Task AddAccount(Records.Account account, CancellationToken cancellationToken = default)
        {
            var domainAccount = account.ToDomainModel();
            await accountRepository.Add(domainAccount, cancellationToken);
            await unitOfWork.SaveChanges(cancellationToken);
        }
        public async Task UpdateAccount(Records.UpdateAccount account)
        {
            if (account.Id == Guid.Empty)
            {
                throw new ArgumentException("Account ID cannot be empty.");
            }

            var existingAccount = await accountRepository.GetById(account.Id) ?? throw new ArgumentException($"Account with ID {account.Id} does not exist.");
            existingAccount.Balance = account.Balance;
            existingAccount.Active = account.Active;
            accountRepository.Update(existingAccount);
            await unitOfWork.SaveChanges();
        }

    }
}
