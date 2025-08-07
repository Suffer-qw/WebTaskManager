﻿namespace WebTaskManager.Contracts
{
    public class MyTaskResponse
    {
        //Generic-класс (обобщённый класс, дженерик) в ASP.NET — это класс, который может работать с различными типами данных,
        //без указания фактического типа до тех пор, пока не будет использован код.
        //generic-класс для стандартизации ответов API
        //Стандартизация формата ответов - все ответы API имеют одинаковую структуру
        //Упрощение фронтенд-разработки - клиент всегда знает структуру ответа
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required string Type { get; set; }

        public required string Status { get; set; }
    }
}
