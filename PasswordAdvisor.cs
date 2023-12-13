using Arowolo_Project_2.Enums;
using System.Text.RegularExpressions;

public class PasswordAdvisor
{
    public static PasswordScore CheckStrength(string password)
    {
        int score = 0;

        if (password.Length < 1)
            return PasswordScore.Blank;
        if (password.Length < 2)
            return PasswordScore.VeryWeak;

        if (password.Length >= 3)
            //return PasswordScore.Strong;
            score += 2;
        if (password.Length >= 5)
            score += 2;
        Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$");
        if (validateGuidRegex.IsMatch(password))
        {
            score += 1;
        }

        return (PasswordScore)score;
    }
}