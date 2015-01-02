
using System;

// assume for simplicity that the ball weights are integers

namespace BallProblem
{
    class Program
    {
        static void Main()
        {
            try
            {
                const int numberBalls = 11;
                const int uniqueBallIndex = 9;

                // run single ball problem
                int ballIndex = RunBallProblem(numberBalls, uniqueBallIndex);
                Console.WriteLine("Unique ballindex = {0}", ballIndex);

                // test all ball problems with up to 300 balls
                const int numberballsToTest = 300;
                TestBallProblem(numberballsToTest);
                Console.WriteLine("Testing completed.");
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }

            Console.WriteLine("Press return to exit.");
            Console.Read();
        }

        static int RunBallProblem(int numberBalls, int uniqueBallIndex)
        {
            BallProblem ballProblem = new BallProblem();

            // setup a valid ball collection
            ballProblem.SetupBalls(numberBalls, uniqueBallIndex);

            // verify that the ball collection is valid 
            // this validation step is optional since SetupBalls() ensures a valid ball collection
            if (!ballProblem.ValidateBallSetup())
            {
                string message = string.Format("Error: The ball setup does not have exactly one ball with a unique weight.");
                throw new Exception(message);
            }

            return BallProblem.FindUniqueBall(ballProblem.Balls);
        }

        static void TestBallProblem(int numberBallsToTest)
        {
            // tests all ball problems with from 3 to numberBallsToTest balls

            for (int numberBalls = 3; numberBalls < numberBallsToTest; numberBalls++)
            {
                for (int ballIndex = 0; ballIndex < numberBalls - 1; ballIndex++)
                {
                    int ballIndexFound = RunBallProblem(numberBalls, ballIndex);

                    if (ballIndexFound != ballIndex)
                    {
                        const string formatString = "Ball problem failed for number ball = {0} and ball index = {1}. Actual Index = {2}";
                        string message = string.Format(formatString, numberBalls, ballIndex, ballIndexFound);
                        throw new Exception(message);
                    }
                }
            }
        }
    }
}
