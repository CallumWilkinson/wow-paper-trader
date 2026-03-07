public sealed record WowApiResult<T>
(
    T DtoPayload,
    DateTime DataReturnedAtUtc,
    string Endpoint
);