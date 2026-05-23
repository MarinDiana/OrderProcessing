namespace OrderProcessing.Api.Validation;

public record ValidationResult(
    bool IsValid,
    List<string> Errors)
{
    public static ValidationResult Success()
        => new(true, new List<string>());

    public static ValidationResult Failure(string error)
        => new(false, new List<string> { error });
}