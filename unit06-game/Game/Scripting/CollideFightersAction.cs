using Unit06.Game.Casting;
using Unit06.Game.Services;
using System.Collections.Generic;

namespace Unit06.Game.Scripting
{
    /// <summary>
    /// Handles the outcomes when two or more fighters from opposing
    /// sides collide or battle each other.
    /// </summary>
    public class CollideFightersAction : Action
    {
        private AudioService audioService;
        private PhysicsService physicsService;

        public CollideFightersAction(PhysicsService physicsService, AudioService audioService)
        {
            this.physicsService = physicsService;
            this.audioService = audioService;
        }

         public void Execute(Cast cast, Script script, ActionCallback callback)
        {
            Fighter fighter = (Fighter)cast.GetFirstActor(Constants.FIGHTER_GROUP);
            //Point sp1 = new Point(50, fighter.GetBody().GetPosition().GetY());

        // public void Execute(Cast cast, Script script, ActionCallback actionCallback)
        // {
        //     List<Actor> bowfighters = cast.GetActors(Constants.BOW_FIGHTER_GROUP);
        //     List<Actor> bears = cast.GetActors(Constants.BEAR_GROUP);
        //     List<Actor> swordfighters = cast.GetActors(Constants.FIGHTER_GROUP);

        //     Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);

            
            // Body bearbody = bears.GetBody();
            // Body swordfightersbody = swordfighters.GetBody();
            // if (physicsService.HasCollided(bearbody,swordfightersbody))
            // {
            //     Sound sound = new Sound(Constants.BEAR_GROWL_SOUND);
            //     audioService.PlaySound(sound);
            //     int points = bears.GetPoints();
            //     stats.AddPoints(points);
            //     cast.RemoveActor(Constants.FIGHTER_GROUP, swordfighters);
            // }

            
            //maybe try a for each loop to loop through every fighter that could collide with one another.
        }
    }
}