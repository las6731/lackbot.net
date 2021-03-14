using System;

namespace LackBot.Common.Models
{
    public class ResultExtended<T>
    {
        public readonly Result Result;
        public readonly T Value;
        public readonly string Error;

        public ResultExtended(Result result, T value)
        {
            Result = result;
            Value = value;
            Error = String.Empty;
        }

        public ResultExtended(Result result, string error)
        {
            Result = result;
            Value = default;
            Error = error;
        }

        public bool IsSuccess => Result.IsSuccess();

        public static ResultExtended<T> Success(T value) => new ResultExtended<T>(Result.Success, value);
        
        public static ResultExtended<T> NoChange(T value) => new ResultExtended<T>(Result.NoChange, value);
        
        public static ResultExtended<T> Failure(string error) => new ResultExtended<T>(Result.Failure, error);
    }

    public enum Result
    {
        Success = 0,
        NoChange = 1,
        Failure = 2
    }

    public static class ResultExtensions
    {
        public static bool IsSuccess(this Result result)
        {
            return result == Result.Success;
        }
    } 
}