public static class ActionQuery
{
    public static string GetUserActionList = "SELECT * FROM " + Constants.DbSchema + ".user_action WHERE action_id = {0} AND user_id = {1}";
}