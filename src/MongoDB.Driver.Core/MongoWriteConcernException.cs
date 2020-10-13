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

using MongoDB.Driver.Core.Connections;
using MongoDB.Driver.Core.Misc;

namespace MongoDB.Driver
{
    /// <summary>
    /// Represents a MongoDB write concern exception.
    /// </summary>

    public class MongoWriteConcernException : MongoCommandException
    {
        #region static
        private static void AddErrorLabelsFromWriteConcernResult(MongoWriteConcernException exception, WriteConcernResult writeConcernResult)
        {
            // note: make a best effort to extract the error labels from the writeConcernResult, but never throw an exception
            if (writeConcernResult != null && writeConcernResult.Response != null)
            {
                if (writeConcernResult.Response.TryGetValue("writeConcernError", out var writeConcernError) &&
                    writeConcernError.IsBsonDocument)
                {
                    AddErrorLabelsFromCommandResult(exception, writeConcernError.AsBsonDocument);
                }
            }
        }
        #endregion

        // fields
        private readonly WriteConcernResult _writeConcernResult;

        // constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoWriteConcernException"/> class.
        /// </summary>
        /// <param name="connectionId">The connection identifier.</param>
        /// <param name="message">The error message.</param>
        /// <param name="writeConcernResult">The command result.</param>
        public MongoWriteConcernException(ConnectionId connectionId, string message, WriteConcernResult writeConcernResult)
            : base(connectionId, message, null, writeConcernResult.Response)
        {
            _writeConcernResult = Ensure.IsNotNull(writeConcernResult, nameof(writeConcernResult));

            AddErrorLabelsFromWriteConcernResult(this, _writeConcernResult);
        }

        // properties
        /// <summary>
        /// Gets the write concern result.
        /// </summary>
        /// <value>
        /// The write concern result.
        /// </value>
        public WriteConcernResult WriteConcernResult
        {
            get { return _writeConcernResult; }
        }

        /// <summary>
        /// Determines whether the exception is due to a write concern error only.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the exception is due to a write concern error only; otherwise, <c>false</c>.
        /// </returns>
        public bool IsWriteConcernErrorOnly()
        {
            return Result != null && Result.Contains("ok") && Result["ok"].ToBoolean() && Result.Contains("writeConcernError");
        }
    }
}
