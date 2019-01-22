using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

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
                .WithIdentity("updateTS" , "group1")
                .Build();
            
            ITrigger triggerTS = TriggerBuilder.Create()
                .WithIdentity("UpdateTS_Trigger", "group1")
                .WithCronSchedule("0 0 14 ? * WED,FRI *") //Every Wed and Fri at 2pm        
                .StartAt(DateTime.Now)
                .WithPriority(1)
                .Build();

            await scheduler.ScheduleJob(job, triggerTS);
            
            //await scheduler.Shutdown();
        }
        public async void SFStart()
        {
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.Start();

            IJobDetail job2 = JobBuilder.Create<SalesForceAL>()
                .WithIdentity("loginSF", "group2")
                .Build();

            ITrigger triggerSF = TriggerBuilder.Create()
                .WithIdentity("SF", "group2")
                .WithCronSchedule("0 0 12 ? * MON *") //Every Monday at 12pm
                .StartAt(DateTime.Now)
                .WithPriority(1)
                .Build();

            await scheduler.ScheduleJob(job2, triggerSF);

            //await scheduler.Shutdown();
        }
        public async void InterruptSF()
        {
            IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            await scheduler.UnscheduleJob(new TriggerKey("SF", "group2"));
            await scheduler.DeleteJob(new JobKey("SF", "group2"));
            await scheduler.Clear();
        }
    }

    public class UpdateTSJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await TimeSheet.StartTimesheet();
        }
    }
    public class SalesForceAL : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await SalesForce.Automate();
        }
    }
    
}
