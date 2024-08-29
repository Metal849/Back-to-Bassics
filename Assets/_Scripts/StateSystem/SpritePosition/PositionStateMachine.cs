public partial class PositionStateMachine : StateMachine<PositionStateMachine, PositionStateMachine.PositionState, PositionStateInput>
{
    #region Unity Messages
    #endregion
    protected override void SetInitialState()
    { 
        CurrInput.PawnSprite = GetComponentInChildren<PawnSprite>();
        Transition<Center>();   
    }
}
