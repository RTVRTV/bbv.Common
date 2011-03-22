//-------------------------------------------------------------------------------
// <copyright file="CsvStateMachineReportGeneratorTest.cs" company="bbv Software Services AG">
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

namespace bbv.Common.StateMachine.Reports
{
    using System.IO;

    using FluentAssertions;

    using Xunit;

    public class CsvStateMachineReportGeneratorTest
    {
        private readonly MemoryStream stateStream;

        private readonly Stream transitionsStream;

        private readonly CsvStateMachineReportGenerator<States, Events> testee;

        public CsvStateMachineReportGeneratorTest()
        {
            this.stateStream = new MemoryStream();
            this.transitionsStream = new MemoryStream();

            this.testee = new CsvStateMachineReportGenerator<States, Events>(this.stateStream, this.transitionsStream);
        }

        ~CsvStateMachineReportGeneratorTest()
        {
            this.stateStream.Dispose();
        }

        /// <summary>
        /// Some test states for simulating an elevator.
        /// </summary>
        private enum States
        {
            /// <summary>Elevator has an Error</summary>
            Error,

            /// <summary>Elevator is healthy, i.e. no error</summary>
            Healthy,

            /// <summary>The elevator is moving (either up or down)</summary>
            Moving,

            /// <summary>The elevator is moving down.</summary>
            MovingUp,

            /// <summary>The elevator is moving down.</summary>
            MovingDown,

            /// <summary>The elevator is standing on a floor.</summary>
            OnFloor,

            /// <summary>The door is closed while standing still.</summary>
            DoorClosed,

            /// <summary>The dor is open while standing still.</summary>
            DoorOpen
        }

        /// <summary>
        /// Some test events for the elevator
        /// </summary>
        private enum Events
        {
            /// <summary>An error occurred.</summary>
            ErrorOccured,

            /// <summary>Reset after error.</summary>
            Reset,

            /// <summary>Open the door.</summary>
            OpenDoor,

            /// <summary>Close the door.</summary>
            CloseDoor,

            /// <summary>Move elevator up.</summary>
            GoUp,

            /// <summary>Move elevator down.</summary>
            GoDown,

            /// <summary>Stop the elevator.</summary>
            Stop
        }

        [Fact]
        public void Report()
        {
            var elevator = new PassiveStateMachine<States, Events>("Elevator");

            elevator.DefineHierarchyOn(States.Healthy, States.OnFloor, HistoryType.Deep, States.OnFloor, States.Moving);
            elevator.DefineHierarchyOn(States.Moving, States.MovingUp, HistoryType.Shallow, States.MovingUp, States.MovingDown);
            elevator.DefineHierarchyOn(States.OnFloor, States.DoorClosed, HistoryType.None, States.DoorClosed, States.DoorOpen);

            elevator.In(States.Healthy)
                .On(Events.ErrorOccured).Goto(States.Error);

            elevator.In(States.Error)
                .On(Events.Reset).Goto(States.Healthy)
                .On(Events.ErrorOccured);

            elevator.In(States.OnFloor)
                .ExecuteOnEntry(AnnounceFloor)
                .ExecuteOnExit(Beep, Beep)
                .On(Events.CloseDoor).Goto(States.DoorClosed)
                .On(Events.OpenDoor).Goto(States.DoorOpen)
                .On(Events.GoUp)
                    .If(CheckOverload).Goto(States.MovingUp)
                    .Otherwise().Execute(AnnounceOverload, Beep)
                .On(Events.GoDown)
                    .If(CheckOverload).Goto(States.MovingDown)
                    .Otherwise().Execute(AnnounceOverload);

            elevator.In(States.Moving)
                .On(Events.Stop).Goto(States.OnFloor);

            elevator.Initialize(States.OnFloor);

            elevator.Report(this.testee);

            string statesReport;
            string transitionsReport;
            this.stateStream.Position = 0;
            using (var reader = new StreamReader(this.stateStream))
            {
                statesReport = reader.ReadToEnd();
            }

            this.transitionsStream.Position = 0;
            using (var reader = new StreamReader(this.transitionsStream))
            {
                transitionsReport = reader.ReadToEnd();
            }

            const string ExpectedStatesReport = "Source;Entry;Exit;ChildrenOnFloor;AnnounceFloor;Beep,Beep;DoorClosed, DoorOpenMoving;;;MovingUp, MovingDownHealthy;;;OnFloor, MovingMovingUp;;;MovingDown;;;DoorClosed;;;DoorOpen;;;Error;;;";
            const string ExpectedTransitionsReport = "Source;Event;Guard;ActionsOnFloor;CloseDoor;;DoorClosed;OnFloor;OpenDoor;;DoorOpen;OnFloor;GoUp;CheckOverload;MovingUp;OnFloor;GoUp;;internal transition;AnnounceOverload,BeepOnFloor;GoDown;CheckOverload;MovingDown;OnFloor;GoDown;;internal transition;AnnounceOverloadMoving;Stop;;OnFloor;Healthy;ErrorOccured;;Error;Error;Reset;;Healthy;Error;ErrorOccured;;internal transition;";

            statesReport.Replace("\n", string.Empty).Replace("\r", string.Empty)
                .Should().Be(ExpectedStatesReport.Replace("\n", string.Empty).Replace("\r", string.Empty));

            transitionsReport.Replace("\n", string.Empty).Replace("\r", string.Empty)
                .Should().Be(ExpectedTransitionsReport.Replace("\n", string.Empty).Replace("\r", string.Empty));
        }

        private static void Beep()
        {
        }

        private static bool CheckOverload(object[] arg)
        {
            return true;
        }

        private static void AnnounceFloor()
        {
        }

        private static void AnnounceOverload(object[] arguments)
        {
        }

        private static void Beep(object[] arguments)
        {
        }
    }
}