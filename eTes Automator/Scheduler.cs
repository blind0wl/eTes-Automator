using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace eTes_Automator
{
    public class Scheduler
    {
        //public async void Start()
        //{
        //    ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
        //    IScheduler scheduler = await schedulerFactory.GetScheduler();
        //    await scheduler.Start();

        //    IJobDetail job = JobBuilder.Create<UpdateTSJob>().Build();

        //    ITrigger trigger = TriggerBuilder.Create()
        //        .WithIdentity("UpdateTSJob ", "GreetingGroup")
        //        .WithCronSchedule("0 0/1 * 1/1 * ? *")
        //        .StartAt(DateTime.UtcNow)
        //        .WithPriority(1)
        //        .Build();
        //    await scheduler.ScheduleJob(job, trigger);
        //}
    }

    //public class UpdateTSJob : IJob
    //{
    //    Task IJob.Execute(IJobExecutionContext context)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException(nameof(context));
    //        }
    //        //MainWindow.AppWindow.btn_start_Click(sender, args);
    //        //Task taskA = new Task(() => Console.WriteLine("Hello from task at {0}", DateTime.Now.ToString()));
    //        //taskA.Start();
    //        //return taskA;
    //    }
    //}
}
