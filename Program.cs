using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Write("Enter Number of Working Days (1-7): ");
        int workingDays = GetValidInput(1, 7);

        Console.Write("Enter Number of Subjects Per Day (1-8): ");
        int subjectsPerDay = GetValidInput(1, 8);

        Console.Write("Enter Total Subjects: ");
        int totalSubjects = GetValidInput(1, int.MaxValue);

        int totalHoursForWeek = workingDays * subjectsPerDay;
        Console.WriteLine($"Total hours for the week: {totalHoursForWeek}\n");

        var subjects = new List<SubjectAllocation>();
        int remainingHours = totalHoursForWeek;

        for (int i = 0; i < totalSubjects; i++)
        {
            Console.Write($"Enter Subject {i + 1} Name: ");
            string subjectName = Console.ReadLine();

            Console.Write($"Enter hours for {subjectName} (Remaining: {remainingHours}): ");
            int subjectHours = GetValidInput(1, remainingHours);
            remainingHours -= subjectHours;

            subjects.Add(new SubjectAllocation { Subject = subjectName, Hours = subjectHours });
        }

        Console.WriteLine("\nGenerating Timetable...\n");
        GenerateTimetable(subjects, workingDays, subjectsPerDay);
    }

    static int GetValidInput(int min, int max)
    {
        int value;
        while (!int.TryParse(Console.ReadLine(), out value) || value < min || value > max)
        {
            Console.Write($"Invalid input. Enter a value between {min} and {max}: ");
        }
        return value;
    }

    static void GenerateTimetable(List<SubjectAllocation> subjects, int workingDays, int subjectsPerDay)
    {
        var subjectQueue = new Queue<SubjectAllocation>(subjects.OrderByDescending(s => s.Hours));
        string[,] timetable = new string[subjectsPerDay, workingDays];

        for (int i = 0; i < subjectsPerDay; i++)
        {
            for (int j = 0; j < workingDays; j++)
            {
                if (subjectQueue.Count > 0)
                {
                    var subject = subjectQueue.Dequeue();
                    timetable[i, j] = subject.Subject;
                    subject.Hours--;
                    if (subject.Hours > 0)
                        subjectQueue.Enqueue(subject);
                }
            }
        }

        Console.WriteLine("Generated Timetable:");
        for (int i = 0; i < subjectsPerDay; i++)
        {
            for (int j = 0; j < workingDays; j++)
            {
                Console.Write($"{timetable[i, j],-10} ");
            }
            Console.WriteLine();
        }
    }
}

public class SubjectAllocation
{
    public string Subject { get; set; }
    public int Hours { get; set; }
}
