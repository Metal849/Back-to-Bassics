using System.Collections.Generic;
using System;
/// <summary>
/// This Type of statemachine calls state update on the Beat Metric system!
/// </summary>
public abstract class StateMachine
{
    private IStateMachineContext _context;
    private State _currState;
    private Dictionary<Type, State> _stateMap;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public StateMachine(IStateMachineContext context)
    {
        _context = context;
        _stateMap = new Dictionary<Type, State>();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    public void TransitionToState(State state)
    {
        _currState.ExitState();
        _currState = state;
        _currState.EnterState();
    }
}
