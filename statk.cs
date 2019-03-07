using System;
using static System.Console;
using System.Threading;

/*
 * includes diagonal jumps
 */

namespace ConsoleApp1
{
    class Program
    {
		static char[,] xy;
		
		static int manX;
		static int manY;
		static Random rnd = new Random();
		static int score = 0;
		static int prevBlock;

		static bool jumpingRight = false;
		static bool jumpingLeft = false;
		
		static bool gameEnd = false;
		static int sleepInt = 500;

		static bool hasMoved = false;


        static void Main(string[] args)
        {
            WriteLine("Hello World!");

			SetupGame();
			DisplayMenu();
			drawXy();
			Step1();
			enterMove();
        }


		static void Step1()
		{
			new Thread(() =>
			{
				Thread.CurrentThread.IsBackground = true;

				while (gameEnd == false )
				{
					Thread.Sleep(sleepInt);
					playGame();
				}
				gameOver();
			}).Start();
		}


		static void gameOver()
		{
			Clear();
			drawXy();
			WriteLine( "game over!");
			WriteLine();
			WriteLine("score : " + score );
		}


		static void enterMove()
		{
            while( gameEnd == false )
            {
                ConsoleKey what1 = ReadKey(true).Key;
                if ( what1 == ConsoleKey.Escape )
                {
                    gameEnd = true;
                }
                
                if( ( gameEnd == false) && ( hasMoved == false ) )
                {
					PlayerMove( what1 );
					
					drawXy();
					WriteLine();
					WriteLine();
					WriteLine("score : " + score );
					WriteLine();
				}
			}
		}


		static void PlayerMove( ConsoleKey nextMove )
		{
			if( ( nextMove == ConsoleKey.LeftArrow ) || (nextMove == ConsoleKey.N ) )
			{
				moveLeft();
			}
			
			if( (nextMove == ConsoleKey.RightArrow) || (nextMove == ConsoleKey.M) )
			{
				moveRight();
			}

			if( ( nextMove == ConsoleKey.UpArrow ) || (nextMove == ConsoleKey.J ) )
			{
				jump();
			}

			if( ( nextMove == ConsoleKey.G ) || ( nextMove == ConsoleKey.K ) )
			{
				jumpRight();
			}

			if( ( nextMove == ConsoleKey.L ) || ( nextMove == ConsoleKey.H ) )
			{
				jumpLeft();
			}
			
			hasMoved = true;
		}


        static void playGame()
        {
			if( hasMoved == false )
			{
				fall();
			}
			
			hasMoved = false;
						
			gameEnd = blocks();

			if( gameEnd )
			{
				WriteLine( "game over!");
			}
			else
			{
				clearBlocks();
				addBlock();
			}
			drawXy();
			WriteLine();
			WriteLine();
			WriteLine("score : " + score );
			WriteLine();

		}
		
		
        static void clearBlocks()
        {
			Boolean clearLine = true;
            for (int col = 1; col < xy.GetLength(0); col++)
            {
				if( ( xy[col, xy.GetLength(1) - 2] == ' ' ) || ( xy[col, xy.GetLength(1) - 2] == 'X' ) )
				{
					clearLine = false;
				}
			}
			if( clearLine == true )
			{
				for (int col = 1; col < xy.GetLength(0) - 1 ; col++)
				{
					xy[ col, xy.GetLength(1) - 2 ] = ' ';
				}
				score++;
			}
		}
		
		
        static void addBlock()
        {
			int difficulty = rnd.Next( 2 ); //23
			int letter = rnd.Next( 26 ); 
			if( letter == 23 )
				letter = 25;
			
			if ( difficulty == 0 )
			{
				int block = rnd.Next( xy.GetLength(0) - 2 ); 
				if( ( block + 1 != prevBlock ) && ( block - 1 != prevBlock ) )
				{
					if( xy[ block + 1, 1] == ' ' )
					{
						xy[ block + 1, 1]  = (char)(97 + letter );
						prevBlock = block;
					}
				}
			}
		}
		
		
        static void fall()
        {
            xy[manX, manY]  = ' ';
            xy[manX, manY-1]  = ' ';

			if( jumpingRight == true )
			{
				if( xy[manX + 1, manY + 1] != ' ' )   //   below right foot is not empty
				{
					if( xy[manX + 1, manY - 1] == ' ' ) // right head is empty
					{
						if( xy[manX + 1, manY] != ' ' ) // right foot is block  
						{
							xy[manX + 2, manY] = xy[manX + 1, manY];
						}
						manX = manX + 1;
					}
					else // right head is not empty
					{
						manY = manY + 1;
					}
				}
				else  //   below right foot is empty
				{
					if(xy[manX + 1, manY] != ' ' )  // block at right foot 
					{
						if(xy[manX + 2, manY] == ' ' ) // to right of right foot is empty
						{
							xy[manX + 2, manY] = char.ToUpper( xy[manX + 1, manY] ) ;   // move block set as processed
							manX = manX + 1;
							manY = manY + 1;
						}
						else
						{
							manY = manY + 1; // man fall
						}
					}
					else
					{
						manX = manX + 1;
						manY = manY + 1;
					}
				}
				jumpingRight = false;
			}
			
			else if( jumpingLeft == true )
			{
				if( xy[manX - 1, manY + 1] != ' ' )   //   below left foot is not empty
				{
					if( xy[manX - 1, manY - 1] == ' ' ) // left head is empty
					{
						if( xy[manX - 1, manY] != ' ' ) // left foot is block  
						{
							xy[manX - 2, manY] = xy[manX - 1, manY];
						}
						manX = manX - 1;
					}
					else // left head is not empty
					{
						manY = manY - 1;
					}
				}
				else  //   below left foot is empty
				{
					if(xy[manX - 1, manY] != ' ' )  // block at left foot 
					{
						if(xy[manX - 2, manY] == ' ' ) // to left of left foot is empty
						{
							xy[manX - 2, manY] = char.ToUpper( xy[manX - 1, manY] ) ;   // move block set as processed
							manX = manX - 1;
							manY = manY + 1;
						}
						else
						{
							manY = manY + 1; // man fall
						}
					}
					else
					{
						manX = manX - 1;
						manY = manY + 1;
					}
				}
				jumpingLeft = false;
			}
			
            else  // jumpingRight == false && jumpingLeft == false
            {
				if( xy[manX, manY + 1] == ' ' )
				{
					manY = manY + 1;
				}
			}
			xy[manX, manY]  = 'X';
            xy[manX, manY-1]  = '@';
		}
		

        static void jumpRight()
        {
            xy[manX, manY]  = ' ';
            xy[manX, manY-1]  = ' ';

			if( jumpingRight )  //  not on empty space ( on ground... )
			{
				fall();
				return ;
			}

			if( xy[manX, manY + 1] != ' ' )  //  not on empty space ( on ground... )
			{
				// is right head empty
				// true
				//   is right foot empty
				//   true
				//     jump right
				//   false
				//     is right of right foot empty
				//     true 
				//       jump right 
				//     false
				//       dont
				// false
				//   dont
				
				if( ( xy[manX + 1, manY - 1] == ' ' )  && // right head empty  
				    ( xy[manX + 2, manY - 3] == ' ' ) &&  //  2 up, 2 right, from head, is empty   // change this...
				    ( manX + 3 < xy.GetLength(0) ) )  //  man is away from right boundary
				{
					if( xy[manX + 1, manY ] == ' ' )  // right foot empty
					{
						// jump right
						manX = manX + 1;
						manY = manY - 1;
						jumpingRight = true;
					}
					else    //  right foot blocked
					{
						if( xy[manX + 2, manY ] == ' ' )  // is right of right foot empty
						{
							// jump right
							xy[manX + 2, manY] = char.ToUpper( xy[manX + 1, manY] ) ;   // move block set as processed
							xy[manX + 1, manY] = ' ';

							manX = manX + 1;
							manY = manY - 1;
							jumpingRight = true;
						}
						else  // right of right foot blocked
						{
							//nothing
						}
					}
				}
				else   // right head blocked
				{
					//nothing
				}
			}
			else   // is on empty space
			{
				manY = manY + 1;  // man falls
			}
			
			xy[manX, manY]  = 'X';
			xy[manX, manY-1]  = '@';
		}


        static void jumpLeft()
        {
            xy[manX, manY]  = ' ';
            xy[manX, manY-1]  = ' ';

			if( jumpingLeft )  //  not on empty space ( on ground... )
			{
				fall();
				return ;
			}
			
			if( xy[manX, manY + 1] != ' ' )  //  not on empty space ( on ground... )
			{
				// is left head empty
				// true
				//   is left foot empty
				//   true
				//     jump left
				//   false
				//     is left of left foot empty
				//     true 
				//       jump left 
				//     false
				//       dont
				// false
				//   dont
				
				if( ( xy[manX - 1, manY - 1] == ' ' )  && // left head empty  
				    ( xy[manX - 2, manY - 3] == ' ' ) &&  //  2 up, 2 left, from head, is empty   // change this...
				    ( manX - 3 < xy.GetLength(0) ) )  //  man is away from left boundary
				{
					if( xy[manX - 1, manY ] == ' ' )  // left foot empty
					{
						// jump left
						manX = manX - 1;
						manY = manY - 1;
						jumpingLeft = true;
					}
					else    //  left foot blocked
					{
						if( xy[manX - 2, manY ] == ' ' )  // is left of left foot empty
						{
							// jump left
							xy[manX - 2, manY] = char.ToUpper( xy[manX - 1, manY] ) ;   // move block set as processed
							xy[manX - 1, manY] = ' ';

							manX = manX - 1;
							manY = manY - 1;
							jumpingLeft = true;
						}
						else  // left of left foot blocked
						{
							//nothing
						}
					}
				}
				else   // left head blocked
				{
					//nothing
				}
			}
			else   // is on empty space
			{
				manY = manY + 1;  // man falls
			}
			
			xy[manX, manY]  = 'X';
			xy[manX, manY-1]  = '@';
		}


        static void jump()
        {
            xy[manX, manY]  = ' ';
            xy[manX, manY-1]  = ' ';

			if( xy[manX, manY + 1] != ' ' )
			{
				manY = manY - 1;
				xy[manX, manY-2]  = ' ';
			}
			else
			{
				manY = manY + 1;
			}

            xy[manX, manY]  = 'X';
            xy[manX, manY-1]  = '@';
		}

		
        static void moveLeft()
        {
            xy[manX, manY]  = ' ';
            xy[manX, manY-1]  = ' ';

			if( xy[manX, manY + 1] == ' ' )   //  not on ground 
			{
				if( ( xy[manX - 1, manY + 1] != ' ' ) && ( xy[manX - 1, manY + 1] != '#' ) )   //   below left foot is not empty and not boundary
				{
					if( isSmall( xy[manX - 1, manY] ) )   // block is at left foot
					{
						if( xy[manX - 1, manY - 1] == ' ' )  // is empty space to left of head
						{
							if( xy[manX - 2, manY] == ' ' )  // is empty to left of block
							{
								xy[manX - 2, manY] = char.ToUpper( xy[manX - 1, manY] ) ;   // move block set as processed
								manX = manX - 1;   // man goes left
							}
							else
							{
								manY = manY + 1;  // man falls
							}
						}
						else
						{
							manY = manY + 1;  // man falls
						}
					}
					else
					{	
						manX = manX - 1;  // man goes left
					}
				}
				else
				{
					manY = manY + 1;  // man falls
				}
			}
			else    // is on ground
			{
				if( manX > 1 )  // man away from left boundary
				{
					if( ( xy[manX - 1, manY] == ' ' ) && ( xy[manX - 1, manY - 1] == ' ' ) )  // left foot and left head  are empty
					{
						manX = manX - 1;  //  man goes left 
					}
					else
					{
						if( ( xy[manX - 2, manY] == ' ' ) && ( xy[manX - 1, manY - 1] == ' ' ) )
						{
							xy[manX - 2, manY] = char.ToUpper( xy[manX - 1, manY] );   // move block set as processed
							manX = manX - 1; // man goes left 
						}
					}
				}
			}
			xy[manX, manY]  = 'X';
			xy[manX, manY-1]  = '@';
		}

		
        static void moveRight()
        {
            xy[manX, manY]  = ' ';
            xy[manX, manY-1]  = ' ';
			if( xy[manX, manY + 1] == ' ' )   // not on ground
			{
				if( ( xy[manX + 1, manY + 1] != ' ' ) && ( xy[manX + 1, manY + 1] != '#' ) )   //   below right foot is not empty and not boundary
				{
					if( isSmall( xy[manX + 1, manY] ) )  // block at right foot
					{
						if( xy[manX + 1, manY - 1] == ' ' )  // is empty space to right of head
						{
							if( xy[manX + 2, manY] == ' ' )  // is empty to the right of block
							{
								xy[manX + 2, manY] = char.ToUpper( xy[manX + 1, manY] ) ;
								manX = manX + 1;
							}
							else
							{
								manY = manY + 1;   // man falls 
							}
						}
						else   // block to right of head
						{
							manY = manY + 1;  // man falls
						}
					}
					else
					{	
						manX = manX + 1;  // man goes right
					}
				}
				else
				{
					manY = manY + 1;  // man goes right
				}
			}
			else   // is on ground
			{
				if( manX < xy.GetLength(0) -2 )
				{
					if( ( xy[manX + 1, manY] == ' ' ) && ( xy[manX + 1, manY - 1] == ' ' ) )  // right foot and right head empty
					{
						manX = manX + 1;     // man goes right
					}
					else
					{
						if( ( xy[manX + 2, manY] == ' ' )  &&  ( xy[manX + 1, manY - 1] == ' ' ) )  //right of right foot, and right head empty
						{
							xy[manX + 2, manY] = char.ToUpper( xy[manX + 1, manY] ) ;   //   block has been processed
							manX = manX + 1;      // man goes right
						}
					}
				}
			}
            xy[manX, manY]  = 'X';
            xy[manX, manY-1]  = '@';
		}


        static Boolean isSmall( char c )
        {
			int cInt = (int)c;
			if( ( cInt <= 122 ) && ( cInt >= 97 ) )
			{
				return true;
			}
			return false;
		}
		

        static Boolean isBig( char c )
        {
			int cInt = (int)c;
			if( ( cInt <= 90 ) && ( cInt >= 65 ) )
			{
				return true;
			}
			return false;
		}
		
		
        static Boolean blocks()
        //returns true for game over
        {
			for (int row = xy.GetLength(1) - 2; row >= 0 ; row--)
			{
				for ( int col = 0; col < xy.GetLength(0) - 1; col++ )
				{
					if( isSmall( xy[col, row] ) )
					{
						if( xy[col, row + 1] == ' ' )
						{
							xy[col, row + 1] = xy[col, row];
							xy[col, row] = ' ';
						}
						
						if( xy[col, row + 1] == '@' )
						{


							if( jumpingRight || jumpingLeft )
							{
								xy[col, row] = ' ';
								return false;
							}
							else
							{
								xy[col, row + 1] = '*';
								xy[col, row] = ' ';
								return true;
							}
						}
					}
					else
					{
						if( isBig( xy[col, row] ) )
						{
							if( xy[col, row] != 'X' )
							{
								xy[col, row] = char.ToLower( xy[col, row] ) ;
							}
						}
					}
                }
            }
            return false; 
		}
		
		
        static void drawXy()  // draw the array
        {
			Clear(); // Clear the screen
			WriteLine();
			WriteLine();
			WriteLine();
			WriteLine();
			for (int row = 0; row < xy.GetLength(0); row++)
			{
				for (int col = 0; col < xy.GetLength(1); col++)
				{
					string spacer = "";
					if( col == 0 )
					{
						spacer = "             ";
					}
                    Write( spacer + "" + xy[col, row]  + " " );
                }
                WriteLine();
            }
            WriteLine();
            WriteLine();
            WriteLine();
            WriteLine();
            WriteLine();
		}
        
        
        static void DisplayMenu()  // init
        {
			Clear();
			string spacer = "             ";
			WriteLine( "\n\n\n" );
			
            for (int row = 0; row < 12; row++)
            {
				Write( spacer );
				if( row == 0 || row == 11 )
				{	
					Write(  "# # # # # # # # # # # #" );
				}
				else if( row == 3 )
				{
					Write( "#     statk           #" );
				}
				else if( row == 7 )
				{
					Write( "#  arrow keys,        #" );
				}
				else if( row == 8 )
				{
					Write( "#   h,j,k,            #" );
				}
				else if( row == 9 )
				{
					Write( "#    n,m              #" );
				}
				else
				{
					Write( "#                     #" );
				}
				WriteLine();
			}
			ConsoleKey nzz = ReadKey(true).Key;
		}
        
        
        static void SetupGame()  // init
        {
			WriteLine( "init seq..." );
			
			xy = new char[12, 12];
			
			manX = 1 + rnd.Next( xy.GetLength(0) - 2 ); 
//			manX = xy.GetLength(0) / 2;
			manY = xy.GetLength(1) - 2;			
			
            for (int col = 0; col < xy.GetLength(0); col++)
            {
                for (int row = 0; row < xy.GetLength(1); row++)
                {
					xy[col, row] = ' ';
					
					if( row == 0 )
					{
						xy[col, row] = '#';
					}
					if( row == xy.GetLength(0) - 1 )
					{
						xy[col, row] = '#';
					}
					if( col == 0 )
					{
						xy[col, row] = '#';
                    }
					if( col == xy.GetLength(0) - 1 )
					{
						xy[col, row] = '#';
                    }
                }
            }

            xy[manX, manY]  = 'X';
            xy[manX, manY-1]  = '@';

            xy[5, 4]  = 'a';
		}
    }
}
