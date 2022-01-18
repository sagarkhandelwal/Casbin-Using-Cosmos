namespace ResourceTokenConsole
{
    using System;
    using Newtonsoft.Json;

    public class Employee
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "empName")]
        public string EmpName { get; set; }

        public string AccountNumber { get; set; }

    }

    
}