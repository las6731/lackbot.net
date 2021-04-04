using System;

namespace LackBot.Common.Models
{
    /// <summary>
    /// A wrapper for <see cref="Result"/> that also includes either a <see cref="T"/> value, or an error message
    /// that describes the outcome.
    /// </summary>
    /// <typeparam name="T">The type of value that was acted on.</typeparam>
    public class ResultExtended<T>
    {
        /// <summary>
        /// The result.
        /// </summary>
        public readonly Result Result;
        
        /// <summary>
        /// The value; default if none is provided.
        /// </summary>
        public readonly T Value;
        
        /// <summary>
        /// The error message, if there is one.
        /// </summary>
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

        /// <summary>
        /// Indicates if the result was successful.
        /// </summary>
        public bool IsSuccess => Result.IsSuccess();

        /// <summary>
        /// Create a successful result, containing a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The created result.</returns>
        public static ResultExtended<T> Success(T value) => new ResultExtended<T>(Result.Success, value);
        
        /// <summary>
        /// Creates an unchanged result, containing a value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The created result.</returns>
        public static ResultExtended<T> NoChange(T value) => new ResultExtended<T>(Result.NoChange, value);
        
        /// <summary>
        /// Creates an unchanged result, containing an error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>The created result.</returns>
        public static ResultExtended<T> NoChange(string error) => new ResultExtended<T>(Result.NoChange, error);
        
        /// <summary>
        /// Creates a failed result, containing an error message.
        /// </summary>
        /// <param name="error">The error message.</param>
        /// <returns>The created result.</returns>
        public static ResultExtended<T> Failure(string error) => new ResultExtended<T>(Result.Failure, error);
    }

    /// <summary>
    /// Enum representing the possible outcomes of an action.
    /// </summary>
    public enum Result
    {
        Success = 0,
        NoChange = 1,
        Failure = 2
    }

    public static class ResultExtensions
    {
        /// <summary>
        /// Returns whether the result is successful.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns>A bool indicating if the result is successful.</returns>
        public static bool IsSuccess(this Result result)
        {
            return result == Result.Success;
        }
    } 
}