namespace API.OneOfErrors;

public record AppError(string detail, string error);

public record EmptyElementInsertError() : AppError("Cannot insert empty object", nameof(EnumCrudErrors.EmptyElementInsertError));

public record InvalidObjectError() : AppError("Invalid object", nameof(EnumCrudErrors.InvalidObjectError));

public record BusinessRulesError() : AppError("Error in business rules", nameof(EnumCrudErrors.BusinessRulesError));

public record DuplicatedError() : AppError("Cannot insert duplicated object", nameof(EnumCrudErrors.DuplicatedError));

public record NotFoundError() : AppError("Not object found", nameof(EnumCrudErrors.NotFoundError));

public record NullError() : AppError("Cannot work with null element", nameof(EnumCrudErrors.NullError));

public record UnauthorizadedError() : AppError("Unauhtorizaded action", nameof(EnumCrudErrors.Unauthorizaded));

public record BadRequestError() : AppError("BadRequest action", nameof(EnumCrudErrors.BadRequest));