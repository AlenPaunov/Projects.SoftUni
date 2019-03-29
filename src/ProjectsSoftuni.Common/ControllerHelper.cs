namespace ProjectsSoftuni.Common
{
    public static class ControllerHelper
    {
        public static string RemoveControllerFromStr(string controllerName)
        {
            string result = controllerName.Replace("Controller", null);

            return result;
        }
    }
}
