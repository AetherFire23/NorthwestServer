namespace IntegrationTests;


// Partial classes need to be in the same namespace obv
public partial class GameTaskAvailabilityResult
{
    public override string ToString()
    {
        return this.GameTaskName;
    }

}
