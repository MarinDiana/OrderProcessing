namespace OrderProcessing.Api.States;

public class InvalidOrderTransitionException : Exception
{
    public InvalidOrderTransitionException(string currentState, string action)
        : base($"Invalid transition: cannot execute '{action}' from state '{currentState}'.")
    {
    }
}