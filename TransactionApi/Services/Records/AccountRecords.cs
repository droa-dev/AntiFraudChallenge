namespace TransactionApi.Services.Records;

public record CreateAccount(decimal Balance);
public record UpdateAccount(Guid Id, decimal Balance, bool Active);
public record Account(Guid Id, decimal Balance, bool Active, DateTimeOffset? CreatedAt, DateTimeOffset? LastModifiedAt)
{
    public static Account FromDomainModel(Domain.Models.Account account)
    {
        return new Account(account.Id, account.Balance, account.Active, account.CreatedAt, account.LastModifiedAt);
    }
    public Domain.Models.Account ToDomainModel()
    {
        return new Domain.Models.Account
        {
            Id = Id,
            Balance = Balance,
            Active = Active,
            CreatedAt = CreatedAt,
            LastModifiedAt = LastModifiedAt
        };
    }
}