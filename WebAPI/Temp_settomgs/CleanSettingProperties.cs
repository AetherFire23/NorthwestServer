namespace WebAPI.Temp_settomgs
{
    public class CleanSettingProperties : TaskBase
    {
        public CleanSettingProperties()
        {
            this.ActionCost = 1;
        }
        public int HygieneGained { get; set; } = 2;
    }
}
