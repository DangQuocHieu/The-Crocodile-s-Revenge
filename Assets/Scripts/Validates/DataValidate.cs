using System.Text.RegularExpressions;
using UnityEngine;

public static class DataValidate 
{
    public static bool IsEmpty(string field, string name)
    {
        if(string.IsNullOrEmpty(field))
        {
            Observer.Notify(GameEvent.OnPlayFabError, name + " can't be empty !");
            return true;
        }
        return false;
    }

    public static bool IsTooShort(string field, string name)
    {
        if(field.Length < 6)
        {
            Observer.Notify(GameEvent.OnPlayFabError, name + " must contain at least 6 characters.");
            return true;
        }
        return false;
    }

    public static bool IsContainCharacterAndNumber(string field, string name)
    {
        Regex regex = new Regex(@"^[A-Za-z][A-Za-z0-9]*$");

        // Kiểm tra username có chứa cả chữ và số không
        bool containsLetter = Regex.IsMatch(field, "[A-Za-z]");
        bool containsDigit = Regex.IsMatch(field, "[0-9]");

        // Kiểm tra tổng thể
        if(regex.IsMatch(field) && containsLetter && containsDigit)
        {
            return true;
        }
        Observer.Notify(GameEvent.OnPlayFabError,name + " must start with a letter and contain both characters and numbers !");
        return false;
    }
}
