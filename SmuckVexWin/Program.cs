using System;
using Microsoft.Xna.Framework.Content;

namespace Smuck
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
			using (SmuckGame game = new SmuckGame())
			{
				game.Run();
			}			
        }
    }
}