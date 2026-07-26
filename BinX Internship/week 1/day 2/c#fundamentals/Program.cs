


using System.Data;  

ShowValueAndRefernceTypes();
Console.WriteLine();

DemonstrateCopyBehavior();
Console.WriteLine();

RunGradeClassifierExamples();
Console.WriteLine();

ReadUserNameSafely();

static void ShowValueAndRefernceTypes()
{
    Console.WriteLine("value types vs Refernce types ");

    int age =21;
    bool isEnrolled = true;
    double gpa = 3.75;


    string fullName = "salah badarneh";
    int[] scores = { 85, 90, 78 };
    var course = new Course("Backend Development");

    Console.WriteLine($"{nameof(age)}= {age} ({age.GetType()}");
    Console.WriteLine($"{nameof(isEnrolled)}= {isEnrolled} ({isEnrolled.GetType()})");
    Console.WriteLine($"{nameof(gpa)}= {gpa} ({gpa.GetType()})");

    Console.WriteLine($"{nameof(fullName)}= {fullName} ({fullName.GetType()})");
    Console.WriteLine($"{nameof(scores)}= [{string.Join(", ",scores)}]({scores.GetType()})");
    Console.WriteLine($"{nameof(course)}= {course} ({course.GetType()})");


}

static void DemonstrateCopyBehavior()
{
    Console.WriteLine("copy behavior");


    int original=10;
    int copy= original;
    copy =99;
    Console.WriteLine($"Value type -> original.Title: {original}, copy: {copy} ");

    var  originalCourse= new Course("Intro to c#");
    var sameCourse= originalCourse;
    sameCourse.Title = "Intro to c# (Updated)";
    Console.WriteLine($"Refernce type -> original.Title: {originalCourse.Title}, sameCourse.Title: {sameCourse.Title}");
}


static void RunGradeClassifierExamples()
{
    Console.WriteLine(" Grade classifier");

    int[] sampleScores= {95 , 72, 58, 30};
    foreach(var score in sampleScores)
    {
        Console.WriteLine($"Score {score} -> {ClassifyGrade(score)}");
    }
}

static string ClassifyGrade(int score) => score switch
{
    >= 90 => "Excellent",
    >= 70 => "proficient",
    >= 50 => "Developing",
    _ =>"Below Standard"

};

static void ReadUserNameSafely()
{
    Console.WriteLine("safe input handling");
    Console.WriteLine("Enter your name:");

    string? input= Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        Console.WriteLine("No name entered ,using defult value");
        input = "Guest";

    }

    Console.WriteLine($"Welcome, {input}!");

}

class Course
{
    public string Title {get; set;}
    public Course(string title)
    {
        Title=title;
    }
}


