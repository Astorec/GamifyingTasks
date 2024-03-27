using GamifyingTasks.Classes;
using GamifyingTasks.Firebase.DB;
using GamifyingTasks.Firebase.DB.Interfaces;
namespace GamifyingTasks.Data
{
    internal static class UserLevelCaluclations
    {
        private static int expCurve = 150; // The amount the EXP require for level up is increased by
        private static float progress; // The progress of the user to the next level
        private static Users currentUser; // The current user
        public static bool hasleveledUp = false; // If the user has leveled up

        /// <summary>
        /// Calculate the EXP of the user
        /// </summary>
        /// <param name="difficulty"></param>
        private static void CalculateEXP(string difficulty)
        {
            // Calculate the EXP based on the difficulty of the task
            switch (difficulty)
            {
                case "Easy":
                    currentUser.Exp = currentUser.Exp + 25;
                    break;
                case "Medium":
                   currentUser.Exp = currentUser.Exp + 50;
                    break;
                case "Hard":
                    currentUser.Exp = currentUser.Exp + 75;
                    break;
                case "Extra Hard":
                   currentUser.Exp = currentUser.Exp + 100;
                    break;
            }

            // if the user has enough EXP to level up, level up the user and reset the EXP
            if (currentUser.Exp >= currentUser.requiredExp)
            {

                currentUser.Level = currentUser.Level + 1;
                currentUser.requiredExp = currentUser.requiredExp + 150;
                currentUser.Exp = 0;
                progress = 0;
                hasleveledUp = true;

            }
        }


        /// <summary>
        /// Calculate the progress of the user
        /// </summary>
        /// <param name="_users"></param>
        /// <returns></returns>
        public static float CalculateProgress(IUsers _users)
        {  
            // create a copy of the user
            var temp = _users.GetUser();

            // if the user has not leveled up, calculate the progress
            if (temp.Exp < temp.requiredExp)
            {              
                // Calculate accurate progress fraction
                float fraction = Math.Min(temp.Exp / (float)temp.requiredExp, 1f);

                progress = (float)Math.Round(fraction * 100, 0); // Round to the nearest whole number after calculation
            }
            
            // return the progress
            return progress;
        }


        /// <summary>
        /// TaskCompleted is called when a task is completed
        /// </summary>
        /// <param name="completedTask"></param>
        /// <param name="_tasks"></param>
        /// <param name="_users"></param>
        /// <returns></returns>
        public static async Task TaskCompleted(Tasks completedTask, ITasks _tasks, IUsers _users)
        {
            // set the current user to a copy of the user
            currentUser = _users.GetUser();

            // Calculate the EXP of the user
            CalculateEXP(completedTask.Difficulty);
            // Mark the task as completed
            completedTask.Completed = true;

            // Update the task and the user
            await _tasks.UpdateTask(completedTask);
            await _users.UpdateUser(currentUser);
        }

    }
}