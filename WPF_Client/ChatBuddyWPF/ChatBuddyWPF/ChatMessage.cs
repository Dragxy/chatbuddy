using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatBuddyWPF
{
    public class ChatMessage
    {
        [JsonPropertyName("content")]
        public string Content { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("publish_time")]
        public string PublishTimeStr { get; set; }
        [JsonIgnore]
        public DateTime PublishTime => ConvertToLocalDateTime(PublishTimeStr);

        [JsonPropertyName("type")]
        [JsonConverter(typeof(MessageTypeConverter))]
        public MessageType Type { get; set; }

        private DateTime ConvertToLocalDateTime(string localDateTimeStr)
        {
            return DateTime.Parse(localDateTimeStr);
        }
    }

    public enum MessageType
    {
        CHAT, LEAVE, JOIN
    }

    public class MessageTypeConverter : JsonConverter<MessageType>
    {
        public override MessageType Read(ref Utf8JsonReader reader , Type typeToConvert , JsonSerializerOptions options)
        {
            if(reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException($"Unexpected token type. Expected String but found {reader.TokenType}");
            }

            string value = reader.GetString();

            return value.ToLower() switch
            {
                "chat" => MessageType.CHAT,
                "leave" => MessageType.LEAVE,
                "join" => MessageType.JOIN,
                _ => throw new JsonException($"Unknown MessageType value: {value}")
            };
        }



        public override void Write(Utf8JsonWriter writer , MessageType value , JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToUpper());
        }
    }
}
