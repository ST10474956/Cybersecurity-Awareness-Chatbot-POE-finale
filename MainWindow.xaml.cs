using System.Windows;

public partial class MainWindow : Window
{
    ChatbotEngine bot = new ChatbotEngine();
    QuizManager quiz = new QuizManager();
    int current = 0;
    int score = 0;

    public MainWindow()
    {
        InitializeComponent();
    }

    private void Send_Click(object sender, RoutedEventArgs e)
    {
        var result = bot.ProcessInput(UserInput.Text);

        if(result == "QUIZ")
        {
            StartQuiz();
        }
        else if(result == "LOG")
        {
            ShowLog();
        }
        else
        {
            ChatBox.Text += "Command not recognised\n";
        }
    }

    void StartQuiz()
    {
        current = 0;
        score = 0;
        ShowQuestion();
        ActivityLogger.Log("Quiz started");
    }

    void ShowQuestion()
    {
        var q = quiz.Questions[current];
        QuestionText.Text = q.Text;
        QuizProgress.Value = (double)current / quiz.Questions.Count * 100;
    }

    void Answer(int index)
    {
        if(index == quiz.Questions[current].CorrectIndex)
        {
            score++;
            ChatBox.Text += "Correct!\n";
        }
        else
        {
            ChatBox.Text += "Wrong!\n";
        }

        current++;

        if(current < quiz.Questions.Count)
            ShowQuestion();
        else
        {
            ScoreText.Text = "Final Score: " + score;
            ActivityLogger.Log("Quiz finished score: " + score);
        }
    }

    private void AnswerA(object s, RoutedEventArgs e){ Answer(0); }
    private void AnswerB(object s, RoutedEventArgs e){ Answer(1); }
    private void AnswerC(object s, RoutedEventArgs e){ Answer(2); }
    private void AnswerD(object s, RoutedEventArgs e){ Answer(3); }

    void ShowLog()
    {
        var logs = ActivityLogger.GetRecent(10);
        foreach(var log in logs)
            ChatBox.Text += log + "\n";
    }
}
