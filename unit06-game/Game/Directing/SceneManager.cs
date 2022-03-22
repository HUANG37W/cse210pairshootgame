using System;
using System.Collections.Generic;
using System.IO;
using Unit06.Game.Casting;
using Unit06.Game.Scripting;
using Unit06.Game.Services;


namespace Unit06.Game.Directing
{
    public class SceneManager
    {
        public static AudioService AudioService = new RaylibAudioService();
        public static KeyboardService KeyboardService = new RaylibKeyboardService();
        // public static MouseService MouseService = new RaylibMouseService();
        public static PhysicsService PhysicsService = new RaylibPhysicsService();
        public static VideoService VideoService = new RaylibVideoService(Constants.GAME_NAME,
            Constants.SCREEN_WIDTH, Constants.SCREEN_HEIGHT, Constants.GREEN);

        public SceneManager()
        {
        }

        public void PrepareScene(string scene, Cast cast, Script script)
        {
            if (scene == Constants.NEW_GAME)
            {
                PrepareNewGame(cast, script);
            }
            else if (scene == Constants.NEXT_LEVEL)
            {
                PrepareNextLevel(cast, script);
            }
            else if (scene == Constants.TRY_AGAIN)
            {
                PrepareTryAgain(cast, script);
            }
            else if (scene == Constants.IN_PLAY)
            {
                PrepareInPlay(cast, script);
            }
            else if (scene == Constants.GAME_OVER)
            {
                PrepareGameOver(cast, script);
            }
        }

        private void PrepareNewGame(Cast cast, Script script)
        {
            AddStats(cast);
            AddLives1(cast);
            AddLives2(cast);
            //AddBowFighter(cast);
            //AddBear(cast);
            AddSelector(cast);
            AddDialog(cast, Constants.ENTER_TO_START);

            script.ClearAllActions();
            AddInitActions(script);
            AddLoadActions(script);

            ChangeSceneAction a = new ChangeSceneAction(KeyboardService, Constants.NEXT_LEVEL);
            script.AddAction(Constants.INPUT, a);

            AddOutputActions(script);
            AddUnloadActions(script);
            AddReleaseActions(script);
        }

        private void ActivateSelector(Cast cast)
        {
            Selector selector = (Selector)cast.GetFirstActor(Constants.SELECTOR_GROUP);
            selector.MoveNext();
        }

        private void PrepareNextLevel(Cast cast, Script script)
        {
            // AddFighter(cast);
            // AddBowFighter(cast);
            AddSelector(cast);
            AddDialog(cast, Constants.PREP_TO_LAUNCH);

            script.ClearAllActions();

            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);

            AddOutputActions(script);

            PlaySoundAction sa = new PlaySoundAction(AudioService, Constants.WELCOME_SOUND);
            script.AddAction(Constants.OUTPUT, sa);
        }

        private void PrepareTryAgain(Cast cast, Script script)
        {
            //AddBall(cast);
            AddSelector(cast);
            AddDialog(cast, Constants.PREP_TO_LAUNCH);

            script.ClearAllActions();
            
            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.IN_PLAY, 2, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);
            
            AddUpdateActions(script);
            AddOutputActions(script);
        }

        private void PrepareInPlay(Cast cast, Script script)
        {
            //ActivateBall(cast);
            cast.ClearActors(Constants.DIALOG_GROUP);

            script.ClearAllActions();

            ControlSelectorAction action = new ControlSelectorAction(KeyboardService);
            script.AddAction(Constants.INPUT, action);

            AddUpdateActions(script);
            AddOutputActions(script);
        
        }

        private void PrepareGameOver(Cast cast, Script script)
        {
            //AddBall(cast);
            AddSelector(cast);
            AddDialog(cast, Constants.WAS_GOOD_GAME);

            script.ClearAllActions();

            TimedChangeSceneAction ta = new TimedChangeSceneAction(Constants.NEW_GAME, 5, DateTime.Now);
            script.AddAction(Constants.INPUT, ta);

            AddOutputActions(script);
        }

        // -----------------------------------------------------------------------------------------
        // casting methods
        // -----------------------------------------------------------------------------------------

        // private void AddFighter(Cast cast)
        // {
        //     cast.ClearActors(Constants.FIGHTER_GROUP);
        
        //     int x = Constants.CENTER_X - Constants.FIGHTER_WIDTH / 2;
        //     int y = Constants.SCREEN_HEIGHT - Constants.Selector_HEIGHT - Constants.FIGHTER_HEIGHT;
        
        //     Point position = new Point(x, y);
        //     Point size = new Point(Constants.FIGHTER_WIDTH, Constants.FIGHTER_HEIGHT);
        //     Point velocity = new Point(0, 0);
        
        //     Body body = new Body(position, size, velocity);
        //     Image image = new Image(Constants.FIGHTER_IMAGE);
        //     Ball ball = new Ball(body, image, false);
        
        //     cast.AddActor(Constants.FIGHTER_GROUP, fighter);
        // }

        // private void AddBricks(Cast cast)
        // {
        //     cast.ClearActors(Constants.BRICK_GROUP);

        //     Stats stats = (Stats)cast.GetFirstActor(Constants.STATS_GROUP);
        //     int level = stats.GetLevel() % Constants.BASE_LEVELS;
        //     string filename = string.Format(Constants.LEVEL_FILE, level);
        //     List<List<string>> rows = LoadLevel(filename);

        //     for (int r = 0; r < rows.Count; r++)
        //     {
        //         for (int c = 0; c < rows[r].Count; c++)
        //         {
        //             int x = Constants.FIELD_LEFT + c * Constants.BRICK_WIDTH;
        //             int y = Constants.FIELD_TOP + r * Constants.BRICK_HEIGHT;

        //             string color = rows[r][c][0].ToString();
        //             int frames = (int)Char.GetNumericValue(rows[r][c][1]);
        //             int points = Constants.BRICK_POINTS;

        //             Point position = new Point(x, y);
        //             Point size = new Point(Constants.BRICK_WIDTH, Constants.BRICK_HEIGHT);
        //             Point velocity = new Point(0, 0);
        //             List<string> images = Constants.BRICK_IMAGES[color].GetRange(0, frames);

        //             Body body = new Body(position, size, velocity);
        //             Animation animation = new Animation(images, Constants.BRICK_RATE, 1);
                    
        //             Brick brick = new Brick(body, animation, points, false);
        //             cast.AddActor(Constants.BRICK_GROUP, brick);
        //         }
        //     }
        // }

        private void AddDialog(Cast cast, string message)
        {
            cast.ClearActors(Constants.DIALOG_GROUP);

            Text text = new Text(message, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_CENTER, Constants.WHITE);
            Point position = new Point(Constants.CENTER_X, Constants.CENTER_Y);

            Label label = new Label(text, position);
            cast.AddActor(Constants.DIALOG_GROUP, label);   
        }

        /// <summary>
        /// Adds player 1's lives to the cast to be be put on top left of HUD
        /// </summary>
        private void AddLives1(Cast cast)
        {
            cast.ClearActors(Constants.LIVES1_GROUP);

            Text text = new Text(Constants.LIVES1_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_LEFT, Constants.WHITE);
            Point position = new Point(Constants.HUD_MARGIN, Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.LIVES1_GROUP, label);
        }

        /// <summary>
        /// Adds player 2's lives to the cast to be be put on top left of HUD
        /// </summary>
        private void AddLives2(Cast cast)
        {
            cast.ClearActors(Constants.LIVES2_GROUP);

            Text text = new Text(Constants.LIVES2_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
                Constants.ALIGN_RIGHT, Constants.WHITE);
            Point position = new Point(Constants.SCREEN_WIDTH - Constants.HUD_MARGIN, 
                Constants.HUD_MARGIN);

            Label label = new Label(text, position);
            cast.AddActor(Constants.LIVES2_GROUP, label);   
        }

        private void AddSelector(Cast cast)
        {
            cast.ClearActors(Constants.SELECTOR_GROUP);
        
            
            int x = Constants.SELECTOR_WIDTH / 4;
            int y = Constants.SCREEN_HEIGHT / 3;
        
            Point position = new Point(x, y);
            Point size = new Point(Constants.SELECTOR_WIDTH, Constants.SELECTOR_HEIGHT);
            Point velocity = new Point(0, 0);
        
            Body body = new Body(position, size, velocity);
            Animation animation = new Animation(Constants.SELECTOR_IMAGES, 0, Constants.SELECTOR_RATE);
            Selector selector = new Selector(body, animation, true);
        
            cast.AddActor(Constants.SELECTOR_GROUP, selector);
        }

        // private void AddScore(Cast cast)
        // {
        //     cast.ClearActors(Constants.SCORE_GROUP);

        //     Text text = new Text(Constants.SCORE_FORMAT, Constants.FONT_FILE, Constants.FONT_SIZE, 
        //         Constants.ALIGN_CENTER, Constants.WHITE);
        //     Point position = new Point(Constants.CENTER_X, Constants.HUD_MARGIN);
            
        //     Label label = new Label(text, position);
        //     cast.AddActor(Constants.SCORE_GROUP, label);   
        // }

        private void AddStats(Cast cast)
        {
            cast.ClearActors(Constants.STATS_GROUP);
            Stats stats = new Stats();
            cast.AddActor(Constants.STATS_GROUP, stats);
        }

        private List<List<string>> LoadLevel(string filename)
        {
            List<List<string>> data = new List<List<string>>();
            using(StreamReader reader = new StreamReader(filename))
            {
                while (!reader.EndOfStream)
                {
                    string row = reader.ReadLine();
                    List<string> columns = new List<string>(row.Split(',', StringSplitOptions.TrimEntries));
                    data.Add(columns);
                }
            }
            return data;
        }

        // -----------------------------------------------------------------------------------------
        // scriptig methods
        // -----------------------------------------------------------------------------------------

        private void AddInitActions(Script script)
        {
            script.AddAction(Constants.INITIALIZE, new InitializeDevicesAction(AudioService, 
                VideoService));
        }

        private void AddLoadActions(Script script)
        {
            script.AddAction(Constants.LOAD, new LoadAssetsAction(AudioService, VideoService));
        }

        private void AddOutputActions(Script script)
        {
            script.AddAction(Constants.OUTPUT, new StartDrawingAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawHudAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawFighterAction(VideoService));
            //script.AddAction(Constants.OUTPUT, new DrawBricksAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawSelectorAction(VideoService));
            script.AddAction(Constants.OUTPUT, new DrawDialogAction(VideoService));
            script.AddAction(Constants.OUTPUT, new EndDrawingAction(VideoService));
        }

        private void AddUnloadActions(Script script)
        {
            script.AddAction(Constants.UNLOAD, new UnloadAssetsAction(AudioService, VideoService));
        }

        private void AddReleaseActions(Script script)
        {
            script.AddAction(Constants.RELEASE, new ReleaseDevicesAction(AudioService, 
                VideoService));
        }

        private void AddUpdateActions(Script script)
        {
            script.AddAction(Constants.UPDATE, new MoveFighterAction());
            script.AddAction(Constants.UPDATE, new MoveSelectorAction());
            script.AddAction(Constants.UPDATE, new CollideBordersAction(PhysicsService, AudioService));
            //script.AddAction(Constants.UPDATE, new CollideBrickAction(PhysicsService, AudioService));
            //script.AddAction(Constants.UPDATE, new CollideSelectorAction(PhysicsService, AudioService));
            //script.AddAction(Constants.UPDATE, new CheckOverAction());     
        }
    }
}