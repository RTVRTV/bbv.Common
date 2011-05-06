//-------------------------------------------------------------------------------
// <copyright file="BehaviorWithStringContext.cs" company="bbv Software Services AG">
//   Copyright (c) 2008-2011 bbv Software Services AG
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
// </copyright>
//-------------------------------------------------------------------------------

namespace bbv.Common.Bootstrapper.Specification.Dummies
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class BehaviorWithStringContext : IBehavior<ICustomExtension>
    {
        private string input;

        private readonly string addition;

        public BehaviorWithStringContext(string input, string addition)
        {
            this.addition = addition;
            this.input = input;
        }

        public void Behave(IEnumerable<ICustomExtension> extensions)
        {
            extensions.First().Dump(string.Format(CultureInfo.InvariantCulture, "input modification with {0}.", this.addition));

            this.input += " " + this.addition;
        }
    }
}