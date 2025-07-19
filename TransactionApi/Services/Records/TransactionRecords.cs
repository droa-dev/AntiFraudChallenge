using Domain.Enums;

namespace TransactionApi.Services.Records;

public record CreateTransaction(Guid SourceAccountId, Guid TargetAccountId, decimal Value);
public record Transaction(Guid Id, Guid SourceAccountId, Guid TargetAccountId, decimal Value, TransactionStatus TransactionStatus, string Message, DateTimeOffset? CreatedAt, DateTimeOffset? LastModifiedAt)
{
    public static Transaction FromDomainModel(Domain.Models.Transaction transaction)
    {
        return new Transaction(
            transaction.Id,
            transaction.SourceAccount.Id,
            transaction.TargetAccount.Id,
            transaction.Value,
            transaction.TransactionStatus,
            transaction.Message,
            transaction.CreatedAt,
            transaction.LastModifiedAt);
    }

    public Domain.Models.Transaction ToDomainModel(Domain.Models.Account source, Domain.Models.Account target)
    {
        return new Domain.Models.Transaction
        {
            Id = Id,
            SourceAccount = source,
            TargetAccount = target,
            Value = Value,
            TransactionStatus = TransactionStatus,
            Message = Message,
            CreatedAt = CreatedAt,
            LastModifiedAt = LastModifiedAt
        };
    }
}
