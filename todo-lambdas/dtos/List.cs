using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace todo_lambdas.dtos
{
    public class List
    {
        [JsonProperty("ListID")]
        public int ListID { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Date")]
        public DateTime Date { get; set; }

    }
}
