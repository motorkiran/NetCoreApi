public static class UserQuery
{
    public static string GetUserByEmail = "SELECT * FROM " + Constants.DbSchema + ".user WHERE email = '{0}'";
}