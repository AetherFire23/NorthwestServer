using System.Runtime.CompilerServices;

namespace WebAPI.Temp_settomgs
{
    public class CookSettingProperties : TaskBase
    {
        public int FoodCreated { get; set; } = 1;
        public CookSettingProperties()
        {
            this.ActionCost = 1;
        }
    }
}
