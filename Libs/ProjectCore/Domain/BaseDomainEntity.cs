namespace ProjectCore.Domain;

public class BaseDomainEntity<T>
{
    public required T Id { get; set; }
}