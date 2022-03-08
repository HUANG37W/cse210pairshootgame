using System;
using System.Collections.Generic;
using System.Data;
using Unit05.Game.Casting;
using Unit05.Game.Services;


namespace Unit05.Game.Scripting
{
    /// <summary>
    /// <para>An update action that handles interactions between the actors.</para>
    /// <para>
    /// The responsibility of HandleCollisionsAction is to handle the situation when the Cycler 
    /// collides with the food, or the Cycler collides with its segments, or the game is over.
    /// </para>
    /// </summary>
    public class HandleCollisionsAction : Action
    {
        private bool isGameOver = false;

        /// <summary>
        /// Constructs a new instance of HandleCollisionsAction.
        /// </summary>
        public HandleCollisionsAction()
        {
        }

        /// <inheritdoc/>
        public void Execute(Cast cast, Script script)
        {
            if (isGameOver == false)
            {
                // HandleFoodCollisions(cast);
                HandleSegmentCollisions(cast);
                HandleGameOver(cast);
            }
        }

        /// <summary>
        /// Updates the score nd moves the food if the Cycler collides with it.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        // private void HandleFoodCollisions(Cast cast)
        // {
        //     Cycler cycler = (Cycler)cast.GetFirstActor("cycler");
        //     Score score = (Score)cast.GetFirstActor("score");
        //     // Food food = (Food)cast.GetFirstActor("food");
            
        //     // if (Cycler.GetHead().GetPosition().Equals(food.GetPosition()))
        //     // {
        //     //     int points = food.GetPoints();
        //     //     Cycler.GrowTail(points);
        //     //     score.AddPoints(points);
        //     //     food.Reset();
        //     // }
        // }

        /// <summary>
        /// Sets the game over flag if the Cycler collides with one of its segments.
        /// </summary>
        /// <param name="cast">The cast of actors.</param>
        private void HandleSegmentCollisions(Cast cast)
        {
            Cycler cycler = (Cycler)cast.GetFirstActor("cycler");
            Cycler cycler2 = (Cycler)cast.GetFirstActor("cycler2");
            Actor head = cycler.GetHead();
            Actor head2 = cycler2.GetHead();
            List<Actor> body = cycler.GetBody();
            List<Actor> body2 = cycler2.GetBody();

            foreach (Actor segment in body)
            {
                if (segment.GetPosition().Equals(head.GetPosition()))
                {
                    isGameOver = false;
                }
            }
        }

        private void HandleGameOver(Cast cast)
        {
            if (isGameOver == true)
            {
                Cycler cycler = (Cycler)cast.GetFirstActor("cycler");
                List<Actor> segments = cycler.GetSegments();
                Cycler cycler2 = (Cycler)cast.GetFirstActor("cycler2");
                // List<Actor> segments = cycler2.GetSegments();
                // Food food = (Food)cast.GetFirstActor("food");

                // create a "game over" message
                int x = (Constants.MAX_X / 2) - 40;
                int y = Constants.MAX_Y / 2;
                Point position = new Point(x, y);

                Actor message = new Actor();
                message.SetText("Game Over!");
                message.SetPosition(position);
                cast.AddActor("messages", message);

                // make everything white
                foreach (Actor segment in segments)
                {
                    segment.SetColor(Constants.WHITE);
                }
                // food.SetColor(Constants.WHITE);
            }
        }

    }
}