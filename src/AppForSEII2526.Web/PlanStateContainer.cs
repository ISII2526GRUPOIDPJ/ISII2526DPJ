using AppForSEII2526.Web.API;

namespace AppForSEII2526.Web
{
    public class PlanStateContainer
    {
        public CreatePlanDTO Plan { get; private set; } = new CreatePlanDTO()
        {
            SelectedClasses = new List<ClassInPlanDTO>(),
        };

        // Calculates the estimated total price of the plan
        public decimal EstimatedTotalPrice
        {
            get
            {
                return Plan.SelectedClasses.Sum(sc => Convert.ToDecimal(sc.Price));
            }
        }

        public event Action? OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();

        // Adds a class to the plan
        public void AddClassToPlan(ClassInPlanDTO clase)
        {
            if (!Plan.SelectedClasses.Any(sc => sc.Id == clase.Id))
                Plan.SelectedClasses.Add(clase);

            NotifyStateChanged();
        }

        // Removes a class from the plan
        public void RemoveClassFromPlan(ClassInPlanDTO clase)
        {
            Plan.SelectedClasses.Remove(clase);
            NotifyStateChanged();
        }

        // Clears all classes from the plan
        public void ClearPlan()
        {
            Plan.SelectedClasses.Clear();
            NotifyStateChanged();
        }

        // Resets the plan after it has been processed
        public void PlanProcessed()
        {
            Plan = new CreatePlanDTO()
            {
                SelectedClasses = new List<ClassInPlanDTO>()
            };
            NotifyStateChanged();
        }

    }
}
