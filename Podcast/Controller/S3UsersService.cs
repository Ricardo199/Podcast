using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Podcast.Model;

namespace Podcast.Controller
{
    public class S3UsersService
    {
        private readonly IAmazonDynamoDB _amazonDynamoDB;
        private readonly string _tableName;

        public S3UsersService(IAmazonDynamoDB dynamoClient, string tableName)
        {
            _amazonDynamoDB = dynamoClient;
            _tableName = tableName;
        }

        //Create User
        public async Task CreateUserAsync(User user) {
            var userDict = new Dictionary<string, AttributeValue> {
                ["ID"] = new AttributeValue { S = user.Id },
                ["Name"] = new AttributeValue {S = user.Name},
                ["Email"] = new AttributeValue {S = user.Email},
                ["Podcasts"] = new AttributeValue { SS = user.Podcasts.ToList() },
                ["IsDeleted"] = new AttributeValue { BOOL = user.IsDeleted }
            };
            await _amazonDynamoDB.PutItemAsync(_tableName, userDict);
        }

        //Get User
        public async Task<User?> GetUserAsync(string id) {
            var key = new Dictionary<string, AttributeValue> {
                ["Id"] = new AttributeValue { S = id }
            };
            var response = await _amazonDynamoDB.GetItemAsync(_tableName, key);
            if (!response.IsItemSet || response.Item["IsDeleted"].BOOL == true) return null;
            
            return new User {
                Id = response.Item["ID"].S,
                Name = response.Item["Name"].S,
                Email = response.Item["Email"].S,
                Podcasts = response.Item["Podcasts"].SS.ToArray(),
                IsDeleted = response.Item["IsDeleted"].BOOL ?? false
            };
        }

        //Update
        public async Task UpdateUserAsync(User user) {
            var key = new Dictionary<string, AttributeValue>
            {
                ["Id"] = new AttributeValue { S = user.Id }
            };

            var updates = new Dictionary<string, AttributeValueUpdate>
            {
                ["Name"] = new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = user.Name } },
                ["Password"] = new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = user.Password } },
                ["Email"] = new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { S = user.Email } },
                ["Podcasts"] = new AttributeValueUpdate { Action = AttributeAction.PUT, Value = new AttributeValue { SS = user.Podcasts.ToList() } }
            };
            await _amazonDynamoDB.UpdateItemAsync(_tableName, key, updates);
        }

        //Delete Users
        public async Task DeleteUserAsync(string id) {
            var key = new Dictionary<string, AttributeValue>
            {
                ["Id"] = new AttributeValue { S = id }
            };
            var updates = new Dictionary<string, AttributeValueUpdate> {
                ["IsDeleted"] = new AttributeValueUpdate
                {
                    Action = AttributeAction.PUT,
                    Value = new AttributeValue { BOOL = true }
                }
            };
            await _amazonDynamoDB.UpdateItemAsync(_tableName, key, updates);
        }
    }
}
