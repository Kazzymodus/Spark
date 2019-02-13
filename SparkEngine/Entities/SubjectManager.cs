namespace SparkEngine.Entities
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using SparkEngine.Level;

    public class SubjectManager
    {
        #region Properties

        private List<Subject> subjects = new List<Subject>();

        public List<Subject> AvailableWorkers
        {
            get { return subjects.FindAll(x => !x.HasTask); }
        }

        #endregion

        #region Methods

        internal void AddSubject(Subject subject)
        {
            subjects.Add(subject);
        }

        internal void UpdateSubjects(GameTime gameTime, Map map)
        {
            foreach (Subject subject in subjects)
            {
                subject.Update(gameTime, map);
                map.ObjectManager.ResetDrawOrderIndividual(subject, map);
            }
        }

        #endregion
    }
}
