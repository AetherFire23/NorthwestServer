using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WebAPI.Conventions;

public class LowercaseControllerModelConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        // Convert the controller's route template to lowercase// Convert the controller's route template to lowercase
        for (int i = 0; i < controller.Selectors.Count; i++)
        {
            SelectorModel selector = controller.Selectors[i];
            selector.AttributeRouteModel.Template = selector.AttributeRouteModel.Template.ToLower();
        }

        for (int i = 0; i < controller.Actions.Count; i++)
        {
            ActionModel action = controller.Actions[i];
            action.ActionName = action.ActionName.ToLower();
        }
    }
}
