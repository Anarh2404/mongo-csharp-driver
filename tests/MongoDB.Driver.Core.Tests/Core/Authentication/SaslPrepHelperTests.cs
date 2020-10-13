/* Copyright 2018–present MongoDB Inc.
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

using FluentAssertions;
using MongoDB.Bson.TestHelpers;
using MongoDB.Bson.TestHelpers.XunitExtensions;
using System;
using Xunit;

namespace MongoDB.Driver.Core.Authentication
{
    /// <summary>
    /// SaslPrep unit tests.
    /// </summary>
    public class SaslPrepHelperTests
    {
        // Currently, we only support SaslPrep in .NET Framework due to a lack of a string normalization function in
        // .NET Standard

        private static readonly Lazy<int> _unassignedCodePoint = new Lazy<int>(FindUnassignedCodePoint);

        private static int FindUnassignedCodePoint()
        {
            for (var i = SaslPrepHelperReflector.MaxCodepoint; SaslPrepHelperReflector.MinCodepoint <= i; --i)
            {
                if (!SaslPrepHelperReflector.IsDefined(i) && !SaslPrepHelperReflector.Prohibited(i))
                {
                    return i;
                };
            }
            throw new Exception("Unable to find unassigned codepoint.");
        }
    }

    public static class SaslPrepHelperReflector
    {
        public static int MaxCodepoint =>
            (int)Reflector.GetStaticFieldValue(typeof(SaslPrepHelper), nameof(MaxCodepoint));

        public static int MinCodepoint =>
            (int)Reflector.GetStaticFieldValue(typeof(SaslPrepHelper), nameof(MinCodepoint));

        public static bool IsDefined(int codepoint) =>
            (bool)Reflector.InvokeStatic(typeof(SaslPrepHelper), nameof(IsDefined), codepoint);

        public static bool Prohibited(int codepoint) =>
            (bool)Reflector.InvokeStatic(typeof(SaslPrepHelper), nameof(Prohibited), codepoint);
    }
}
