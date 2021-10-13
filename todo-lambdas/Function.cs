using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Amazon.Lambda.Core;
using Amazon.Lambda.APIGatewayEvents;
using todo_lambdas.dtos;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace todo_lambdas
{
    public class Functions
    {
        public static string server = "todolist-db.cnk2lsktvjkn.us-east-1.rds.amazonaws.com";
        public static string database = "todo";
        public static string user_id = "admin";
        public static string password = "wwe734412";
        public static string connstring = $"Server={server}; Database={database}; Uid={user_id}; Pwd={password};";
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }


        /// <summary>
        /// A Lambda function to create a new todo list in the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse PostList(APIGatewayProxyRequest request, ILambdaContext context)
        {
            string json = request.Body;
            List list = JsonConvert.DeserializeObject<List>(json);

                //opens db connection
                using (var conn = new MySqlConnection(connstring))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("INSERT INTO list (title,creation_date) VALUES (@title,@date);", conn))
                    {
                        cmd.Parameters.AddWithValue("title", list.Title);
                        cmd.Parameters.AddWithValue("date", list.Date);
                        cmd.ExecuteNonQuery();
                    }
                }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                Body = "",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
            };

            return response;
        }

        /// <summary>
        /// A Lambda function to gets all the todo lists in the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse GetLists(APIGatewayProxyRequest request, ILambdaContext context)
        {
            List<List> lists = new List<List>();
            //opens db connection
            using (var conn = new MySqlConnection(connstring))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM list;", conn))
                {
                    var read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        lists.Add(new List()
                        {
                            ListID = read.GetInt32(0),
                            Title = read.GetString(1),
                            Date = read.GetDateTime(2)
                        });
                    }
                }
            }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(lists),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
            };

            return response;
        }

        /// <summary>
        /// A Lambda function to update a todo list in the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse PutList(APIGatewayProxyRequest request, ILambdaContext context)
        {
            string json = request.Body;
            context.Logger.LogLine(json);
            List list = JsonConvert.DeserializeObject<List>(json);

            //opens db connection
            using (var conn = new MySqlConnection(connstring))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("UPDATE list SET title=@title WHERE list_id=@list_id;", conn))
                {
                    cmd.Parameters.AddWithValue("title", list.Title);
                    cmd.Parameters.AddWithValue("list_id", list.ListID);
                    cmd.ExecuteNonQuery();
                }
            }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
            };

            return response;
        }

        /// <summary>
        /// A Lambda function to delete a todo list in the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse DeleteList(APIGatewayProxyRequest request, ILambdaContext context)
        {
            IDictionary<string, string> pathParams = request.PathParameters;
            string listId = "";
            pathParams.TryGetValue("list-id", out listId);

            //opens db connection
            using (var conn = new MySqlConnection(connstring))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM list WHERE list_id=@list_id;", conn))
                {
                    cmd.Parameters.AddWithValue("list_id", listId);
                    cmd.ExecuteNonQuery();
                }
            }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
            };

            return response;
        }




        /// <summary>
        /// A Lambda function to create a new item in a list
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse PostItem(APIGatewayProxyRequest request, ILambdaContext context)
        {
            IDictionary<string, string> pathParams = request.PathParameters;
            string json = request.Body;
            string listId = "";
            pathParams.TryGetValue("list-id", out listId);
            Item list = JsonConvert.DeserializeObject<Item>(json);

                //opens db connection
                using (var conn = new MySqlConnection(connstring))
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand("INSERT INTO item (list_id,message,is_high_priority,is_completed) VALUES (@list_id,@message,@is_high_priority,@is_completed);", conn))
                    {
                        cmd.Parameters.AddWithValue("list_id", listId);
                        cmd.Parameters.AddWithValue("message", list.Message);
                        cmd.Parameters.AddWithValue("is_high_priority", list.IsHighPriority);
                        cmd.Parameters.AddWithValue("is_completed", list.IsCompleted);
                        cmd.ExecuteNonQuery();
                    }
                }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.Created,
                Body = "",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
            };

            return response;
        }

        /// <summary>
        /// A Lambda function to gets all the items in a todo list
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse GetItems(APIGatewayProxyRequest request, ILambdaContext context)
        {
            IDictionary<string, string> pathParams = request.PathParameters;
            List<Item> items = new List<Item>();
            string listId = "";
            pathParams.TryGetValue("list-id", out listId);
            //opens db connection
            using (var conn = new MySqlConnection(connstring))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("SELECT * FROM item WHERE list_id=@list_id;", conn))
                {
                    cmd.Parameters.AddWithValue("list_id", listId);
                    var read = cmd.ExecuteReader();
                    while (read.Read())
                    {
                        items.Add(new Item()
                        {
                            ItemID = read.GetInt32(0),
                            ListID = read.GetInt32(1),
                            Message = read.GetString(2),
                            IsHighPriority = read.GetBoolean(3),
                            IsCompleted = read.GetBoolean(4)
                        });
                    }
                }
            }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = JsonConvert.SerializeObject(items),
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
            };

            return response;
        }

        /// <summary>
        /// A Lambda function to update an item in a todo list
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse PutItem(APIGatewayProxyRequest request, ILambdaContext context)
        {
            string json = request.Body;
            context.Logger.LogLine(json);
            Item item = JsonConvert.DeserializeObject<Item>(json);

            //opens db connection
            using (var conn = new MySqlConnection(connstring))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("UPDATE item SET message=@message,is_high_priority=@is_high_priority,is_completed=@is_completed WHERE item_id=@item_id;", conn))
                {
                    cmd.Parameters.AddWithValue("item_id", item.ItemID);
                    cmd.Parameters.AddWithValue("message", item.Message);
                    cmd.Parameters.AddWithValue("is_high_priority", item.IsHighPriority);
                    cmd.Parameters.AddWithValue("is_completed", item.IsCompleted);
                    cmd.ExecuteNonQuery();
                }
            }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
            };

            return response;
        }

        /// <summary>
        /// A Lambda function to delete an item in a todo list
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse DeleteItem(APIGatewayProxyRequest request, ILambdaContext context)
        {
            IDictionary<string, string> pathParams = request.PathParameters;
            string itemId = "";
            pathParams.TryGetValue("item-id", out itemId);

            //opens db connection
            using (var conn = new MySqlConnection(connstring))
            {
                conn.Open();
                using (var cmd = new MySqlCommand("DELETE FROM item WHERE item_id=@item_id;", conn))
                {
                    cmd.Parameters.AddWithValue("item_id", itemId);
                    cmd.ExecuteNonQuery();
                }
            }

            context.Logger.LogLine("Get Request\n");

            //http response
            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                Body = "",
                Headers = new Dictionary<string, string> { { "Content-Type", "application/json" }, { "Access-Control-Allow-Origin", "*" } }
            };

            return response;
        }


    }
}
