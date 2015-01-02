
using System;
using System.Collections.Generic;
using System.Linq;

namespace BallProblem
{
    public class BallProblem
    {
        public BallProblem()
        {
            TypicalBallWeight = 1;
            UniqueBallWeight = 2;
        }

        private int TypicalBallWeight { get; set; }

        private int UniqueBallWeight { get; set; }

        public List<int> Balls { private set; get;}

        /// <summary>
        /// SetupBalls set up a ball configuration containing exactly one ball with a unique weight
        /// </summary>
        /// <param name="numberBalls">Number of Balls</param>
        /// <param name="uniqueBallIndex">Unique Ball Index</param>
        public void SetupBalls(int numberBalls, int uniqueBallIndex)
        {
            // check that there are at least 3 balls
            if (numberBalls < 3)
            {
                string message = string.Format("Error: There must be at least 3 balls, currently there are {0}.", numberBalls);
                throw new ArgumentException(message);
            }

            // check that the ball index is within the range of the list size
            if (uniqueBallIndex < 0 || uniqueBallIndex > numberBalls - 1)
            {
                string message = string.Format("Error: The unique ball index is must be from 0 to {0}.", numberBalls - 1);
                throw new ArgumentException(message);
            }

            Balls = new List<int>(numberBalls);

            for (int n = 0; n < numberBalls; n++)
            {
                Balls.Add(TypicalBallWeight);
            }

            Balls[uniqueBallIndex] = UniqueBallWeight;
        }

        /// <summary>
        /// ValidateBallSetup returns true if exactly one ball has a unique weight, and false otherwise
        /// </summary>
        /// <returns></returns>
        public bool ValidateBallSetup()
        {
            int maximumBallWeight = Balls.Max();
            int minimumBallWeight = Balls.Min();
            int numberballs = Balls.Count();

            // count the number of minimum and maximum weight balls
            int numberMaximumWeightballs = Balls.Count(n => (n == maximumBallWeight));
            int numberMinimumWeightballs = Balls.Count(n => (n == minimumBallWeight));

            // check that there is exactly 1 minimum weight ball and rest are maximum weight (or vice versa)
            bool option1 = (numberMinimumWeightballs == 1) && (numberMaximumWeightballs == numberballs - 1);
            bool option2 = (numberMaximumWeightballs == 1) && (numberMinimumWeightballs == numberballs - 1);

            return (option1 || option2);
        }

        /// <summary>
        /// FindUniqueBall returns the index of the unique ball.
        /// 
        /// The algorithm divides the ball collection into three equal groups plus a remainder.
        /// The remainder contains 1 or 2 balls. The groups are compared and the one with the different
        /// weight contains the unique ball. If the three groups are equal weight then the unique ball is
        /// in the remainder group. If the unique ball is in a group then the function is called recursively.
        /// The recursion ends when groups size is less than 3.
        /// </summary>
        /// <param name="balls">List of Balls</param>
        /// <returns>Index of Unique Ball</returns>
        public static int FindUniqueBall(List<int> balls)
        {
            int numberBalls = balls.Count();
            int ballGroupSize = numberBalls / 3;
            int ballsRemaining = numberBalls % 3;

            // divide the balls into 3 equal size groups plus remaining balls
            List<int> ballGroup1 = balls.Take(ballGroupSize).ToList();
            List<int> ballGroup2 = balls.Skip(ballGroupSize).Take(ballGroupSize).ToList();
            List<int> ballGroup3 = balls.Skip(ballGroupSize * 2).Take(ballGroupSize).ToList();
            List<int> remainingBallGroup = balls.Skip(ballGroupSize * 3).Take(ballsRemaining).ToList();

            // weight each group
            int ballGroup1Weight = ballGroup1.Sum();
            int ballGroup2Weight = ballGroup2.Sum();
            int ballGroup3Weight = ballGroup3.Sum();

            bool groups1And2AreSameWeight = (ballGroup1Weight == ballGroup2Weight);
            bool groups1And3AreSameWeight = (ballGroup1Weight == ballGroup3Weight);
            bool groups2And3AreSameWeight = (ballGroup2Weight == ballGroup3Weight);

            // determine which group has the unique ball
            bool group1HasUniqueBall = !groups1And2AreSameWeight && !groups1And3AreSameWeight && groups2And3AreSameWeight;
            bool group2HasUniqueBall = !groups1And2AreSameWeight && !groups2And3AreSameWeight && groups1And3AreSameWeight;
            bool group3HasUniqueBall = !groups1And3AreSameWeight && !groups2And3AreSameWeight && groups1And2AreSameWeight;

            // end the recursion if the ball groups size is less than 3
            if (ballGroupSize == 1)
            {
                if (group1HasUniqueBall)
                {
                    return 0;
                }

                if (group2HasUniqueBall)
                {
                    return 1;
                }

                if (group3HasUniqueBall)
                {
                    return 2;
                }

                if (remainingBallGroup.Count() == 1)
                {
                    return 3;
                }
                
                if (remainingBallGroup.Count() == 2)
                {
                    return (balls[0] == remainingBallGroup[0]) ? 4 : 3;
                }
            }
            else if (ballGroupSize == 2)
            {
                if (group1HasUniqueBall)
                {
                    return (balls[2] == ballGroup1[0]) ? 1 : 0;
                }

                if (group2HasUniqueBall)
                {
                    return (balls[0] == ballGroup2[0]) ? 3 : 2;
                }

                if (group3HasUniqueBall)
                {
                    return (balls[0] == ballGroup3[0]) ? 5 : 4;
                }

                if (remainingBallGroup.Count() == 1)
                {
                    return 6;
                }

                if (remainingBallGroup.Count() == 2)
                {
                    return (balls[0] == remainingBallGroup[0]) ? 7 : 6;
                }
            }

            // recursively call FindUniqueBall()
            if (group1HasUniqueBall)
            {
                return FindUniqueBall(ballGroup1);
            }

            if (group2HasUniqueBall)
            {
                return (FindUniqueBall(ballGroup2) + ballGroupSize);
            }

            if (group3HasUniqueBall)
            {
                return (FindUniqueBall(ballGroup3) + ballGroupSize * 2);
            }

            // else find unique ball in remaining group
            if (remainingBallGroup.Count() == 1)
            {
                return ballGroupSize * 3;
            }

            // remainingBallGroup.Count() == 2
            return (balls[0] == remainingBallGroup[0]) ? (ballGroupSize * 3 + 1) : ballGroupSize * 3;
        }
    }
}
