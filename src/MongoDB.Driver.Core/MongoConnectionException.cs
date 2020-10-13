/* Copyright 2010-present MongoDB Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.IO;
using System.Net.Sockets;
using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver
{
    /// <summary>
    /// Represents a MongoDB connection exception.
    /// </summary>

    public class MongoConnectionException : MongoException
    {
        // fields
        private readonly ConnectionId _connectionId;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConnectionException"/> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="message">The error message.</param>
        public MongoConnectionException(ConnectionId connectionId, string message)
            : this(connectionId, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoConnectionException"/> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="message">The error message.</param>
        /// <param name="innerException">The inner exception.</param>
        public MongoConnectionException(ConnectionId connectionId, string message, Exception innerException)
            : base(message, innerException)
        {
            _connectionId = Ensure.IsNotNull(connectionId, nameof(connectionId));
        }

        // properties
        /// <summary>
        /// Gets the connection identifier.
        /// </summary>
        public ConnectionId ConnectionId
        {
            get { return _connectionId; }
        }

        /// <summary>
        /// Whether or not this exception contains a socket timeout exception.
        /// </summary>
        public bool ContainsSocketTimeoutException
        {
            get
            {
                for (var exception = InnerException; exception != null; exception = exception.InnerException)
                {
                    if (exception is SocketException socketException &&
                        socketException.SocketErrorCode == SocketError.TimedOut)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Determines whether the exception is network error or no.
        /// </summary>
        public virtual bool IsNetworkException => true; // true in subclasses, only if they can be considered as a network error

        // methods
    }
}
