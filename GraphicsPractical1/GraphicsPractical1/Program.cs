using System;

namespace GraphicsPractical1
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// 
        /// BONUS:
        /// 
        /// 
        /// </summary>
        static void Main(string[] args)
        {
            
            #region BONUS
            // Handling the input args
            // Setting the default values for each of the inputs
            
            int width = 800;
            int height = 600;
            float fov = 60;
            bool screen = false;
            string controls = "keyboard";

            for (int i = 0; i < args.Length - 1; i += 2)
            {
                switch (args[i])
                {
                    case "-w":
                        width = Convert.ToInt32(args[i + 1]);
                        break;
                    case "-h":
                        height = Convert.ToInt32(args[i + 1]);
                        break;
                    case "-fov":
                        fov = (float)Convert.ToDouble(args[i + 1]);
                        break;
                    case "-s":
                        if (args[i + 1] == "fullscreen")
                            screen = true;
                        break;
                    case "-c":
                        controls = args[i + 1];
                        break;
                }
            }
            #endregion
            
            // BONUS:
            // Edited the Game() struct to accept multiple input variables.
            using (Game1 game = new Game1(fov, width, height, screen,controls))
            {
                game.Run();
            }

        }
    }
#endif
}

