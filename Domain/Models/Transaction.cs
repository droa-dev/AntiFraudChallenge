using Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Transaction : IEntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.NewGuid();
    public required decimal Value { get; set; }
    public required Account SourceAccount { get; set; }
    public required Account TargetAccount { get; set; }
    public required TransactionStatus TransactionStatus { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}
