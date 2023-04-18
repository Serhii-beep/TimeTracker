namespace TimeTracker.Services
{
    public class ResponseModel<T>
    {
        public bool IsSuccess { get; private set; }

        public T Data { get; }

        public string Error { get; private set; }

        public int StatusCode { get; private set; }

        private ResponseModel()
        { }

        private ResponseModel(T data)
        {
            Data = data;
        }

        public static ResponseModel<T> Success(T data)
        {
            ResponseModel<T> result = new(data);
            result.IsSuccess = true;
            return result;
        }

        public static ResponseModel<T> Failure(int statusCode, string error)
        {
            ResponseModel<T> result = new()
            {
                IsSuccess = false,
                Error = error,
                StatusCode = statusCode
            };
            return result;
        }
    }
}
