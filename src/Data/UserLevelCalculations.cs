using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB;
namespace GamifyingTasks.Data
{
    internal static class UserLevelCaluclations
    {
        private static int expCurve = 150; // The amount the EXP require for level up is increased by

        private static void CalculateEXP(string difficulty)
        {
            switch (difficulty)
            {
                case "Easy":
                    DBCore.CurrentLocalUser.Exp = DBCore.CurrentLocalUser.Exp + 25;
                    break;
                case "Medium":
                    DBCore.CurrentLocalUser.Exp = DBCore.CurrentLocalUser.Exp + 50;
                    break;
                case "Hard":
                    DBCore.CurrentLocalUser.Exp = DBCore.CurrentLocalUser.Exp + 75;
                    break;
                case "Extra Hard":
                    DBCore.CurrentLocalUser.Exp = DBCore.CurrentLocalUser.Exp + 100;
                    break;
            }

            if (DBCore.CurrentLocalUser.Exp >= DBCore.CurrentLocalUser.requiredExp){

                DBCore.CurrentLocalUser.Level = DBCore.CurrentLocalUser.Level + 1;
                DBCore.CurrentLocalUser.requiredExp = DBCore.CurrentLocalUser.requiredExp + 150;
                DBCore.CurrentLocalUser.Exp = 0;
            }
        }

        public static async Task TaskCompleted(Tasks completedTask)
        {
            CalculateEXP(completedTask.Difficulty);
            await DBCore.UpdateTask(completedTask);
            await DBCore.UpdateUser(DBCore.CurrentLocalUser);
        }

    }
}