//-------------------------------------------------------------------------------
// <copyright file="IEventBrokerExtension.cs" company="bbv Software Services AG">
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

namespace bbv.Common.EventBroker
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    using bbv.Common.EventBroker.Handlers;

    using Internals;
    using Matchers;

    /// <summary>
    /// Interface for implementing event broker extensions.
    /// </summary>
    public interface IEventBrokerExtension
    {
        /// <summary>
        /// Called when an event is fired.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void FiringEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e);

        /// <summary>
        /// Called when the event was fired (processing completed).
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void FiredEvent(IEventTopicInfo eventTopic, IPublication publication, object sender, EventArgs e);

        /// <summary>
        /// Called when an item was registered.
        /// </summary>
        /// <param name="item">The item that was registered.</param>
        void RegisteredItem(object item);

        /// <summary>
        /// Called when an item was unregistered.
        /// </summary>
        /// <param name="item">The item that was unregistered.</param>
        void UnregisteredItem(object item);

        /// <summary>
        /// Called after a (potential) publisher was processed.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        /// <param name="register">Whether the publisher is registered; or unregistered.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        void ProcessedPublisher(object publisher, bool register, IEventTopicHost eventTopicHost);

        /// <summary>
        /// Called after a (potential) subscriber was processed.
        /// </summary>
        /// <param name="subscriber">The subscriber.</param>
        /// <param name="register">Whether the subscriber is registered; or unregistered.</param>
        /// <param name="eventTopicHost">The event topic host.</param>
        void ProcessedSubscriber(object subscriber, bool register, IEventTopicHost eventTopicHost);

        /// <summary>
        /// Called after an event topic was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        void CreatedTopic(IEventTopicInfo eventTopic);

        /// <summary>
        /// Called after a publication was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        void CreatedPublication(IEventTopicInfo eventTopic, IPublication publication);

        /// <summary>
        /// Called after a subscription was created.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="subscription">The subscription.</param>
        void CreatedSubscription(IEventTopicInfo eventTopic, ISubscription subscription);

        /// <summary>
        /// Called after a publication was added to an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        void AddedPublication(IEventTopicInfo eventTopic, IPublication publication);

        /// <summary>
        /// Called after a publication was removed from an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic. Null if removed by code.</param>
        /// <param name="publication">The publication.</param>
        void RemovedPublication(IEventTopicInfo eventTopic, IPublication publication);

        /// <summary>
        /// Called after a subscription was added to an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="subscription">The subscription.</param>
        void AddedSubscription(IEventTopicInfo eventTopic, ISubscription subscription);

        /// <summary>
        /// Called after a subscription was removed from an event topic.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="subscription">The subscription.</param>
        void RemovedSubscription(IEventTopicInfo eventTopic, ISubscription subscription);

        /// <summary>
        /// Called after an event topic was disposed.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        void Disposed(IEventTopicInfo eventTopic);

        /// <summary>
        /// Called when an exception occurred during event handling by a subscriber.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="context">The context providing information whether the exception is handled by an extension or is re-thrown.</param>
        void SubscriberExceptionOccurred(IEventTopicInfo eventTopic, Exception exception, ExceptionHandlingContext context);

        /// <summary>
        /// Called before an event is relayed from the publication to the subscribers.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void RelayingEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, IHandler handler, object sender, EventArgs e);

        /// <summary>
        /// Called after the event was relayed from the publication to the subscribers.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="handler">The handler.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void RelayedEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, IHandler handler, object sender, EventArgs e);

        /// <summary>
        /// Called when a publication or subscription matcher did not match and the event was not relayed to a subscription.
        /// </summary>
        /// <param name="eventTopic">The event topic.</param>
        /// <param name="publication">The publication.</param>
        /// <param name="subscription">The subscription.</param>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void SkippedEvent(IEventTopicInfo eventTopic, IPublication publication, ISubscription subscription, object sender, EventArgs e);
    }
}