//-------------------------------------------------------------------------------
// <copyright file="Subscriber.cs" company="bbv Software Services AG">
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

namespace bbv.Common.MappingEventBroker
{
    using bbv.Common.EventBroker;

    public class Subscriber
    {
        public DestinationEventArgs MappedSubscriptionEventArgs { get; private set; }

        public SourceEventArgs SubscriptionEventArgs { get; private set; }

        [EventSubscription(@"mapped://Publisher", typeof(Common.EventBroker.Handlers.Publisher))]
        public void HandleMapped(object sender, DestinationEventArgs e)
        {
            this.MappedSubscriptionEventArgs = e;
        }

        [EventSubscription(@"userdefined://Publisher", typeof(Common.EventBroker.Handlers.Publisher))]
        public void HandleUserDefined(object sender, DestinationEventArgs e)
        {
            this.MappedSubscriptionEventArgs = e;
        }

        [EventSubscription(Topics.Publisher, typeof(Common.EventBroker.Handlers.Publisher))]
        public void HandleSubscription(object sender, SourceEventArgs e)
        {
            this.SubscriptionEventArgs = e;
        }
    }
}