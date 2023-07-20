#region License

/*
 * All content copyright Marko Lahma, unless otherwise indicated. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy
 * of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 *
 */

#endregion

namespace Quartz.Examples.Example06;

/// <summary>
/// A job dumb job that will throw a job execution exception.
/// </summary>
/// <author>Bill Kratzer</author>
/// <author>Marko Lahma (.NET)</author>
[PersistJobDataAfterExecution]
[DisallowConcurrentExecution]
public class BadJob2 : IJob
{
    /// <summary>
    /// Called by the <see cref="IScheduler" /> when a <see cref="ITrigger" />
    /// fires that is associated with the <see cref="IJob" />.
    /// <para>
    /// The implementation may wish to set a  result object on the
    /// JobExecutionContext before this method exits.  The result itself
    /// is meaningless to Quartz, but may be informative to
    /// <see cref="IJobListener" />s or
    /// <see cref="ITriggerListener" />s that are watching the job's
    /// execution.
    /// </para>
    /// </summary>
    /// <param name="context">Execution context.</param>
    public virtual ValueTask Execute(IJobExecutionContext context)
    {
        JobKey jobKey = context.JobDetail.Key;
        Console.WriteLine("---{0} executing at {1:r}", jobKey, DateTime.Now);

        // a contrived example of an exception that
        // will be generated by this job due to a
        // divide by zero error
        try
        {
            int zero = 0;
            int calculation = 4815 / zero;
        }
        catch (Exception e)
        {
            Console.WriteLine("--- Error in job!");
            JobExecutionException e2 = new JobExecutionException(e);
            // Quartz will automatically unschedule
            // all triggers associated with this job
            // so that it does not run again
            e2.UnscheduleAllTriggers = true;
            throw e2;
        }

        Console.WriteLine("---{0} completed at {1:r}", jobKey, DateTime.Now);
        return default;
    }
}