namespace SharedModel.Responses;

public record AddStudentsResult(
    int SuccessCount,
    int FailedCount,
    List<string> Errors
);
