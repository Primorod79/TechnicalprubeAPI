namespace EcommerceAPI.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string Message { get; set; }
        public object? Errors { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse(bool success, T? data, string message, object? errors, int statusCode)
        {
            Success = success;
            Data = data;
            Message = message;
            Errors = errors;
            StatusCode = statusCode;
        }

        public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
        {
            return new ApiResponse<T>(true, data, message, null, 200);
        }

        public static ApiResponse<T> Failure(string message, object? errors = null, int statusCode = 400)
        {
            return new ApiResponse<T>(false, default, message, errors, statusCode);
        }
    }
}