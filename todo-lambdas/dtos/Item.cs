using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace todo_lambdas.dtos
{
    public class Item
    {
        [JsonProperty("ItemID")]
        public int ItemID { get; set; }
        [JsonProperty("ListID")]
        public int ListID { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("IsHighPriority")]
        public bool IsHighPriority{ get; set; }
        [JsonProperty("IsCompleted")]
        public bool IsCompleted { get; set; }
    }
}
