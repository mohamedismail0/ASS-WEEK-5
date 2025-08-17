using System;
using System.Collections.Generic;

class Question
{
    public Question(string header, string body, List<string> choices, List<int> correctAnswers, double mark)
    {
        Header = header;
        Body = body;
        Choices = choices;
        CorrectAnswers = correctAnswers;
        Mark = mark;
    }

    public string Header { get; set; }
    public string Body { get; set; }
    public List<string> Choices { get; set; }
    public List<int> CorrectAnswers { get; set; }
    public double Mark { get; set; }

    public void AddChoice(string choice)
    {
        Choices.Add(choice);
    }
    public void AddCorrectAnswer(int index)
    {
        CorrectAnswers.Add(index);
    }

    public bool CheckAnswer(List<int> studentAnswers)
    {
        if (studentAnswers.Count != CorrectAnswers.Count)
            return false;

        studentAnswers.Sort();
        CorrectAnswers.Sort();

        for (int i = 0; i < CorrectAnswers.Count; i++)
        {
            if (CorrectAnswers[i] != studentAnswers[i])
                return false;
        }
        return true;
    }
}

class Exam
{
    public List<Question> Questions { get; set; } = new List<Question>();

    public Exam(List<Question> questions)
    {
        Questions = questions;
    }

    public void DoctorMode()
    {
        Console.WriteLine("====== Doctor Mode ======");
        string type;
        do
        {
            Console.Write("Enter question type (True/False, MCQ, ChooseAll) or 'q' to quit: ");
            type = Console.ReadLine();

            if (type.ToLower() != "q")
            {
                Console.WriteLine("Enter the question body: ");
                string Body = Console.ReadLine();

                Console.WriteLine("Enter the mark of the question: ");
                double Mark = Convert.ToDouble(Console.ReadLine());

                Question q = new Question(type, Body, new List<string>(), new List<int>(), Mark);

                Console.WriteLine("How many choices? ");
                int numChoices = Convert.ToInt32(Console.ReadLine());

                for (int i = 1; i <= numChoices; i++)
                {
                    Console.Write($"Choice {i}: ");
                    q.Choices.Add(Console.ReadLine());
                }

                Console.WriteLine("Enter the correct choice numbers separated by comma (e.g., 1,3): ");
                string[] correctAnswers = Console.ReadLine().Split(',');

                foreach (string answer in correctAnswers)
                {
                    if (int.TryParse(answer.Trim(), out int index) && index > 0 && index <= numChoices)
                    {
                        q.AddCorrectAnswer(index - 1);
                    }
                    else
                    {
                        Console.WriteLine($"Invalid choice index: {answer}. Please enter a valid index between 1 and {numChoices}.");
                    }
                }

                Questions.Add(q);
                Console.WriteLine("Question added successfully \n");
            }

        } while (type.ToLower() != "q");
    }

    public void StudentMode()
    {
        Console.WriteLine("====== Student Mode ======");
        double totalScore = 0;
        double studentScore = 0;

        for (int i = 0; i < Questions.Count; i++)
        {
            Question q = Questions[i];
            totalScore += q.Mark;

            Console.WriteLine($"\nQuestion {i + 1} ({q.Mark} mark) - {q.Header}");
            Console.WriteLine(q.Body);

            for (int j = 0; j < q.Choices.Count; j++)
            {
                Console.WriteLine($"{j + 1}. {q.Choices[j]}");
            }

            Console.Write("Your answer(s) (comma-separated for multiple choices): ");
            string[] answers = Console.ReadLine().Split(',');
            List<int> studentAnswers = new List<int>();

            foreach (string answer in answers)
            {
                if (int.TryParse(answer.Trim(), out int index) && index > 0 && index <= q.Choices.Count)
                {
                    studentAnswers.Add(index - 1);
                }
                else
                {
                    Console.WriteLine($"Invalid choice index: {answer}. Please enter a valid index between 1 and {q.Choices.Count}.");
                }
            }

            if (q.CheckAnswer(studentAnswers))
            {
                Console.WriteLine("Correct \n");
                studentScore += q.Mark;
            }
            else
            {
                Console.WriteLine("Incorrect \n");
            }
        }

        Console.WriteLine($"Exam finished! Your score : {studentScore}/{totalScore}");
    }
}

class Program
{
    static void Main()
    {
        Exam exam = new Exam(new List<Question>());

        exam.DoctorMode();

        Console.WriteLine("\nNow switching to Student Mode...");
        exam.StudentMode();
    }
}

















