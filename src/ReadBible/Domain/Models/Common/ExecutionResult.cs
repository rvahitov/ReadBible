using Monads;

namespace ReadBible.Domain.Models.Common
{
    public static class ExecutionResult
    {
        public static IResult<Nothing, string> Success() => new SuccessResult<Nothing, string>(Nothing.Instance);

        public static IResult<Nothing, string> Failure( string failure ) => new FailureResult<Nothing, string>(failure);
    }
}