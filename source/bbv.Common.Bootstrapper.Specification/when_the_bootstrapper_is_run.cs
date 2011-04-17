//-------------------------------------------------------------------------------
// <copyright file="when_the_bootstrapper_is_run.cs" company="bbv Software Services AG">
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

namespace bbv.Common.Bootstrapper.Specification
{
    using Machine.Specifications;

    public class When_the_bootstrapper_is_run : BootstrapperSpecification
    {
        Establish context = () =>
            {
                Bootstrapper.Initialize(Strategy);
                Bootstrapper.AddExtension(First);
                Bootstrapper.AddExtension(Second);
            };

        Because of = () =>
            {
                Bootstrapper.Run();
            };

        It should_execute_the_extensions_in_the_correct_order;

        It should_pass_the_initialized_values_to_the_extension;

        It should_execute_the_extension_point_according_to_the_strategy_defined_order;
    }
}