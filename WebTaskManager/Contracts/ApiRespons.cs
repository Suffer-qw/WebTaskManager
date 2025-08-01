namespace WebTaskManager.Contracts
{
    public class ApiResponse<T>
    {
        //Generic-класс (обобщённый класс, дженерик) в ASP.NET — это класс, который может работать с различными типами данных,
        //без указания фактического типа до тех пор, пока не будет использован код.
        //generic-класс для стандартизации ответов API
        //Стандартизация формата ответов - все ответы API имеют одинаковую структуру
        //Упрощение фронтенд-разработки - клиент всегда знает структуру ответа
        public T Data { get; set; }

        public string Message { get; set; }

        public ApiResponse(T data, string message)
        {
            Data = data;
            Message = message;
        }
    }
}
