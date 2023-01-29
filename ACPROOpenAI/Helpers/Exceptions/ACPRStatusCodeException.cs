using System;
using System.Runtime.Serialization;
using System.Net;
using PX.Common;
using ACPROpenAI.Descriptor;

namespace ACPROpenAI.Helpers.Exceptions
{
    public class ACPRStatusCodeException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string Content { get; private set; }

        /// <summary>
        /// HttpStatusCode and string content and it throws an exception with a message 'Received a {statusCode} status code' 
        /// which can be used to display more detailed information about the error.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <param name="content"></param>
        public ACPRStatusCodeException(HttpStatusCode statusCode, string content) : base(string.Format(ACPRMessages.StatusCodeError, statusCode.ToString(), content))
        {
            StatusCode = statusCode;
            Content = content;
        }

        /// <summary>
        /// This constructor is typically used when an exception object is being deserialized, 
        /// for example when it is being passed across a remoting boundary or when it is being deserialized from a persisted state.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ACPRStatusCodeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        /// <summary>
        /// It is used to serialize an exception object to a stream, so it can be persisted or passed across remoting boundaries.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            ReflectionSerializer.GetObjectData(this, info);
            base.GetObjectData(info, context);
        }
    }
}
