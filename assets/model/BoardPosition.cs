using System.Text.Json;

namespace Fold.Motor.Model; 

public sealed class BoardPosition : IEquatable<BoardPosition> {
    // you can change where the alphabet starts and ends when formating,
    // ex. changing 'a'->'A' to make the system work with upper case,
    // same with the final character
    private static readonly char _startingAlphabetCharacter = 'a', _finalAlphabetCharacter = 'z';
    private static readonly int _baseFormatLetter = 26;

    public readonly int x, y;
    private readonly string _formatedPosition;

    #region IEquatable
    public override int GetHashCode() {
        return Board.SIZE_X * y + x;
    }

    public override bool Equals(object? other) {
        return other != null && Equals((BoardPosition)other);
    }

    public bool Equals(BoardPosition? other) {
        return other != null && other.GetHashCode() == this.GetHashCode();
    }
    #endregion

    // transform formated, ex. "a1", "h23", "az1" to BoardPosition
    public BoardPosition(string? formatedPosition) {
        if(formatedPosition == null)
            throw new NullReferenceException();

        this._formatedPosition = formatedPosition;
        // we are going to interate through each character
        char[] formatedPositionCharArray = formatedPosition.ToCharArray();
        int charCount = formatedPosition.Length;

        Exception? exception = null;

        // TODO : somehow fix so that aaa isn't 0?
        // getting x position
        int calculatedX = 0;
        bool hasCalculatedX = false;
        int index = 0;
        while(index < charCount && !hasCalculatedX) {
            char character = formatedPositionCharArray[index];
            
            // if character is in letter range
            if(character >= _startingAlphabetCharacter) {
                if(character <= _finalAlphabetCharacter) {
                    // increase order of magnitude
                    calculatedX *= _baseFormatLetter;
                    // add character as int
                    calculatedX += (int)(character - _startingAlphabetCharacter);
                    index++;
                } else {
                    hasCalculatedX = true;
                    exception = new FormatException("Board x position character over upper bound!");
                }
            } else {
                // if it hasn't calculated any x yet...
                if(index == 0) {
                    hasCalculatedX = true;
                    exception = new FormatException("Board x position character under lower bound!");
                }
                // else tell the while loop to stop calculating x
                else hasCalculatedX = true;
            }
        }
        if(exception != null) throw exception;

        // set x attribute
        this.x = calculatedX;

        // getting y position
        int calculatedY = 0;
        while(index < charCount) {
            char character = formatedPositionCharArray[index];

            if(character >= '0') {
                if(character <= '9') {
                    // if it has already a value...
                    calculatedY *= 10;

                    // add character as int
                    calculatedY += (int)(character - '0');
                    index++;

                } else {
                    index = charCount;
                    exception = new FormatException("Board y position character over upper bound!");
                }
            } else {
                index = charCount;
                exception = new FormatException("Board y position character under lower bound!");
            }
        }
        if(exception != null) throw exception;

        if(calculatedY < 1) throw new FormatException("Board y position character under lower bound!");

        // set y attribute
        this.y = calculatedY - 1;
    }

    // gets the position formated
    public override string ToString() => _formatedPosition;

    // turn a string array into a board position array
    public static BoardPosition[] FormatedPositionStringArrayToBoardPositionArray(string[] formatedPositionArray, int count) {
        BoardPosition[] boardPositionArray = new BoardPosition[count];
        
        for (int i = 0; i < count; i++) {
            BoardPosition newboardPosition = new BoardPosition(formatedPositionArray[i]);
            boardPositionArray[i] = newboardPosition;
        }

        return boardPositionArray;
    }

    // returns distance between two positions
    public static bool AreAdjacent(BoardPosition bp1, BoardPosition bp2) {
        if(bp1 == bp2) return false;
        return MathF.Abs(bp1.x - bp2.x) <= 1 && MathF.Abs(bp1.y - bp2.y) <= 1;
    }
}