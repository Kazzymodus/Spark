using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SparkEngine.Systems.Tasks
{
    public class TaskManager
    {
        public TaskManager(Action<UpdateInfo, int, int>[] taskMethods, int maxTasks)
        {
            Tasks = new SystemTask[maxTasks];
            TaskMethods = taskMethods;
        }

        public SystemTask[] Tasks { get; }

        private Action<UpdateInfo, int, int>[] TaskMethods { get; }

        public int TaskAmount { get; private set; }

        public void ScheduleTask(int task, int source, int target, UpdateInfo updateInfo)
        {
            Tasks[TaskAmount++] = new SystemTask(task, source, target, updateInfo);
        }

        public void ExecuteTasks()
        {
            for (int i = 0; i < TaskAmount; i++)
            {
                SystemTask currentTask = Tasks[i];

                TaskMethods[currentTask.task].Invoke(currentTask.updateInfo, currentTask.source, currentTask.target);
            }
        }

        internal void ResetTasks()
        {
            TaskAmount = 0;
        }

    }
}
