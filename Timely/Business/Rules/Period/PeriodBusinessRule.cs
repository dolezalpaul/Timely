using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moravia.Timely.Models;

namespace Moravia.Timely.Business.Rules
{
    public class PeriodBusinessRule : BusinessRule<Period>
    {
        public Service<Activity> activityService {get; set;}

        public override void Apply(Period entity)
        {
            Activity activity = activityService.Get(entity.activity_id);
            decimal adjuctedDifference = activity.adjusted_time - activity.spent_time;

            if (this.GetEntityState(entity) == System.Data.Entity.EntityState.Deleted)
            {
                activity.spent_time = (decimal)activity.periods.Where(p => p.closed == true && p.id != entity.id).Sum(p => (p.end.Value - p.start).TotalHours);
            }
            else
            {
                activity.spent_time = (decimal)activity.periods.Where(p => p.closed == true).Sum(p => (p.end.Value - p.start).TotalHours);
            }

            activity.adjusted_time = activity.spent_time + adjuctedDifference;
            activityService.Post(activity);
        }
    }
}