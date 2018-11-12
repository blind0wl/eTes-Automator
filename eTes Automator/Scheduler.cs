using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace eTes_Automator
{
    public class Scheduler
    {
        public async void Start()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();            
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<UpdateTSJob>()
                .WithIdentity("name", "group")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateTSJob ", "Timesheet")
                .WithCronSchedule("0 52,52 21 ? * MON-FRI")                
                .StartAt(DateTime.Now)
                .WithPriority(1)
                .Build();
            await scheduler.ScheduleJob(job, trigger);

            //await scheduler.Shutdown();
        }
    }

    public class UpdateTSJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await TimeSheet.StartTimesheet();
        }
    }
}
