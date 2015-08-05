using System;
using System.Linq;

namespace Smc
{
    public class Parser : ITokenCollector
    {
        public enum ParserState
        {
            Header,
            HeaderColon,
            StateSpec,
            HeaderValue,
            SuperStateName,
            StateModifier,
            End,
            SuperStateClose,
            EntryAction,
            ExitAction,
            StateBase,
            SingleEvent,
            SubTransitionGroup,
            SingleNextState,
            SingleActionGroup,
            SingleActionGroupName,
            GroupEvent,
            GroupNextState,
            GroupActionGroup,
            GroupActionGroupName
        }

        public enum ParserEvent
        {
            Name,
            OpenBrace,
            Colon,
            OpenParen,
            ClosedBrace,
            ClosedParen,
            OpenAngle,
            ClosedAngle,
            Dash,
            Eof
        }

        private ParserState _state = ParserState.Header;
        private readonly IBuilder _builder;

        public Parser(IBuilder builder)
        {
            _builder = builder;
        }

        public void OpenBrace(int line, int pos)
        {
            HandleEvent(ParserEvent.OpenBrace, line, pos);
        }

        public void ClosedBrace(int line, int pos)
        {
            HandleEvent(ParserEvent.ClosedBrace, line, pos);
        }

        public void OpenParen(int line, int pos)
        {
            HandleEvent(ParserEvent.OpenParen, line, pos);
        }

        public void ClosedParen(int line, int pos)
        {
            HandleEvent(ParserEvent.ClosedParen, line, pos);
        }

        public void OpenAngle(int line, int pos)
        {
            HandleEvent(ParserEvent.OpenAngle, line, pos);
        }

        public void ClosedAngle(int line, int pos)
        {
            HandleEvent(ParserEvent.ClosedAngle, line, pos);
        }

        public void Dash(int line, int pos)
        {
            HandleEvent(ParserEvent.Dash, line, pos);
        }

        public void Colon(int line, int pos)
        {
            HandleEvent(ParserEvent.Colon, line, pos);
        }

        public void Name(string name, int line, int pos)
        {
            _builder.SetName(name);
            HandleEvent(ParserEvent.Name, line, pos);
        }

        public void Error(int line, int pos)
        {
            _builder.SyntaxError(line, pos);
        }

        private void HandleEvent(ParserEvent parserEvent, int line, int pos)
        {
            var transition = _transitions.SingleOrDefault(t => t.CurrentState == _state && t.Event == parserEvent);

            if (null != transition)
            {
                _state = transition.NewState;
                transition.Action?.Invoke(_builder);
            }
            else
                HandleEventError(parserEvent, line, pos);
        }

        private void HandleEventError(ParserEvent parserEvent, int line, int pos)
        {
            switch (_state)
            {
                case ParserState.Header:
                case ParserState.HeaderValue:
                    _builder.HeaderError(_state, parserEvent, line, pos);
                    break;

                case ParserState.StateSpec:
                case ParserState.SuperStateName:
                case ParserState.SuperStateClose:
                case ParserState.StateModifier:
                case ParserState.ExitAction:
                case ParserState.EntryAction:
                case ParserState.StateBase:
                    _builder.StateSpecError(_state, parserEvent, line, pos);
                    break;

                case ParserState.SingleEvent:
                case ParserState.SingleNextState:
                case ParserState.SingleActionGroup:
                case ParserState.SingleActionGroupName:
                    _builder.TransitionError(_state, parserEvent, line, pos);
                    break;

                case ParserState.SubTransitionGroup:
                case ParserState.GroupEvent:
                case ParserState.GroupNextState:
                case ParserState.GroupActionGroup:
                case ParserState.GroupActionGroupName:
                    _builder.TransitionGroupError(_state, parserEvent, line, pos);
                    break;

                case ParserState.End:
                    _builder.EndError(_state, parserEvent, line, pos);
                    break;

                default:
                    throw new ApplicationException($"Unhandled parser state: '{_state}'.");
            }
        }

        class Transition
        {
            public Transition(ParserState currentState, ParserEvent evt, ParserState newState, Action<IBuilder> action)
            {
                CurrentState = currentState;
                Event = evt;
                NewState = newState;
                Action = action;
            }
            public ParserState CurrentState { get; private set; }
            public ParserEvent Event { get; private set; }
            public ParserState NewState { get; private set; }
            public Action<IBuilder> Action { get; private set; }
        }

        private Transition[] _transitions = new Transition[] {
            new Transition(ParserState.Header, ParserEvent.Name, ParserState.HeaderColon, t => t.NewHeaderWithName()), 
            new Transition(ParserState.Header, ParserEvent.OpenBrace, ParserState.StateSpec, null),
            new Transition(ParserState.HeaderColon, ParserEvent.Colon, ParserState.HeaderValue, null),
            new Transition(ParserState.HeaderValue, ParserEvent.Name, ParserState.Header, t => t.AddHeaderWithValue()),
            new Transition(ParserState.StateSpec, ParserEvent.OpenParen, ParserState.SuperStateName, null),
            new Transition(ParserState.StateSpec, ParserEvent.Name, ParserState.StateModifier, t => t.SetStateName()),
            new Transition(ParserState.StateSpec, ParserEvent.ClosedBrace, ParserState.End, t => t.Done()),
            new Transition(ParserState.SuperStateName, ParserEvent.Name, ParserState.SuperStateClose, t => t.SetSuperState()),
            new Transition(ParserState.SuperStateClose, ParserEvent.ClosedParen, ParserState.StateModifier, t => t.Done()),
            new Transition(ParserState.StateModifier, ParserEvent.OpenAngle, ParserState.EntryAction, null),
            new Transition(ParserState.StateModifier, ParserEvent.ClosedAngle, ParserState.ExitAction, null),
            new Transition(ParserState.StateModifier, ParserEvent.Colon, ParserState.StateBase, null),
            new Transition(ParserState.StateModifier, ParserEvent.Name, ParserState.SingleEvent, t => t.SetEvent()),
            new Transition(ParserState.StateModifier, ParserEvent.Dash, ParserState.SingleEvent, t => t.SetNullEvent()),
            new Transition(ParserState.StateModifier, ParserEvent.OpenBrace, ParserState.SubTransitionGroup, null),
            new Transition(ParserState.StateModifier, ParserEvent.OpenBrace, ParserState.SubTransitionGroup, null),
            new Transition(ParserState.SingleEvent, ParserEvent.Name, ParserState.SingleNextState, null),
            new Transition(ParserState.SingleEvent, ParserEvent.Dash, ParserState.SingleNextState, null),
            new Transition(ParserState.SingleNextState, ParserEvent.Name, ParserState.StateSpec, null),
            new Transition(ParserState.SingleNextState, ParserEvent.Dash, ParserState.StateSpec, null),
            new Transition(ParserState.SingleNextState, ParserEvent.OpenBrace, ParserState.SingleActionGroup, null),
            new Transition(ParserState.SingleActionGroup, ParserEvent.Name, ParserState.SingleActionGroupName, null),
            new Transition(ParserState.SingleActionGroup, ParserEvent.ClosedBrace, ParserState.StateSpec, null),
            new Transition(ParserState.SingleActionGroupName, ParserEvent.Name, ParserState.StateSpec, t => t.AddAction()),
            new Transition(ParserState.SingleActionGroupName, ParserEvent.ClosedBrace, ParserState.StateSpec, t => t.TransitionWithActions()),
            new Transition(ParserState.SubTransitionGroup, ParserEvent.ClosedBrace, ParserState.StateSpec, null),
            new Transition(ParserState.SubTransitionGroup, ParserEvent.Name, ParserState.GroupEvent, t => t.SetEvent()),
            new Transition(ParserState.SubTransitionGroup, ParserEvent.Dash, ParserState.GroupEvent, t => t.SetNullEvent()),
            new Transition(ParserState.GroupEvent, ParserEvent.Name, ParserState.GroupNextState, t => t.SetNextState()),
            new Transition(ParserState.GroupEvent, ParserEvent.Dash, ParserState.GroupNextState, t => t.SetNullNextState()),
            new Transition(ParserState.GroupNextState, ParserEvent.Name, ParserState.SubTransitionGroup, t => t.TransitionWithAction()),
            new Transition(ParserState.GroupNextState, ParserEvent.Dash, ParserState.SubTransitionGroup, t => t.TransitionNullAction()),
            new Transition(ParserState.GroupNextState, ParserEvent.OpenBrace, ParserState.GroupActionGroup, null),
            new Transition(ParserState.GroupActionGroup, ParserEvent.Name, ParserState.GroupActionGroupName, t => t.AddAction()),
            new Transition(ParserState.GroupActionGroup, ParserEvent.ClosedBrace, ParserState.SubTransitionGroup, t => t.TransitionNullAction()),
            new Transition(ParserState.GroupActionGroupName, ParserEvent.Name, ParserState.GroupActionGroupName, t => t.AddAction()),
            new Transition(ParserState.GroupActionGroupName, ParserEvent.ClosedBrace, ParserState.SubTransitionGroup, t => t.TransitionWithAction()),
            new Transition(ParserState.End, ParserEvent.Eof, ParserState.End, null),
        };
    }
}