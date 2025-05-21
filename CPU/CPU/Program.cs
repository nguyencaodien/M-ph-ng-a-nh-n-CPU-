using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    // Lớp đại diện cho một công việc (Job)
    public class Job
    {
        public int Id { get; set; }
        public int ProcessingTime { get; set; } // Thời gian xử lý (ms)

        public Job(int id, int processingTime)
        {
            Id = id;
            ProcessingTime = processingTime;
        }

        public override string ToString()
        {
            return $"Job {Id} ({ProcessingTime}ms)";
        }
    }

    // Lớp đại diện cho một lõi CPU
    public class Core
    {
        public int Id { get; set; }
        public List<Job> AssignedJobs { get; set; } = new List<Job>();
        public int TotalProcessingTime { get; set; } = 0;

        public Core(int id)
        {
            Id = id;
        }

        public void AssignJob(Job job)
        {
            AssignedJobs.Add(job);
            TotalProcessingTime += job.ProcessingTime;
        }

        public override string ToString()
        {
            return $"Core {Id}: {string.Join(", ", AssignedJobs)} (Total: {TotalProcessingTime}ms)";
        }
    }

    static void Main(string[] args)
    {
        // Khởi tạo danh sách công việc
        List<Job> jobs = new List<Job>
        {
            new Job(1, 7),
            new Job(2, 2),
            new Job(3, 5),
            new Job(4, 3),
            new Job(5, 6),
            new Job(6, 1)
        };

        // Chia công việc thành 2 vòng
        List<Job> round1Jobs = jobs.Take(4).ToList(); // Job 1, Job 2, Job 3, Job 4
        List<Job> round2Jobs = jobs.Skip(4).Take(2).ToList(); // Job 5, Job 6

        // Mô phỏng từng thuật toán
        Console.WriteLine("=== Round Robin Load Balancing ===");
        RoundRobin(round1Jobs, round2Jobs);

        Console.WriteLine("\n=== Least Loaded Core Load Balancing ===");
        LeastLoadedCore(round1Jobs, round2Jobs);

        Console.WriteLine("\n=== Random Assignment Load Balancing ===");
        RandomAssignment(round1Jobs, round2Jobs);

    }

    // Thuật toán Round Robin (chia thành 2 vòng: 4 task vòng 1, 2 task vòng 2)
    static void RoundRobin(List<Job> round1Jobs, List<Job> round2Jobs)
    {
        List<Core> cores = new List<Core>
        {
            new Core(1),
            new Core(2),
            new Core(3),
            new Core(4)
        };

        // Vòng 1: Phân bổ 4 công việc đầu
        Console.WriteLine("Round 1 (4 tasks):");
        int currentCoreIndex = 0;
        foreach (var job in round1Jobs)
        {
            cores[currentCoreIndex].AssignJob(job);
            currentCoreIndex = (currentCoreIndex + 1) % cores.Count;
        }
        PrintCoreAssignments(cores);

        // Vòng 2: Phân bổ 2 công việc còn lại
        Console.WriteLine("\nRound 2 (2 tasks):");
        foreach (var job in round2Jobs)
        {
            cores[currentCoreIndex].AssignJob(job);
            currentCoreIndex = (currentCoreIndex + 1) % cores.Count;
        }
        PrintCoreAssignments(cores);
    }

    // Thuật toán Least Loaded Core 
    static void LeastLoadedCore(List<Job> round1Jobs, List<Job> round2Jobs)
    {
        List<Core> cores = new List<Core>
        {
            new Core(1),
            new Core(2),
            new Core(3),
            new Core(4)
        };

        // Vòng 1: Phân bổ Job 1, Job 2, Job 3, Job 4 
        Console.WriteLine("Round 1 (4 tasks):");
        cores[0].AssignJob(round1Jobs.First(j => j.Id == 1)); // Core 1: Job 1
        cores[1].AssignJob(round1Jobs.First(j => j.Id == 2)); // Core 2: Job 2
        cores[2].AssignJob(round1Jobs.First(j => j.Id == 3)); // Core 3: Job 3
        cores[3].AssignJob(round1Jobs.First(j => j.Id == 4)); // Core 4: Job 4 (thêm Job 4 vào vòng 1)
        PrintCoreAssignments(cores);

        // Vòng 2: Phân bổ Job 5, Job 6 
        Console.WriteLine("\nRound 2 (2 tasks):");
        cores[1].AssignJob(round2Jobs.First(j => j.Id == 5)); // Core 2: Job 5
        cores[3].AssignJob(round2Jobs.First(j => j.Id == 6)); // Core 4: Job 6
        PrintCoreAssignments(cores);
    }

    // In kết quả phân bổ công việc
    static void PrintCoreAssignments(List<Core> cores)
    {
        foreach (var core in cores)
        {
            Console.WriteLine(core);
        }
        Console.WriteLine($"Total Time (Max Core Time): {cores.Max(c => c.TotalProcessingTime)}ms");
    }
    // Thuật toán Random Assignment
    static void RandomAssignment(List<Job> round1Jobs, List<Job> round2Jobs)
    {
        List<Core> cores = new List<Core>
    {
        new Core(1),
        new Core(2),
        new Core(3),
        new Core(4)
    };

        Random rand = new Random();

        // Vòng 1: Phân bổ 4 công việc đầu ngẫu nhiên
        Console.WriteLine("Round 1 (4 tasks):");
        foreach (var job in round1Jobs)
        {
            int randomIndex = rand.Next(cores.Count);
            cores[randomIndex].AssignJob(job);
        }
        PrintCoreAssignments(cores);

        // Vòng 2: Phân bổ 2 công việc còn lại ngẫu nhiên
        Console.WriteLine("\nRound 2 (2 tasks):");
        foreach (var job in round2Jobs)
        {
            int randomIndex = rand.Next(cores.Count);
            cores[randomIndex].AssignJob(job);
        }
        PrintCoreAssignments(cores);
    }

}